using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DTOs;
//using ABI.Windows.Security.EnterpriseData;
using LogicUnit.Logic.GamePageLogic;
using LogicUnit.Logic.GamePageLogic.LiteNet;
using Microsoft.AspNetCore.SignalR.Client;
using Objects;
using Objects.Enums;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;

namespace LogicUnit
{
    public abstract partial class Game
    {
        private List<string> m_PlayerMovementsLogs = new List<string>();

        private readonly HubConnection r_ConnectionToServer;
        //protected readonly LiteNetClient r_LiteNetClient = LiteNetClient.Instance;

        //Events
        public event EventHandler<List<GameObject>> AddGameObjectList;
        public event EventHandler<List<GameObject>> GameObjectsUpdate;
        public event EventHandler<GameObject> GameObjectToDelete;
        public event EventHandler<List<int>> GameObjectsToHide;
        public event EventHandler<List<int>> GameObjectsToShow;

        //basic game info
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player = Player.Instance;
        protected PlayerData[] r_PlayersData = new PlayerData[4]; //TODO replace with 4
        protected PlayerData m_CurrentPlayerData;


        //Screen info 
        protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        protected SizeDTO m_BoardOurSize = new SizeDTO();

        //Need to initialize each different game
        protected string m_GameName;
        protected ScoreBoard m_scoreBoard = new ScoreBoard();
        protected Hearts m_Hearts = new Hearts();
        protected PauseMenu m_PauseMenu = new PauseMenu();

        //Things that might change while playing 
        protected int[,] m_Board;
        public eGameStatus m_GameStatus = eGameStatus.Running;
        protected List<string> m_LoseOrder = new List<string>();

        //Don't mind this
        protected Buttons m_Buttons = new Buttons();
        protected Random m_randomPosition = new Random();
        protected List<List<Direction>> m_DirectionsBuffer = new List<List<Direction>>();
        protected Dictionary<int, Direction> m_PlayersDirectionsFromServer = new Dictionary<int, Direction>();


        //List for Ui changes
        protected List<GameObject> m_GameObjectsToAdd = new List<GameObject>();
        protected List<GameObject> m_gameObjectsToUpdate = new List<GameObject>();
        private bool m_Flag = false;
        private bool m_IsMenuVisible = false;

        //Game loop variables
        private bool m_IsGameRunning = true;
        private int m_GapInFrames = 0;
        private Stopwatch m_LoopStopwatch = new Stopwatch();
        protected Stopwatch m_GameStopwatch = new Stopwatch();
        private double k_DesiredFrameTime = 0.016;
        protected eButton m_LastClickedButton = 0;
        private bool m_FlagUpdateRecived = false;
        public bool m_NewButtonPressed = false;




        public Game()
        {
            for (int i = 0; i < 4; i++)
            {
                r_PlayersData[i] = new(i);
            }

            r_ConnectionToServer = new HubConnectionBuilder()
                .WithUrl(Utils.m_InGameHubAddress)
                .Build();

            r_ConnectionToServer.On<int, int, int, int>("GameUpdateReceived", (int i_PlayerID, int i_button, int i_X, int i_Y) =>
            {
                //Point point = new Point(i_X, i_Y);
                //r_PlayersData[i_PlayerID - 1].Button = i_button;
                //r_PlayersData[i_PlayerID - 1].PlayerPointData = point;
                ChangeDirection(Direction.getDirection(i_button),i_PlayerID );

            });

            r_ConnectionToServer.On<int[]>("GetPlayersData", (int[] i_PlayersButtons) =>
            {
                for (int i = 0; i < 4; i++)
                {
                    r_PlayersData[i].Button = i_PlayersButtons[i];
                }
            });

            Task.Run(() =>
           {
               Application.Current.Dispatcher.Dispatch(async () =>
               {
                   await r_ConnectionToServer.StartAsync();
                   await r_ConnectionToServer.SendAsync("ResetHub");
                   OnDeleteGameObject(new GameObject());/////////////////////////////
               });
           });
        }

        public void InitializeGame()
        {
            m_BoardOurSize = m_ScreenMapping.m_TotalScreenOurSize;
            m_Board = new int[m_BoardOurSize.m_Width, m_BoardOurSize.m_Height];
            m_CurrentPlayerData = new PlayerData(m_Player.ButtonThatPlayerPicked);
            m_CurrentPlayerData.Button = -1;

            //r_LiteNetClient.ReceivedData += OnUpdatesReceived;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                m_DirectionsBuffer.Add(new List<Direction>());
            }
            SetGameScreen();

            //m_networkThread = new Thread(() => r_LiteNetClient.Run());
            //m_networkThread.Start();
            ////r_LiteNetClient.Run();
            ////for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
            ////{
            ////    r_LiteNetClient.PlayersData[i].Button = 0;
            ////}
        }

