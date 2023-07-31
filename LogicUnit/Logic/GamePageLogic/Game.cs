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
        public Notify GameStart;

        //basic game info
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player = Player.Instance;
        protected PlayerData[] m_PlayersDataArray = new PlayerData[4]; //TODO replace with 4
        protected PlayerData m_CurrentPlayerData;
        private LinkedList<PlayerData> moveBuffer = new LinkedList<PlayerData>();

        //Screen info 
        protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        protected SizeDTO m_BoardSizeByGrid = new SizeDTO();

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
        private Stopwatch m_ServerStopwatch = new Stopwatch();
        private int m_LastElapsedTime;

        protected eButton m_LastClickedButton = 0;
        private bool m_FlagUpdateRecived = false;
        public bool m_NewButtonPressed = false;
        private int i_AvgPing = 0;
        public double m_LoopNumber = 0;
        public GameObject[] m_MoveableGameObjects;
        private const double J_DesiredFrameTime = 0.067;
        protected readonly CollisionManager m_CollisionManager = new CollisionManager();
        private bool m_ConnectedToServer = true;    //TODO

        /*TODO:
         * create different thread for server updates
         * the server will update an array named ServerInput that is a global variable
         * add method Update(int deltaTime) to all GameObject. meaning we will be time driven
         * this method will read the ServerInput and update the game accordingly.
         *
         *
         *
         */

        public Game()
        {
            for (int i = 0; i < 4; i++)
            {
                m_PlayersDataArray[i] = new(i);
            }

            m_MoveableGameObjects = new GameObject[1];//[m_GameInformation.AmountOfPlayers];
            r_ConnectionToServer = new HubConnectionBuilder()
                .WithUrl(Utils.m_InGameHubAddress)
                .Build();

            r_ConnectionToServer.On<int, int, int, int>("GameUpdateReceived", (int i_PlayerID, int i_button, int i_X, int i_Y) =>
            {
                //moveBuffer(new PlayerData(i_PlayerID)).
                //Point point = new Point(i_X, i_Y);
                //m_PlayersDataArray[i_PlayerID - 1].Button = i_button;
                //m_PlayersDataArray[i_PlayerID - 1].PlayerPointData = point;
                //ChangeDirection(Direction.getDirection(i_button),i_PlayerID, new Point(i_X, i_Y));
                ChangeDirection(Direction.getDirection(i_button), i_PlayerID, i_X);
            });

            r_ConnectionToServer.On<int[]>("GetPlayersData", (int[] i_PlayersButtons) =>
            {
                for (int i = 0; i < 4; i++)
                {
                    m_PlayersDataArray[i].Button = i_PlayersButtons[i];
                }
            });

            Task.Run(() =>
           {
               Application.Current.Dispatcher.Dispatch(async () =>
               {
                   await r_ConnectionToServer.StartAsync();
                   await r_ConnectionToServer.SendAsync("ResetHub");
                   m_ConnectedToServer = true;
                   OnGameStart();
               });
           });
        }

        public void InitializeGame()
        {
            m_BoardSizeByGrid= m_ScreenMapping.m_TotalScreenGridSize;
            m_Board = new int[m_BoardSizeByGrid.Width, m_BoardSizeByGrid.Height];
            m_CurrentPlayerData = new PlayerData(m_Player.ButtonThatPlayerPicked);
            m_CurrentPlayerData.Button = -1;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                m_DirectionsBuffer.Add(new List<Direction>());
            }
            SetGameScreen();
        }

        public void GameLoop()
        {
            m_GameStopwatch.Start();
            while (m_GameStatus == eGameStatus.Running) //(m_GameStatus != eGameStatus.Restarted && m_GameStatus != eGameStatus.Ended)
            {
                m_GameStopwatch.Restart();
                m_gameObjectsToUpdate = new List<GameObject>();
                m_GameObjectsToAdd = new List<GameObject>();
                updateGame();

                Draw();

                Thread.Sleep((int)((J_DesiredFrameTime - m_LoopStopwatch.Elapsed.Seconds) * 1000));
                m_LastElapsedTime = (int) m_GameStopwatch.Elapsed.TotalMilliseconds;

            }
            if (m_GameStatus == eGameStatus.Ended)
            {
                m_ConnectedToServer = false;
            }
        }

        private async void serverUpdateLoop()
        {
            while (m_ConnectedToServer)
            {
                int[] temp = await r_ConnectionToServer.InvokeAsync<int[]>("GetPlayersData");
               
                for (int i = 0; i < 4; i++)
                {
                    //if(m_PlayersDataArray[i].Button != temp[i])
                    //{
                    //    m_PlayersDataArray[i].IsNewButton = true;
                        m_PlayersDataArray[i].Button = temp[i];
                    //}
                    //else
                    //{
                    //    m_PlayersDataArray[i].IsNewButton = false;
                    //}
                    m_PlayersDataArray[i].PlayerPointData = new Point(temp[i + 4], temp[i + 8]);
                }
                
            }
        }

        private void updateGame()
        {
            SendServerUpdate();
            GetServerUpdate();
            m_LoopNumber = m_LastElapsedTime;
            updatePosition(m_LastElapsedTime);

        }

        //private async void calculateAvgPing()
        //{
        //    for(int i = 0; i < 4; i++)
        //    {
        //        DateTime dateTimeNow = DateTime.Now;
        //        await r_ConnectionToServer.InvokeAsync<DateTime>(
        //                             "Ping");
        //        i_AvgPing = (DateTime.Now.Millisecond - dateTimeNow.Millisecond) / (i+1);
        //        Thread.Sleep(250);
        //    }
        //}

        protected virtual void Draw()
        {
            if (m_GameObjectsToAdd.Count != 0)
            {
                OnAddScreenObjects();
            }

            OnUpdateScreenObject();
        }

        private async void SendServerUpdate()
        {
            if (m_NewButtonPressed)
            {
                //m_CurrentPlayerData.PlayerPointData = getPlayerCurrentPointPoint(m_CurrentPlayerData.PlayerNumber);

                await r_ConnectionToServer.SendAsync(
                   "UpdatePlayerSelection",
                   m_Player.ButtonThatPlayerPicked,
                   m_CurrentPlayerData.Button,
                   m_LoopNumber,
                   -1);
                //m_CurrentPlayerData.PlayerPointData.Column, //X
                //m_CurrentPlayerData.PlayerPointData.Row); //Y

                m_NewButtonPressed = false;
            }
        }

        private async void GetServerUpdate()
        {
            //get data from the server
            for (int i = 1; i <= 1; i++)
            {
                //if(m_PlayersDataArray[i - 1].IsNewButton)
                {
                    ChangeDirection(Direction.getDirection(m_PlayersDataArray[i - 1].Button), i, 1);
                }
            }
        }

        protected virtual Point getPlayerCurrentPointPoint(int i_Player)
        {
            throw new NotImplementedException();
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

            if (i_Point.Row < 0 || i_Point.Column < 0 || i_Point.Column > m_BoardSizeByGrid.Width
               || i_Point.Row > m_BoardSizeByGrid.Height)
            {
                isPointOnTheBoard = false;
            }
            else if (m_GameInformation.AmountOfPlayers == 3)
            {
                //check special boards
            }

            return isPointOnTheBoard;
        }

        protected virtual void ChangeDirection(Direction i_Direction, int i_GameObject, int i_LoopNumber)
        {
            m_MoveableGameObjects[i_GameObject - 1].RequestDirection(i_Direction);
        }

        protected virtual void updatePosition(double i_TimeElapsed)
        {
            foreach (var gameObject in m_MoveableGameObjects)
            {
                gameObject.Update(i_TimeElapsed);
            }

            foreach (var gameObject in m_MoveableGameObjects)
            {
                m_CollisionManager.FindCollisions(gameObject);
            }
        }

        protected virtual void ChangeDirection(Direction i_Direction, int i_Player, Point i_Point)
        {

        }

        public async void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            m_NewButtonPressed = m_CurrentPlayerData.Button != (int)m_Buttons.StringToButton(button!.ClassId);

            m_CurrentPlayerData.Button = (int)m_Buttons.StringToButton(button!.ClassId);
            //Point point = m_CurrentPlayerData.PlayerPointData = getPlayerCurrentPointPoint(m_CurrentPlayerData.PlayerNumber);

            //await r_ConnectionToServer.SendAsync(
            //    "UpdatePlayerSelection",
            //    m_Player.ButtonThatPlayerPicked,
            //    (int)m_Buttons.StringToButton(button.ClassId),
            //    point.Column, //X
            //    point.Row); //Y

             //loopnum = m_LoopNumber;

            //await r_ConnectionToServer.SendAsync(
            //    "UpdatePlayerSelection",
            //    m_Player.ButtonThatPlayerPicked,
            //    (int)m_Buttons.StringToButton(button.ClassId),
            //    loopnum, //X
            //    0); //Y

            //.Button = (int)m_Buttons.StringToButton(button.ClassId);
            //ChangeDirection(Direction.getDirection(button.ClassId), m_Player.ButtonThatPlayerPicked);
            //SendServerMoveUpdate(m_Buttons.StringToButton(button.ClassId));
            //notifyGameObjectUpdate(eScreenObjectType.Player, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());
        }

        protected GameObject addGameBoardObject(eScreenObjectType i_Type, Point i_Point, int i_ObjectNumber, int i_BoardNumber, string i_Version)
        {
            string png = generatePngString(i_Type, i_ObjectNumber, i_Version);
            GameObject gameObject = new GameObject();

            gameObject.Initialize(i_Type, i_ObjectNumber, png, i_Point, true, m_ScreenMapping.m_ValueToAdd);

            m_GameObjectsToAdd.Add(gameObject);
            m_Board[(int)i_Point.Column, (int)i_Point.Row] = i_BoardNumber;

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

        public void RunGame()
        {
            Thread serverUpdateThread = new(serverUpdateLoop);
            Thread newThread = new(GameLoop);
            serverUpdateThread.Start();
            newThread.Start();

        }

        protected virtual void OnAddScreenObjects()
        {
            foreach(var newObject in m_GameObjectsToAdd)
            {
                if(newObject.IsCollisionDetectionEnabled)
                {
                    m_CollisionManager.AddObjectToMonitor(newObject);
                    if(newObject.ScreenObjectType == eScreenObjectType.Player)
                    {
                        m_MoveableGameObjects[newObject.ObjectNumber-1] = newObject;
                    }
                }
            }
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

        protected void OnDeleteGameObject(GameObject i_GameObject)
        {
            GameObjectToDelete.Invoke(this, i_GameObject);
        }

        protected virtual void OnGameStart()
        {
            GameStart.Invoke();
        }

        //protected virtual void getUpdate(int i_Player)
        //{
        //    eGameStatus returnStatus;
        //    m_GameStatus = m_Buttons.GetGameStatue(m_PlayersDataArray[i_Player - 1].Button, m_GameStatus);


        //    //if (m_GameStatus == eGameStatus.Running)
        //    //{
        //    //    if (m_PlayersDataArray[i_Player - 1].Button <= 4)
        //    //    {
        //    //        ChangeDirection(
        //    //            Direction.getDirection(m_PlayersDataArray[i_Player - 1].Button),
        //    //            i_Player - 1, m_PlayersDataArray[i].PlayerPointData);
        //    //    }
        //    //}


        //    if (m_IsMenuVisible && m_GameStatus != eGameStatus.Paused)
        //    {
        //        m_IsMenuVisible = false;
        //        OnHideGameObjects(m_PauseMenu.m_PauseMenuIDList);
        //    }

        //}

        //protected void OnUpdatesReceived()
        //{
        //    for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
        //    {
        //        getUpdate(i);
        //    }

        //    if (m_Flag)
        //    {
        //        m_Flag = false;

        //        if (m_GameStatus == eGameStatus.Running)
        //        {
        //            //TODO: update other players direction
        //            //gameLoop();
        //        }
        //        else if (!m_IsMenuVisible && m_GameStatus == eGameStatus.Paused)
        //        {
        //            m_IsMenuVisible = true;
        //            OnShowGameObjects(m_PauseMenu.m_PauseMenuIDList);
        //        }
        //    }
        //    else
        //    {
        //        m_Flag = true;
        //    }

        //    m_FlagUpdateRecived = true;
        //}
    }
}
