
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

        //Events
        public event EventHandler<List<GameObject>> AddGameObjectList;
        public event EventHandler<GameObject> GameObjectUpdate;
        public event EventHandler<GameObject> GameObjectToDelete;
        public Notify GameStart;
        public Notify GameRestart;
        public Notify GameExit;

        //basic game info
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player;
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
        protected int m_AmountOfPlayers;

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
        private eButton m_SpecialButton;
        private int[] temp = new int[12];
        protected Queue<Vector2> a = new Queue<Vector2>();

        public Game()
        {
            m_Player = m_GameInformation.Player;
            for (int i = 0; i < 4; i++)
            {
                m_PlayersDataArray[i] = new(i);
            }
            
            m_PlayerObjects = new GameObject[m_GameInformation.AmountOfPlayers];// new GameObject[2];//
            r_ConnectionToServer = new HubConnectionBuilder()
                .WithUrl(Utils.m_InGameHubAddress)
                .Build();

            r_ConnectionToServer.On<int, int>("SpecialUpdateReceived", (int i_WhatHappened, int i_Player) =>
            {
                //SpecialUpdateReceived(1, i_Player);
                a.Enqueue(new Vector2(i_WhatHappened, i_Player));
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
            m_AmountOfPlayers = m_GameInformation.AmountOfPlayers;
            m_BoardSizeByGrid = m_ScreenMapping.m_TotalScreenGridSize;
            m_Board = new int[m_BoardSizeByGrid.Width, m_BoardSizeByGrid.Height];
            m_CurrentPlayerData = new PlayerData(m_Player.PlayerNumber);
            m_CurrentPlayerData.Button = -1;

            for (int i = 0; i < m_AmountOfPlayers; i++)
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
            while (m_GameStatus != eGameStatus.Restarted && m_GameStatus != eGameStatus.Ended)
            {
                m_GameStopwatch.Restart();
                m_gameObjectsToUpdate = new List<GameObject>();
                m_GameObjectsToAdd = new List<GameObject>();
                updateGame();
                Draw();

                Thread.Sleep((int)((J_DesiredFrameTime - m_LoopStopwatch.Elapsed.Seconds) * 1000));
                m_LastElapsedTime = (int)m_GameStopwatch.Elapsed.TotalMilliseconds;
                //m_LoopNumber++;

            }
            if (m_GameStatus == eGameStatus.Ended)
            {
                m_ConnectedToServer = false;
            }
        }

        private void updateGame()
        {
            SendServerUpdate();
            getUpdatedPosition();
            getButtonUpdate();
            if(m_GameStatus == eGameStatus.Running)
            {
                UpdatePosition(m_LastElapsedTime);
            }
            m_LoopNumber = m_LastElapsedTime;
        }

        private void getButtonUpdate()
        {
            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                if (m_PlayersDataArray[i].Button > 6)
                {
                    if (m_PlayersDataArray[i].Button == 8)
                    {
                        if(m_GameStatus == eGameStatus.Running)
                        {
                            m_SpecialButton = eButton.PauseMenu;
                            m_GameStatus = eGameStatus.Paused;
                            if(m_Player.PlayerNumber == i + 1)
                            {
                                m_PauseMenu.ShowPauseMenu();
                            }
                            m_GameInformation.RealWorldStopwatch.Stop();
                        }
                    }
                    else
                    {
                        if(m_GameStatus == eGameStatus.Paused)
                        {
                            if (m_PlayersDataArray[i].Button == 7)
                            {
                                m_SpecialButton = eButton.Resume;
                                m_GameStatus = eGameStatus.Running;
                                m_PauseMenu.HidePauseMenu();
                            }
                            else if (m_PlayersDataArray[i].Button == 9)
                            {
                                m_SpecialButton = eButton.Restart;
                                m_GameStatus = eGameStatus.Restarted;
                                m_PauseMenu.HidePauseMenu();
                                GameRestart.Invoke();
                            }
                            else if (m_PlayersDataArray[i].Button == 10)
                            {
                                m_SpecialButton = eButton.Exit;
                                m_GameStatus = eGameStatus.Exited;
                                m_PauseMenu.HidePauseMenu();
                                GameExit.Invoke();
                            }
                            m_GameInformation.RealWorldStopwatch.Start();
                        }
                    }
                }
                else
                {
                    m_PlayerObjects[i].RequestDirection(Direction.getDirection(m_PlayersDataArray[i].Button));
                }
            }
        }

        protected void stopMovement(int i_Player)
        {
            if (i_Player == m_Player.PlayerNumber)
            {
                m_NewButtonPressed = true;
                m_CurrentPlayerData.Button = 0;
            }
        }
        void getUpdatedPosition()
        {
            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                Point pointRecived = new Point(temp[i + 4], temp[i + 8]);
                if (pointRecived.Row != 0 && pointRecived.Column != 0 && m_PlayersDataArray[i].PlayerPointData != pointRecived)
                {
                    if (m_Player.PlayerNumber == 1)
                    {
                        System.Diagnostics.Debug.WriteLine("1r " + pointRecived.Column + " " + pointRecived.Row);
                        System.Diagnostics.Debug.WriteLine("__1r " + m_PlayersDataArray[i].PlayerPointData.Column + " " + m_PlayersDataArray[i].PlayerPointData.Row);
                    }

                    m_PlayerObjects[i].UpdatePointOnScreen(pointRecived);
                    m_PlayersDataArray[i].PlayerPointData = pointRecived;
                }
                m_PlayersDataArray[i].Button = temp[i];
            }
        }

        protected virtual void Draw()
        {
            if (m_GameObjectsToAdd.Count != 0)
            {
                OnAddScreenObjects();
            }

            foreach (var player in m_PlayerObjects)
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

                r_ConnectionToServer.SendAsync(
                  "UpdatePlayerSelection",
                  m_Player.PlayerNumber - 1,
                  m_CurrentPlayerData.Button,
                  0, 0);
             
                m_NewButtonPressed = false;
            }
        }

        private async void GetServerUpdate()
        {
            while (m_ConnectedToServer)
            {
                temp = await r_ConnectionToServer.InvokeAsync<int[]>("GetPlayersData");
                for (int i = 0; i < 4; i++)
                {
                    m_PlayersDataArray[i].Button = temp[i];
                }
                // m_PlayersDataArray[0].Button = temp[0];
                //m_PlayersDataArray[1].Button = temp[1];
                //System.Diagnostics.Debug.WriteLine("r " + temp[0]);
            }
        }

        protected async void SendSpecialServerUpdate(object sender, int i_eventNumber)
        {
            //System.Diagnostics.Debug.WriteLine("s" + Player.PlayerNumber);
            GameObject gameObject = sender as GameObject;
            System.Diagnostics.Debug.WriteLine("HIT " + m_Player.PlayerNumber);//("s "+ playerPosition.Column + " "+ playerPosition.Row);
            r_ConnectionToServer.SendAsync(
                "SpecialUpdate",
                i_eventNumber, gameObject.ObjectNumber
                );
        }
        protected virtual void SpecialUpdateReceived(int i_WhatHappened, int i_Player)
        {

        }

        protected virtual void PlayerLostALife(object sender, int i_Player)
        {
            m_GameStatus = m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);
            System.Diagnostics.Debug.WriteLine("GOT " + m_Player.PlayerNumber);
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
                    // m_scoreBoard.ShowScoreBoard();
                }
            }
        }

        protected virtual void UpdatePosition(double i_TimeElapsed)
        {
            foreach (var gameObject in m_PlayerObjects)
            {
                gameObject.Update(i_TimeElapsed);
            }

            foreach (var gameObject in m_PlayerObjects)
            {
                if (gameObject.IsCollisionDetectionEnabled)
                {
                    m_CollisionManager.FindCollisions(gameObject);
                }
            }

            if (a.Count != 0)
            {
                Thread t = Thread.CurrentThread;
                System.Diagnostics.Debug.WriteLine($"(EVENT) - Player num- {m_GameInformation.Player.PlayerNumber}Thread id- {t.ManagedThreadId}processor Id-{Thread.GetCurrentProcessorId()}");
                Vector2 e = a.Dequeue();
                SpecialUpdateReceived((int)e.X, (int)e.Y);
            }
        }
        public void UpdateClientsAboutPosition(object sender, Point i_Point)
        {
            GameObject a = sender as GameObject;
            SendServerPositionUpdate(a.ObjectNumber, i_Point);
        }

        private async void SendServerPositionUpdate(int i_Player, Point i_Point)
        {
            Thread t = Thread.CurrentThread;

            System.Diagnostics.Debug.WriteLine("hIT" + t.ManagedThreadId + " " + Thread.GetCurrentProcessorId() + " " + i_Point.Row);
            //System.Diagnostics.Debug.WriteLine("s "+Player.PlayerNumber+" "+ i_Point.Column + " "+ i_Point.Row);
            await r_ConnectionToServer.SendAsync(
                "UpdatePlayerSelection", i_Player - 1
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
            GameObject i = sender as GameObject;
            GameObjectUpdate.Invoke(this, i);
        }

        public void RunGame()
        {
            Thread serverUpdateThread = new(GetServerUpdate);//(serverUpdateLoop);
            Thread newThread = new(GameLoop);
            serverUpdateThread.Start();
            newThread.Start();
        }

        protected virtual void OnAddScreenObjects()
        {
            foreach (var newObject in m_GameObjectsToAdd)
            {
                if (newObject.IsCollisionDetectionEnabled)
                {
                    m_CollisionManager.AddObjectToMonitor(newObject);
                    if (newObject.ScreenObjectType == eScreenObjectType.Player)
                    {
                        m_PlayerObjects[newObject.ObjectNumber - 1] = newObject;
                        newObject.SpecialEvent += SendSpecialServerUpdate;
                        newObject.UpdatePosition += UpdateClientsAboutPosition;
                    }
                    else
                    {
                        newObject.SpecialEvent += SendSpecialServerUpdate;
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