        public void GameLoop()
        {
            m_LoopStopwatch.Start();
            m_GameStopwatch.Start();
            while (m_GameStatus != eGameStatus.Restarted && m_GameStatus != eGameStatus.Ended)
            {
                m_LoopStopwatch.Restart();
                if (m_GameStatus == eGameStatus.Running)
                {
                    updateGame();
                    //while (m_GapInFrames > 0)
                    //{
                    //    updateGame();
                    //    m_GapInFrames--;
                    //    //Thread.Sleep((int)(k_DesiredFrameTime * 1000));
                    //}

                    Draw();
                    m_GapInFrames = (int)((m_LoopStopwatch.Elapsed.Seconds - k_DesiredFrameTime) / k_DesiredFrameTime);
                    if (m_GapInFrames <= 0)
                    {
                        Thread.Sleep((int)((k_DesiredFrameTime - m_LoopStopwatch.Elapsed.Seconds) * 1000));
                    }
                    //Thread.Sleep((int)(k_DesiredFrameTime * 1000));

                }
            }

             throw new Exception("exit the game loop");
        }

        protected virtual void Draw()
        {
            throw new NotImplementedException();
        }

        protected virtual async void updateGame()
        {
            if (m_NewButtonPressed)
            {
                await r_ConnectionToServer.SendAsync(
                    "UpdatePlayerSelection",
                    m_Player.ButtonThatPlayerPicked,
                    m_CurrentPlayerData.Button,
                    m_CurrentPlayerData.PlayerPointData.m_Column, //X
                    m_CurrentPlayerData.PlayerPointData.m_Row); //Y
                m_NewButtonPressed = false;
            }

            int[,] temp = await r_ConnectionToServer.InvokeAsync<int[,]>("GetPlayersData");


            for (int i = 0; i < 4; i++)
            {
                if (r_PlayersData[i].Button != temp[i,0])
                {
                    r_PlayersData[i].Button = temp[i,0];
                    m_CurrentPlayerData.PlayerPointData.m_Column = temp[i,1];
                    m_CurrentPlayerData.PlayerPointData.m_Row) = temp[i,2];
                    ChangeDirection(
                        Direction.getDirection(r_PlayersData[i].Button),
                        i, r_PlayerData.PlayerPointData) ;
                }
            }
        }

        protected virtual Point getPlayerCurrentPointPoint(int i_Player)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnAddScreenObjects()
        {
            AddGameObjectList.Invoke(this, m_GameObjectsToAdd); //..Invoke(this, i_ScreenObject));
        }

        protected virtual void OnHideGameObjects(List<int> i_GameObjectsIDToHide)
        {
            GameObjectsToHide.Invoke(this, i_GameObjectsIDToHide);
        }

        protected virtual void OnShowGameObjects(List<int> i_GameObjectsIDToShow)
        {
            GameObjectsToShow.Invoke(this, i_GameObjectsIDToShow);
        }

        protected void PlayerLostALife(int i_Player)
        {
            m_GameStatus = m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);

            if (m_Hearts.m_HeartToRemove != null)
            {
                OnDeleteGameObject(m_Hearts.m_HeartToRemove);
                m_Hearts.m_HeartToRemove = null;
            }

            if (m_GameStatus != eGameStatus.Running)
            {
                m_scoreBoard.m_LoseOrder.Add(m_GameInformation.m_NamesOfAllPlayers[i_Player - 1]);
                if (m_GameStatus == eGameStatus.Lost)
                {
                    //gameStatusUpdate();//Will show client that he lost 
                }
                else //Game has ended 
                {
                    m_scoreBoard.ShowScoreBoard();
                }
            }
        }

        private bool checkThatPointIsOnBoardGrid(Point i_Point)
        {
            bool isPointOnTheBoard = true;

            if (i_Point.m_Row < 0 || i_Point.m_Column < 0 || i_Point.m_Column > m_BoardOurSize.m_Width
               || i_Point.m_Row > m_BoardOurSize.m_Height)
            {
                isPointOnTheBoard = false;
            }
            else if (m_GameInformation.AmountOfPlayers == 3)
            {
                //check special boards
            }

            return isPointOnTheBoard;
        }

        protected bool isPointOnBoard(Point i_Point)
        {
            bool isPointOnTheBoard = !(i_Point.m_Row < 0 || i_Point.m_Row >= m_BoardOurSize.m_Height || i_Point.m_Column < 0
                                       || i_Point.m_Column >= m_BoardOurSize.m_Width);

            return isPointOnTheBoard;
        }

        //todo: remove
        protected virtual void OBgameLoop()
        {

        }

        protected void OnDeleteGameObject(GameObject i_GameObject)
        {
            GameObject background = new GameObject();
            background.Initialize(eScreenObjectType.Image, 0, "snakebackground.png", new Point(0, 0), 2, m_ScreenMapping.m_ValueToAdd);
            GameObjectToDelete.Invoke(this, background);
        }

        protected virtual void ChangeDirection(Direction i_Direction, int i_Player, Point i_Point)
        {

        }
        public async void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            m_NewButtonPressed = m_CurrentPlayerData.Button != (int)m_Buttons.StringToButton(button!.ClassId);

            m_CurrentPlayerData.Button = (int)m_Buttons.StringToButton(button!.ClassId);
            m_CurrentPlayerData.PlayerPointData = getPlayerCurrentPointPoint(m_CurrentPlayerData.PlayerNumber)
            //await r_ConnectionToServer.SendAsync(
            //    "UpdatePlayerSelection",
            //    m_Player.ButtonThatPlayerPicked,
            //    (int)m_Buttons.StringToButton(button.ClassId),
            //    -1, //X
            //    -1); //Y


            //.Button = (int)m_Buttons.StringToButton(button.ClassId);
            //ChangeDirection(Direction.getDirection(button.ClassId), m_Player.ButtonThatPlayerPicked);
            //SendServerMoveUpdate(m_Buttons.StringToButton(button.ClassId));
            //notifyGameObjectUpdate(eScreenObjectType.Player, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());
        }

        public void SendServerMoveUpdate(eButton i_Button, int i_X = -1, int i_Y = -1)
        {
            //m_FlagUpdateRecived = false;
            //r_LiteNetClient.Send(m_Player.ButtonThatPlayerPicked, (int)i_Button, i_X, i_Y);
            //r_LiteNetClient.Send(m_Player.ButtonThatPlayerPicked, (int)i_Button);
            //await r_ConnectionToServer.SendAsync(
            //    "MoveUpdate",
            //    m_Player.ButtonThatPlayerPicked-1,(int)i_Button);
        }

        public async Task SendServerObjectUpdate(eButton i_Button, int i_X = -1, int i_Y = -1)
        {
            //r_LiteNetClient.Send(m_GameInformation.AmountOfPlayers + 1, (int)i_Button, i_X, i_Y);
            //r_LiteNetClient.Send(m_Player.ButtonThatPlayerPicked, (int)i_Button);
            //await r_ConnectionToServer.SendAsync(
            //    "MoveUpdate",
            //    m_Player.ButtonThatPlayerPicked-1,(int)i_Button);
        }

        protected GameObject addGameBoardObject_(eScreenObjectType i_Type, Point i_Point, int i_ObjectNumber, int i_BoardNumber, string i_Version)
        {
            string png = generatePngString(i_Type, i_ObjectNumber, i_Version);
            GameObject gameObject = new GameObject();

            gameObject.Initialize(i_Type, i_ObjectNumber, png, i_Point, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd);

            m_GameObjectsToAdd.Add(gameObject);
            m_Board[i_Point.m_Column, i_Point.m_Row] = i_BoardNumber;

            return gameObject;
        }

        protected void OnUpdateScreenObject()
        {
            GameObjectsUpdate.Invoke(this, m_gameObjectsToUpdate);
        }

        protected string generatePngString(eScreenObjectType i_Type, int i_ObjectNumber, string i_Version)
        {
            string png;

            png = m_GameName + i_Type.ToString() + i_ObjectNumber + i_Version + ".png";

            return png.ToLower();
        }


        //protected void notifyGameObjectUpdate(eScreenObjectType i_ObjectType, int i_ObjectNumber, Direction i_Direction, Point i_Point)
        //{
        //    if (i_ObjectType == eScreenObjectType.Player)
        //    {
        //        ChangeDirection(i_Direction, i_ObjectNumber);
        //    }
        //    else
        //    {
        //        ChangeGameObject(i_ObjectNumber, i_Direction, i_Point);
        //    }
        //}

        // notifyGameObjectUpdate(eScreenObjectType.Player, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());

        protected virtual void ChangeGameObject(int i_ObjectNumber, Direction i_Direction, Point i_Point)
        {

        }

        public virtual void RunGame()
        {

        }

        protected virtual void getUpdate(int i_Player)
        {
            eGameStatus returnStatus;
            m_GameStatus = m_Buttons.GetGameStatue(r_PlayersData[i_Player - 1].Button, m_GameStatus);


            if (m_GameStatus == eGameStatus.Running)
            {
                if (r_PlayersData[i_Player - 1].Button <= 4)
                {
                    ChangeDirection(
                        Direction.getDirection(r_PlayersData[i_Player - 1].Button),
                        i_Player - 1);
                }
            }


            if (m_IsMenuVisible && m_GameStatus != eGameStatus.Paused)
            {
                m_IsMenuVisible = false;
                OnHideGameObjects(m_PauseMenu.m_PauseMenuIDList);
            }

        }

        protected void OnUpdatesReceived()
        {
            for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
            {
                getUpdate(i);
            }

            if (m_Flag)
            {
                m_Flag = false;

                if (m_GameStatus == eGameStatus.Running)
                {
                    //TODO: update other players direction
                    //gameLoop();
                }
                else if (!m_IsMenuVisible && m_GameStatus == eGameStatus.Paused)
                {
                    m_IsMenuVisible = true;
                    OnShowGameObjects(m_PauseMenu.m_PauseMenuIDList);
                }
            }
            else
            {
                m_Flag = true;
            }

            m_FlagUpdateRecived = true;
        }
    }
}
