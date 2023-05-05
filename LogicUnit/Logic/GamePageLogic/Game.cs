using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//using ABI.Windows.Security.EnterpriseData;
using LogicUnit.Logic.GamePageLogic;
using Microsoft.AspNetCore.SignalR.Client;
using Objects;
using Objects.Enums;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;
using Size = Objects.Size;

namespace LogicUnit
{
    public abstract partial class Game
    {
        private readonly HubConnection r_ConnectionToServer;

        public event EventHandler<List<ScreenObjectAdd>> AddScreenObject;
        public event EventHandler<List<ScreenObjectUpdate>> GameObjectUpdate;
        public event EventHandler<ScreenObjectUpdate> GameObjectToDelete;

        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player = Player.Instance;
        protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        protected eTypeOfGameButtons m_TypeOfGameButtons;
        protected int[] m_AmountOfLivesPlayerHas = new int[4];
        protected Size m_BoardSize = new Size();
        protected int[,] m_Board;
        protected int m_AmountOfActivePlayers;
        protected Buttons m_Buttons = new Buttons();
        protected List<GameObject> m_PlayerGameObjects = new List<GameObject>();
        protected List<GameObject> m_gameObjects = new List<GameObject>();
        public eGameStatus m_GameStatus = eGameStatus.Running;
        protected Random m_randomPosition = new Random();
        protected int m_AmountOfLivesPlayersGetAtStart = 3;
        protected int m_AmountOfPlayersThatAreAlive;
        protected List<ScreenObjectUpdate> m_ScreenObjectUpdate;
        protected List<string> m_LoseOrder = new List<string>();
        protected string m_GameName;
        
        protected List<List<Direction>> m_DirectionsBuffer = new List<List<Direction>>();

