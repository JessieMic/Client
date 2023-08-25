﻿
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
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using LogicUnit.Logic.GamePageLogic.LiteNet;
using Microsoft.AspNetCore.SignalR.Client;
using Objects;
using Objects.Enums;
using Objects.Enums.BoardEnum;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Point = Objects.Point;

namespace LogicUnit
{
    public abstract partial class Game
    {
        private List<string> m_PlayerMovementsLogs = new List<string>();
        protected readonly HubConnection r_ConnectionToServer;

        //Events
        public event EventHandler<List<GameObject>> AddGameObjectList;
        public event EventHandler<GameObject> GameObjectUpdate;
        public event EventHandler<GameObject> GameObjectToDelete;
        public Notify GameStart;
        public Notify GameRestart;
        public Notify GameExit;
        public Notify ShowWinner;

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
        protected int m_AmountOfPlayers;

        //Game loop variables
        private Stopwatch m_LoopStopwatch = new Stopwatch();
        protected Stopwatch m_GameStopwatch = new Stopwatch();
        private int m_LastElapsedTime;

        protected eButton m_LastClickedButton = 0;
        public bool m_NewButtonPressed = false;
        public int m_LoopNumber = 0;
        public GameObject[] m_PlayerObjects;
        private const double k_DesiredFrameTime = 0.067;
        protected readonly CollisionManager r_CollisionManager = new CollisionManager();
        private bool m_ConnectedToServer = true;    //TODO
        static readonly object m_lock = new object();
        protected Queue<SpecialUpdate> m_SpecialEventQueue = new Queue<SpecialUpdate>();
        protected Queue<SpecialUpdate> m_SpecialEventWithPointQueue = new Queue<SpecialUpdate>();
        private int[] m_ServerUpdates = new int[12];
        protected eMoveType m_MoveType;

