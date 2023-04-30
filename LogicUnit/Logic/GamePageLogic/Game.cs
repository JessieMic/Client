using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ABI.Windows.Security.EnterpriseData;
using LogicUnit.Logic.GamePageLogic;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Size = Objects.Size;

namespace LogicUnit
{
    public abstract partial class Game
    {
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Player m_Player = Player.Instance;
        protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        protected eTypeOfGameButtons m_TypeOfGameButtons;
        protected int[] m_AmountOfLivesPlayerHas = new int[4];
        protected Size m_BoardSize = new Size();
        protected int[,] m_Board;
        protected int m_AmountOfActivePlayers;
        protected Buttons m_Buttons = new Buttons();
        Point PlayerObject = new Point();
        protected List<GameObject> m_PlayerGameObjects = new List<GameObject>();
        protected List<GameObject> m_gameObjects = new List<GameObject>();
        public eGameStatus m_GameStatus = eGameStatus.Running;
        protected Random m_randomPosition = new Random();
        protected List<ScreenObjectUpdate> m_ScreenObjectUpdate;// = new List<ScreenObjectUpdate>();
        public abstract void RunGame();

        protected List<List<Direction>> m_DirectionsBuffer = new List<List<Direction>>();


        public async void InitializeGame()
        {
            m_BoardSize = m_ScreenMapping.m_TotalScreenSize;
            m_Board = new int[m_BoardSize.m_Width, m_BoardSize.m_Height];
            for(int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                m_DirectionsBuffer.Add(new List<Direction>());
                m_AmountOfLivesPlayerHas[i] = 3;
                m_PlayerGameObjects.Add(null);
            }
            await SetGameScreen();
            await gameLoop();
        }

        protected abstract void setGameBoardAndGameObjects();

        public bool SetAmountOfPlayers(int i_amountOfPlayers)
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

        //protected eGameStatus PlayerLostALife(string i_NameOfClientThatLostALife)
        //{
        //    eGameStatus returnStatus = eGameStatus.Running;

        //    m_AmountOfLivesTheClientHas--;

        //    if (i_NameOfClientThatLostALife == m_Player.Name)
        //    {
        //        Add a function here that tells the UI to take a heart away
        //    }

        //    if (m_AmountOfLivesTheClientHas == 0)
        //    {
        //        returnStatus = eGameStatus.Lost;
        //    }

        //    return returnStatus;
        //}

        private bool checkThatPointIsOnBoardGrided(Point i_Point)
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
                notifyGameObjectUpdate(eScreenObjectType.PlayerObject, m_Player.ButtonThatPlayerPicked, Direction.getDirection(button.ClassId), new Point());
            }
        }

        protected void notifyGameObjectUpdate(eScreenObjectType i_ObjectType,int i_ObjectNumber,Direction i_Direction, Point i_Point)
        {
            if(i_ObjectType == eScreenObjectType.PlayerObject)
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
    }
}