        public Game()
        {
            r_ConnectionToServer = new HubConnectionBuilder()
                .WithUrl(Utils.m_GameHubAddress)
                .Build();

            r_ConnectionToServer.On("Update", (int[] i_Update) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    for(int i = 0; i < 2; i++)
                    {
                        ChangeDirection(Direction.getDirection(i_Update[i]), i+1);
                    }

                    gameLoop();
                });
            });

            Task.Run(() =>
                {
                    Application.Current.Dispatcher.Dispatch(async () =>
                    {
                            await r_ConnectionToServer.StartAsync();
                    });
                });
        }

        public void InitializeGame()
        {
            m_BoardSize = m_ScreenMapping.m_TotalScreenSize;
            m_Board = new int[m_BoardSize.m_Width, m_BoardSize.m_Height];
            m_AmountOfPlayersThatAreAlive = m_GameInformation.AmountOfPlayers;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                m_DirectionsBuffer.Add(new List<Direction>());
                m_AmountOfLivesPlayerHas[i] = m_AmountOfLivesPlayersGetAtStart;
            }
            SetGameScreen();
        }

        public bool SetAmountOfPlayers(int i_amountOfPlayers)//for when you want to get ready in lobby 
        {
            if(i_amountOfPlayers >= 2 && i_amountOfPlayers <= 4)
            {
                m_GameInformation.AmountOfPlayers = i_amountOfPlayers;
                m_AmountOfActivePlayers = i_amountOfPlayers;
                return true;
            }
            else
            {
                return false; //If false is returned tell user that the number of players isn't right
            }
        }

        protected eGameStatus PlayerLostALife(int i_Player)
        {
            eGameStatus returnStatus = eGameStatus.Running;
            bool isGameRunning = false;

            m_AmountOfLivesPlayerHas[i_Player - 1]--;

            if (i_Player == m_Player.ButtonThatPlayerPicked)
            {
                //Add a function here that tells the UI to take a heart away
            }

            if (m_AmountOfLivesPlayerHas[i_Player - 1] == 0)
            {
                m_AmountOfPlayersThatAreAlive--;

                if(m_AmountOfPlayersThatAreAlive > 1)//Only player lost but game is still running
                {
                    returnStatus = eGameStatus.Lost;
                    m_LoseOrder.Add(m_GameInformation.m_NamesOfAllPlayers[i_Player]);
                    gameStatusUpdate(returnStatus);
                }
                else//only one player is alive so the game has ended 
                {
                    returnStatus = eGameStatus.Ended;
                    m_LoseOrder.Add(m_GameInformation.m_NamesOfAllPlayers[i_Player]);
                    showScoreBoard();
                }
            }

            return returnStatus;
        }

        protected void showScoreBoard()
        {

        }

        protected void gameStatusUpdate(eGameStatus i_Status)
        {
            //send ui to show defeated
        }

        private bool checkThatPointIsOnBoardGrid(Point i_Point)
        {
            bool isPointOnTheBoard = true;

            if(i_Point.m_Row < 0 || i_Point.m_Column < 0 || i_Point.m_Column > m_BoardSize.m_Width
               || i_Point.m_Row > m_BoardSize.m_Height)
            {
                isPointOnTheBoard = false;
            }
            else if(m_GameInformation.AmountOfPlayers == 3)
            {
                //check special boards
            }

            return isPointOnTheBoard;
        }

        protected bool isPointOnBoard(Point i_Point)
        {
            bool isPointOnTheBoard = true;

            if(i_Point.m_Row < 0 || i_Point.m_Row >= m_BoardSize.m_Height || i_Point.m_Column < 0
               || i_Point.m_Column >= m_BoardSize.m_Width)
            {
                isPointOnTheBoard = false;
            }

            return isPointOnTheBoard;
        }

        protected eGameStatus ClientLostGame(string i_NameOfClientThatLost)
        {
            eGameStatus returnStatus = eGameStatus.Running;

            m_AmountOfActivePlayers--;

            if (i_NameOfClientThatLost == m_Player.Name)
            {
                // Add a function here that tells the UI what to show to the client in case of 
            }

            if(m_AmountOfActivePlayers == 0)
            {
                returnStatus = eGameStatus.Ended;
            }

            return returnStatus;
        }

        protected virtual async Task gameLoop()
        {

        }

        protected void OnDeleteGameObject(int i_Player)
        {
            GameObjectToDelete.Invoke(this, m_PlayerGameObjects[i_Player - 1].GetObjectUpdate());
        }

        protected void OnUpdateScreenObject(List<ScreenObjectUpdate> i_Update)
        {
            GameObjectUpdate.Invoke(this,i_Update);
        }

        protected virtual void ChangeDirection(Direction i_Direction, int i_Player)
        {
            m_PlayerGameObjects[i_Player - 1].m_Direction = i_Direction;
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (m_GameStatus == eGameStatus.Running)
            {

                SendServerMoveUpdate(m_Buttons.StringToButton(button.ClassId));
                //notifyGameObjectUpdate(eScreenObjectType.Player, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());
            }
        }

        public async Task SendServerMoveUpdate(eButton i_Button)
        {
            await r_ConnectionToServer.SendAsync(
                "MoveUpdate",
                m_Player.ButtonThatPlayerPicked-1,(int)i_Button);
        }

        protected void addGameBoardObject(eScreenObjectType i_Type,Point i_Point,int i_ObjectNumber,int i_BoardNumber,string i_Version, bool i_ToInitialize)
        {
            string png = generatePngString(i_Type, i_ObjectNumber, i_Version);
            GameObject gameObject = new GameObject(); 

            if(i_ToInitialize)
            {
                gameObject.Initialize(i_Type,i_ObjectNumber, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd);

                if (i_Type == eScreenObjectType.Player)
                {
                    m_PlayerGameObjects.Add(gameObject);
                }
                else
                {
                    m_gameObjects.Add(gameObject);
                }
            }

            ScreenObjectAdd objectAdd = new ScreenObjectAdd(i_Type, null, i_Point, m_ScreenMapping.m_MovementButtonSize, png, string.Empty, i_ObjectNumber);

            if(i_Type == eScreenObjectType.Player)
            {
                m_PlayerGameObjects[i_ObjectNumber - 1].SetObject(ref objectAdd);
            }
            else
            {
                m_gameObjects[i_ObjectNumber - 1].SetObject(ref objectAdd);
            }

            m_ScreenObjectList.Add(objectAdd);
            m_Board[i_Point.m_Column, i_Point.m_Row] = i_BoardNumber;
        }

        protected string generatePngString(eScreenObjectType i_Type, int i_ObjectNumber, string i_Version)
        {
            string png;

            png = m_GameName + i_Type.ToString() + i_ObjectNumber + i_Version + ".png";
            
            return png.ToLower();
        }


        protected void notifyGameObjectUpdate(eScreenObjectType i_ObjectType,int i_ObjectNumber,Direction i_Direction, Point i_Point)
        {
            if(i_ObjectType == eScreenObjectType.Player)
            {
                ChangeDirection(i_Direction,i_ObjectNumber);
            }
            else
            {
                ChangeGameObject(i_ObjectNumber,i_Direction,i_Point);
            }
        }

        // notifyGameObjectUpdate(eScreenObjectType.Player, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());

        protected virtual void ChangeGameObject(int i_ObjectNumber, Direction i_Direction, Point i_Point)
        {

        }
        public virtual async void RunGame()
        {

        }
    }
}
