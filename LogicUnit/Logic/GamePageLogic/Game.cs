
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
        public event EventHandler<GameObject> GameObjectUpdate;
        public event EventHandler<GameObject> GameObjectToDelete;
        public Notify GameStart;

        //basic game info
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player = Player.Instance;
        protected PlayerData[] m_PlayersDataArray = new PlayerData[4]; //TODO replace with 4
        protected PlayerData m_CurrentPlayerData;

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
        private int m_LastElapsedTime;

        protected eButton m_LastClickedButton = 0;
        private bool m_FlagUpdateRecived = false;
        public bool m_NewButtonPressed = false;
        private int i_AvgPing = 0;
        public int m_LoopNumber = 0;
        public GameObject[] m_PlayerObjects;
        private const double J_DesiredFrameTime = 0.067;
        protected readonly CollisionManager m_CollisionManager = new CollisionManager();
        private bool m_ConnectedToServer = true;    //TODO
        static readonly object m_lock = new object();
        //private int sent = 0;
        //private int recived = 0;
        private int d = 0;

        public Game()
        {
            for (int i = 0; i < 4; i++)
            {
                m_PlayersDataArray[i] = new(i);
            }

            m_PlayerObjects = new GameObject[m_GameInformation.AmountOfPlayers];// new GameObject[2];//
            r_ConnectionToServer = new HubConnectionBuilder()
                .WithUrl(Utils.m_InGameHubAddress)
                .Build();


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
            m_CurrentPlayerData = new PlayerData(m_Player.PlayerNumber);
            m_CurrentPlayerData.Button = -1;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                m_DirectionsBuffer.Add(new List<Direction>());
            }
            SetGameScreen();
        }

        public void GameLoop()
        {
            m_GameInformation.RealWorldStopwatch = new Stopwatch();
            m_GameInformation.RealWorldStopwatch.Start();
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
                m_LoopNumber++;

            }
            if (m_GameStatus == eGameStatus.Ended)
            {
                m_ConnectedToServer = false;
            }
        }
        private void updateGame()
        {
            SendServerUpdate();
            changeDirectons();
            GetServerUpdate();
            //m_LoopNumber = m_LastElapsedTime;
            
            updatePosition(m_LastElapsedTime);
        }

        
        

        protected virtual void Draw()
        {
            if (m_GameObjectsToAdd.Count != 0)
            {
                OnAddScreenObjects();
            }

            foreach(var player in m_PlayerObjects)
            {
                player.Draw();
            }
        }

        private async void SendServerUpdate()
        {
            //System.Diagnostics.Debug.WriteLine("s" + m_CurrentPlayerData.Button);
            if (m_NewButtonPressed)
            {
                Point playerPosition = m_PlayerObjects[m_Player.PlayerNumber - 1]
                    .GetCurrentPointOnScreen();

                //System.Diagnostics.Debug.WriteLine("s"+m_CurrentPlayerData.Button);//("s "+ playerPosition.Column + " "+ playerPosition.Row);
               // sent = m_LoopNumber;
                await r_ConnectionToServer.SendAsync(
                   "UpdatePlayerSelection",
                   m_Player.PlayerNumber-1,
                   m_CurrentPlayerData.Button,
                   0,0);
                
                m_NewButtonPressed = false;
            }
        }

        private async void GetServerUpdate()
        {
            if (m_ConnectedToServer)
            {
                int[] temp = await r_ConnectionToServer.InvokeAsync<int[]>("GetPlayersData");

                //System.Diagnostics.Debug.WriteLine("r " + temp[0]);
                for (int i = 0; i < 4; i++)
                {
                    Point pointRecived = new Point(temp[i + 4], temp[i + 8]);
                    if (pointRecived.Row != 0 && pointRecived.Column != 0 && m_PlayersDataArray[i].PlayerPointData != pointRecived)
                    {
                        //if (m_Player.PlayerNumber == 1)
                        //{
                        //    System.Diagnostics.Debug.WriteLine("1r " + pointRecived.Column + " " + pointRecived.Row);
                        //    System.Diagnostics.Debug.WriteLine("__1r " + m_PlayersDataArray[i].PlayerPointData.Column + " " + m_PlayersDataArray[i].PlayerPointData.Row);
                        //}
                        //else
                        //{
                        //    System.Diagnostics.Debug.WriteLine("2r " + pointRecived.Column + " " + pointRecived.Row);
                        //    System.Diagnostics.Debug.WriteLine("__2r " + m_PlayersDataArray[i].PlayerPointData.Column + " " + m_PlayersDataArray[i].PlayerPointData.Row);
                        //}

                        m_PlayerObjects[i].UpdatePointOnScreen(pointRecived);
                        m_PlayersDataArray[i].PlayerPointData = pointRecived;
                    }

                    m_PlayersDataArray[i].Button = temp[i];
                    //d++;
                    //System.Diagnostics.Debug.WriteLine("server finis " + d);
                }
            }
        }

        private void changeDirectons()
        {
            for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
            {
                ChangeDirection(Direction.getDirection(m_PlayersDataArray[i - 1].Button), i, m_LoopNumber);
            }
            //System.Diagnostics.Debug.WriteLine("change finish " + d);
        }

        protected virtual void PlayerLostALife(object sender, int i_Player)
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

        private void ChangeDirection(Direction i_Direction, int i_GameObject, int i_LoopNumber)
        {
            //if(m_Player.PlayerNumber == 1 && i_Direction == Direction.Right)
            //{
            //    int g = 0;
            //}
            //else if(i_Direction == Direction.Right)
            //{
            //    int g = 0;
            //}
            m_PlayerObjects[i_GameObject - 1].RequestDirection(i_Direction);
        }

        protected virtual void updatePosition(double i_TimeElapsed)
        {
            foreach (var gameObject in m_PlayerObjects)
            {
                gameObject.Update(i_TimeElapsed);
            }

            foreach (var gameObject in m_PlayerObjects)
            {
                if(gameObject.IsCollisionDetectionEnabled)
                {
                    m_CollisionManager.FindCollisions(gameObject);
                }
            }
        }
        public void UpdateClientsAboutPosition(object sender, Point i_Point)
        {
            GameObject a = sender as GameObject;
            SendServerPositionUpdate(a.ObjectNumber, i_Point);
        }

        private async void SendServerPositionUpdate(int i_Player,Point i_Point)
        {
            System.Diagnostics.Debug.WriteLine("s "+m_Player.PlayerNumber+" "+ i_Point.Column + " "+ i_Point.Row);
            await r_ConnectionToServer.SendAsync(
                "UpdatePlayerSelection", i_Player-1
                ,
                -1,
                (int)i_Point.Column, (int)i_Point.Row);
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            m_NewButtonPressed = m_CurrentPlayerData.Button != (int)m_Buttons.StringToButton(button!.ClassId);
            
            m_CurrentPlayerData.Button = (int)m_Buttons.StringToButton(button!.ClassId);
        }

        private void OnUpdateScreenObject(object sender, EventArgs e)
        {
            GameObject i= sender as GameObject;
            GameObjectUpdate.Invoke(this,i);
        }

        public void RunGame()
        {
            //Thread serverUpdateThread = new(serverUpdateLoop);
            Thread newThread = new(GameLoop);
            //serverUpdateThread.Start();
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
                        m_PlayerObjects[newObject.ObjectNumber-1] = newObject;
                        newObject.PlayerGotHit += PlayerLostALife;
                        newObject.UpdatePosition += UpdateClientsAboutPosition;
                    }
                }

                newObject.UpdateGameObject += OnUpdateScreenObject;
            }
            AddGameObjectList.Invoke(this, m_GameObjectsToAdd); //..Invoke(this, i_ScreenObject));
        }

        protected void OnDeleteGameObject(GameObject i_GameObject)
        {
            GameObjectToDelete.Invoke(this, i_GameObject);
        }

        protected virtual void OnGameStart()
        {
            GameStart.Invoke();
        }
    }
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