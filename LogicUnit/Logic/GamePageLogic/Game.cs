using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic;
using LogicUnit.Logic.GamePageLogic.Games;
using Objects;
using Objects.Enums;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;
using Size = Objects.Size;

namespace LogicUnit
{
    public abstract partial class Game
    {
        //Events
        public event EventHandler<List<Image>> AddGameObjectList;
        //public event EventHandler<List<GameObject>> GameObjectsUpdate;
        //public event EventHandler<GameObject> GameObjectToDelete;

        //basic game info
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player = Player.Instance;

        //Screen info 
        protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        protected Size m_BoardSize = new Size();

        //Need to initialize each different game
        //protected int[] m_AmountOfLivesPlayerHas = new int[4];
        //protected int m_AmountOfLivesPlayersGetAtStart = 3;
        protected string m_GameName;
        protected ScoreBoard m_scoreBoard = new ScoreBoard();
        protected Hearts m_Hearts = new Hearts();

        //Things that might change while playing 
        protected int[,] m_Board;
        protected int m_AmountOfActivePlayers;
        //protected int m_AmountOfPlayersThatAreAlive;
        public eGameStatus m_GameStatus = eGameStatus.Running;
        protected List<string> m_LoseOrder = new List<string>();

        //Don't mind this
        protected Buttons m_Buttons = new Buttons();
        protected Random m_randomPosition = new Random();
        protected List<List<Direction>> m_DirectionsBuffer = new List<List<Direction>>();

        //List for Ui changes
        public List<Image> m_GameImagesToAdd = new List<Image>();

        public Game()
        {
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
            m_Hearts.setHearts(m_GameInformation.AmountOfPlayers, ref m_GameStatus, ref m_LoseOrder);
        }
        public void InitializeGame()
        {
            m_BoardSize = m_ScreenMapping.m_TotalScreenSize;
            m_Board = new int[m_BoardSize.m_Width, m_BoardSize.m_Height];
            
            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                m_DirectionsBuffer.Add(new List<Direction>());
            }
            SetGameScreen();
        }

        protected virtual void OnAddScreenObjects()
        {
            AddGameObjectList.Invoke(this,m_GameImagesToAdd); //..Invoke(this, i_ScreenObject));
        }

        public bool SetAmountOfPlayers(int i_amountOfPlayers)//for when you want to get ready in lobby 
        {
            if(i_amountOfPlayers >= 2 && i_amountOfPlayers <= 4)
            {
                m_GameInformation.AmountOfPlayers = i_amountOfPlayers;
                m_AmountOfActivePlayers = i_amountOfPlayers;
                m_Hearts.m_ClientNumber = m_Player.ButtonThatPlayerPicked;
                return true;
            }
            else
            {
                return false; //If false is returned tell user that the number of players isn't right
            }
        }

        protected void PlayerLostALife(int i_Player)
        {
            m_GameStatus = m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);

            if(m_GameStatus != eGameStatus.Running)
            {
                m_scoreBoard.m_LoseOrder.Add(m_GameInformation.m_NamesOfAllPlayers[i_Player-1]);
                if(m_GameStatus == eGameStatus.Lost)
                {
                    gameStatusUpdate();//Will show client that he lost 
                }
                else //Game has ended 
                {
                    m_scoreBoard.ShowScoreBoard();
                }
            }
        }

        protected void gameStatusUpdate()
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

        protected virtual async Task gameLoop()
        {

        }

        //protected void OnDeleteGameObject(GameObject i_ObjectToDelete)
        //{
        //    GameObjectToDelete.Invoke(this, i_ObjectToDelete);
        //}

        protected virtual void ChangeDirection(Direction i_Direction, int i_Player)
        {
            //m_PlayerGameObjects[i_Player - 1].m_Direction = i_Direction;
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (m_GameStatus == eGameStatus.Running)
            {
                notifyGameObjectUpdate(eScreenObjectType.Player, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());
            }
        }

        protected GameObject addGameBoardObject(eScreenObjectType i_Type, Point i_Point, int i_ObjectNumber, int i_BoardNumber, string i_Version)
        {
            string png = generatePngString(i_Type, i_ObjectNumber, i_Version);
            GameObject gameObject = new GameObject();

            gameObject.Initialize(i_Type, i_ObjectNumber, png, i_Point, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd);

            m_GameImagesToAdd.Add(gameObject.m_Images[0]);
            m_Board[i_Point.m_Column, i_Point.m_Row] = i_BoardNumber;

            return gameObject;
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

        protected virtual void ChangeGameObject(int i_ObjectNumber, Direction i_Direction, Point i_Point)
        {

        }

        private void showPauseMenu()
        {
            //GameObject for menu
            m_Buttons.ShowMenuButtons();
        }

        private void restartGame()
        {

        }

        private void exitGame()
        {

        }

        private void hidePauseMenu()
        {
            //Hide pause menu background
            m_Buttons.hideMenuButtons();
        }

        public virtual async void RunGame()
        {

        }
    }
}