        public string msg = string.Empty;
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
                lock (m_lock)
                {
                    m_SpecialEventQueue.Enqueue(new SpecialUpdate(i_WhatHappened, i_Player));
                }
            });

            r_ConnectionToServer.On<int, int,int>("SpecialUpdateWithPointReceived", (i_X, i_Y, i_Player) =>
            { 
                lock(m_lock)
                {
                    m_SpecialEventWithPointQueue.Enqueue(new SpecialUpdate(i_X, i_Y, i_Player));
                }
                //SpecialUpdateWithPointReceived(new SpecialUpdate(i_X, i_Y, i_Player));
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

        private void checkForSpecialUpdates()
        {
            lock(m_lock)
            {
                if (m_SpecialEventQueue.Count != 0)
                {
                    SpecialUpdate specialUpdate = m_SpecialEventQueue.Dequeue();
                    SpecialUpdateReceived(specialUpdate);
                }
                if (m_SpecialEventWithPointQueue.Count != 0)
                {
                    SpecialUpdate specialUpdate = m_SpecialEventWithPointQueue.Dequeue();
                    SpecialUpdateWithPointReceived(specialUpdate);
                }
            }
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
            while (m_GameStatus != eGameStatus.Restarted && m_GameStatus != eGameStatus.Exited)
            {
                m_GameStopwatch.Restart();
                m_gameObjectsToUpdate.Clear();
                m_GameObjectsToAdd.Clear();
                updateGame();
                Draw();

                Thread.Sleep((int)((k_DesiredFrameTime - m_LoopStopwatch.Elapsed.Seconds) * 1000));
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
            else
            {
                checkForSpecialUpdates();
            }
            m_LoopNumber = m_LastElapsedTime;
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
                Point pointRecived = new Point(m_ServerUpdates[i + 4], m_ServerUpdates[i + 8]);
                if (pointRecived.Row != 0 && pointRecived.Column != 0 && m_PlayersDataArray[i].PlayerPointData != pointRecived)
                {
                    m_PlayerObjects[i].UpdatePointOnScreen(pointRecived);
                    m_PlayersDataArray[i].PlayerPointData = pointRecived;
                }
                m_PlayersDataArray[i].Button = m_ServerUpdates[i];
            }
        }

        protected void addGameObjectImmediately(GameObject newObject)
        {
            List<GameObject> k = new List<GameObject>();
            k.Add(newObject);

            if (newObject.IsCollisionDetectionEnabled)
            {
                r_CollisionManager.AddObjectToMonitor(newObject);
                newObject.SpecialEvent += specialEventInvoked;
            }

            newObject.UpdateGameObject += OnUpdateScreenObject;
           
            AddGameObjectList.Invoke(this, k);
        }

        protected void addBoarderFor3Players()
        {
            if (m_AmountOfPlayers == 3)
            {
                int y = m_ScreenMapping.m_Boundaries.Height;
                int x = m_ScreenMapping.m_Boundaries.Width;

                for (int i = y; i < m_BoardSizeByGrid.Height; i++)
                {
                    addBoarder(new Point(x, i));
                }
                for (int i = x; i < m_BoardSizeByGrid.Width; i++)
                {
                    addBoarder(new Point(x, i));
                }
            }
        }

        protected virtual void addBoarder(Point i_Point)
        {
            m_GameObjectsToAdd.Add(new Boarder(new Point(i_Point.Column, i_Point.Row), string.Empty));
        }
        
        protected virtual void Draw()
        {
            if (m_GameObjectsToAdd.Count != 0 && m_GameStatus != eGameStatus.Ended)
            {
                OnAddScreenObjects();
            }

            foreach (var player in m_PlayerObjects)
            {
                player.OnDraw();
            }
        }

        private async void SendServerUpdate()
        {
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
                m_ServerUpdates = await r_ConnectionToServer.InvokeAsync<int[]>("GetPlayersData");
                for (int i = 0; i < 4; i++)
                {
                    m_PlayersDataArray[i].Button = m_ServerUpdates[i];
                }
                // m_PlayersDataArray[0].Button = m_ServerUpdates[0];
                //m_PlayersDataArray[1].Button = m_ServerUpdates[1];
                //System.Diagnostics.Debug.WriteLine("r " + m_ServerUpdates[0]);
            }
        }

        protected virtual void specialEventInvoked(object i_Sender, int i_eventNumber)
        {
            SendSpecialServerUpdate(i_Sender,i_eventNumber);
        }

        protected async void SendSpecialServerUpdate(object? sender, int i_eventNumber)
        {
            int number = m_GameInformation.Player.PlayerNumber;
            if(sender != null)
            {
                GameObject gameObject = sender as GameObject;
                number = gameObject.ObjectNumber;
            }
            
            r_ConnectionToServer.SendAsync(
                "SpecialUpdate",
                i_eventNumber, number
                );
        }

        
        protected virtual void SpecialUpdateWithPointReceived(SpecialUpdate i_SpecialUpdate)
        {

        }

        protected virtual void checkForGameStatusUpdate()
        {

        }

        protected virtual void PlayerLostALife(object sender, int i_Player)
        {
            if(m_Hearts.setPlayerLifeAndCheckIfDead(i_Player))
            {
               // checkForGameStatusUpdate(i_Player);
            }
        }

        protected virtual void UpdatePosition(double i_TimeElapsed)
        {
            foreach (var gameObject in m_PlayerObjects)
            {
                gameObject.Update(i_TimeElapsed);
            }

            checkForSpecialUpdates();
            r_CollisionManager.CheckAllCollidablesForCollision();
            checkForGameStatusUpdate();
        }

      

        public void UpdateClientsAboutPosition(object sender, Point i_Point)
        {
            GameObject a = sender as GameObject;
            SendServerPositionUpdate(a.ObjectNumber, i_Point);
        }

        private async void SendServerPositionUpdate(int i_Player, Point i_Point)
        {
            await r_ConnectionToServer.SendAsync(
                "UpdatePlayerSelection", i_Player - 1
                ,
                -1,
                (int)i_Point.Column, (int)i_Point.Row);
        }

        virtual public void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            m_NewButtonPressed = m_CurrentPlayerData.Button != (int)m_Buttons.StringToButton(button!.ClassId);
            if((int)m_Buttons.StringToButton(button!.ClassId) > 6)
            {
               SendSpecialServerUpdate(null, (int)m_Buttons.StringToButton(button!.ClassId));
            }
            else
            {
                m_CurrentPlayerData.Button = (int)m_Buttons.StringToButton(button!.ClassId);
            }
        }

        public void OnButtonRelesed(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (m_CurrentPlayerData.Button == (int)m_Buttons.StringToButton(button!.ClassId))
            {
                m_CurrentPlayerData.Button = (int)eButton.Stop;
                m_NewButtonPressed = true;
            }
        }

        protected virtual void SpecialUpdateReceived(SpecialUpdate i_SpecialUpdate)
        {
            if (i_SpecialUpdate.Update == 8)
            {
                if (m_GameStatus == eGameStatus.Running)
                {
                    m_GameStatus = eGameStatus.Paused;
                    if (m_Player.PlayerNumber == i_SpecialUpdate.Player_ID)
                    {
                        m_PauseMenu.ShowPauseMenu();
                    }
                    m_GameInformation.RealWorldStopwatch.Stop();
                }
            }
            else
            {
                if (m_GameStatus == eGameStatus.Paused || m_GameStatus == eGameStatus.Ended)
                {

                    m_PauseMenu.HidePauseMenu();
                    if (i_SpecialUpdate.Update == 7)
                    {
                        m_GameStatus = eGameStatus.Running;
                    }
                    else if (i_SpecialUpdate.Update == 9)
                    {
                        m_GameStatus = eGameStatus.Restarted;
                        GameRestart.Invoke();
                    }
                    else if (i_SpecialUpdate.Update == 10)
                    {
                        m_GameStatus = eGameStatus.Exited;
                        GameExit.Invoke();
                    }
                    m_GameInformation.RealWorldStopwatch.Start();
                }
            }
        }


        private void getButtonUpdate()
        {
            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
            //    if (m_PlayersDataArray[i].Button > 6)
            //    {
            //        if (m_PlayersDataArray[i].Button == 8)
            //        {
            //            if (m_GameStatus == eGameStatus.Running)
            //            {
            //                m_GameStatus = eGameStatus.Paused;
            //                if (m_Player.PlayerNumber == i + 1)
            //                {
            //                    m_PauseMenu.ShowPauseMenu();
            //                }
            //                m_GameInformation.RealWorldStopwatch.Stop();
            //            }
            //        }
            //        else
            //        {
            //            if (m_GameStatus == eGameStatus.Paused || m_GameStatus == eGameStatus.Ended)
            //            {

            //                m_PauseMenu.HidePauseMenu();
            //                if (m_PlayersDataArray[i].Button == 7)
            //                {
            //                    m_GameStatus = eGameStatus.Running;
            //                }
            //                else if (m_PlayersDataArray[i].Button == 9)
            //                {
            //                    m_GameStatus = eGameStatus.Restarted;
            //                    GameRestart.Invoke();
            //                }
            //                else if (m_PlayersDataArray[i].Button == 10)
            //                {
            //                    m_GameStatus = eGameStatus.Exited;
            //                    GameExit.Invoke();
            //                }
            //                m_GameInformation.RealWorldStopwatch.Start();
            //            }
            //        }
            //    }
            //    else
            //    {
                    m_PlayerObjects[i].RequestDirection(Direction.getDirection(m_PlayersDataArray[i].Button));
                //}
            }
        }

        private void OnUpdateScreenObject(object sender, EventArgs e)
        {
            GameObject i = sender as GameObject;
            GameObjectUpdate.Invoke(this, i);
        }

        protected async void SendServerSpecialPointUpdate(Point i_Point,int i_Player)
        {
            System.Diagnostics.Debug.WriteLine("BOMB " + m_Player.PlayerNumber);
            r_ConnectionToServer.SendAsync(
                "SpecialUpdateWithPoint",
                i_Point.Column, i_Point.Row,i_Player
            );
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
                    r_CollisionManager.AddObjectToMonitor(newObject);
                    if (newObject.ScreenObjectType == eScreenObjectType.Player)
                    {
                        m_PlayerObjects[newObject.ObjectNumber - 1] = newObject;
                        newObject.SpecialEvent += specialEventInvoked;
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

        public bool DoesGameNeedToKnowIfButtonReleased()
        {
            bool result = false;

            if(m_MoveType == eMoveType.ClickAndRelease)
            {
                result = true;
            }

            return result;
        }
    }
}