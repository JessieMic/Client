using Objects.Enums;
using Point = Objects.Point;
using Objects;
using Objects.Enums.BoardEnum;
using System.Numerics;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Microsoft.AspNetCore.SignalR.Client;

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    public class BombIt : Game
    {
        private BombItPlayer[] m_BombItPlayers;
        BombItBoard m_BombItBoard = new BombItBoard();
        private int m_FoodCounterForPlayerScreen = 0;
        private int m_AmountOfScreenThatHaveNoFood = 0;
        private int j = 0;

        public BombIt(InGameConnectionManager i_GameConnectionManager)
            : base(i_GameConnectionManager)
        {
            m_GameName = "bombit";
            m_MoveType = eMoveType.ClickAndRelease;
            m_Buttons.m_AmountOfExtraButtons = 1;
            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 3;
            m_BombItPlayers = new BombItPlayer[r_GameInformation.AmountOfPlayers];
        }

        void createBoard()
        {
            m_Board = m_BombItBoard.BuildBoardMatrix(m_BoardSizeByGrid.Width, m_BoardSizeByGrid.Height);
            for (int col = 0; col < m_BoardSizeByGrid.Width; col++)
            {
                for (int row = 0; row < m_BoardSizeByGrid.Height; row++)
                {
                    if (m_Board[col, row] == 1)
                    {
                        addBoarder(new Point(col, row));
                    }
                    else if (m_Board[col, row] == 2)
                    {
                        m_GameObjectsToAdd.Add(new BreakableBoarder(new Point(col, row)));
                    }
                }
            }
            addBoarderFor3Players();
        }

        protected override void specialEventInvoked(object i_Sender, int i_eventNumber)
        {
            if(i_eventNumber == -1)
            {
                base.specialEventInvoked(i_Sender, i_eventNumber);
            }
        }

        protected override void addBoarder(Point i_Point)
        {
            Random random = new Random();

            m_GameObjectsToAdd.Add(new Boarder(new Point(i_Point.Column, i_Point.Row), $"bombitwall{random.Next(1,3)}.png"));
        }

        protected override void SpecialUpdateReceived(SpecialUpdate i_SpecialUpdate)
        {
            if (i_SpecialUpdate.Update < 4)
            {
                PlayerGothit(i_SpecialUpdate.Player_ID);
            }
            else
            {
                base.SpecialUpdateReceived(i_SpecialUpdate);
            }
        }

        protected override void SpecialUpdateWithPointReceived(SpecialUpdate i_SpecialUpdate)
        {
            m_BombItPlayers[i_SpecialUpdate.Player_ID - 1].PlaceBomb(new Point(i_SpecialUpdate.X, i_SpecialUpdate.Y));
            
        }

        private void PlayerGothit(int i_Player)
        {
            double startTimeOfDeathAnimation = r_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;

            m_BombItPlayers[i_Player - 1].AmountOfLives--;
            m_BombItPlayers[i_Player - 1].DeathAnimation(startTimeOfDeathAnimation);

            base.PlayerLostALife(null, i_Player);
        }

        protected override void AddGameObjects()
        {
            createBoard();
            addPlayersAndBombsObjects();
        }

        private void addPlayersAndBombsObjects()
        {
            for (int i = 1; i <= r_GameInformation.AmountOfPlayers; i++)
            {
                BombItPlayer newPlayer = new BombItPlayer(
                    i,
                    m_BoardSizeByGrid.Width,
                    m_BoardSizeByGrid.Height,
                    ref m_Board);

                m_GameObjectsToAdd.Add(newPlayer);
                m_BombItPlayers[i - 1] = newPlayer;
                m_GameObjectsToAdd.Add(newPlayer.Bomb);
                m_GameObjectsToAdd.AddRange(m_BombItPlayers[i-1].GetExplosions());
            }
        }

        protected override void checkForGameStatusUpdate()
        {
            if (m_Hearts.m_AmountOfPlayersThatAreAlive <= 1)//Search for the player that is alive
            {
                j++;
                if(j == 5 && m_GameStatus != eGameStatus.Ended)
                {
                    if(m_Hearts.m_AmountOfPlayersThatAreAlive == 0)
                    {
                        m_EndGameText = "Everyone lost!!";
                    }
                    else
                    {
                        string nameOfWinner = m_Hearts.GetNameOfPlayerThatIsAlive();
                        m_EndGameText = $"{nameOfWinner} won!!";
                    }

                    m_ScoreBoard.ShowScoreBoard(m_EndGameText, m_PauseMenu);
                    //OnAddScreenObjects();
                    m_GameStatus = eGameStatus.Ended;
                }
            }
        }
        
        public override void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (m_Buttons.StringToButton(button!.ClassId) == eButton.ButtonA)
            {
                BombPlaceRequest();
            }
            else
            {
                base.OnButtonClicked(sender, e);
            }
        }

        private void BombPlaceRequest()
        {
            if(m_BombItPlayers[r_GameInformation.Player.PlayerNumber-1].CanPlaceABomb)
            {
                SendServerSpecialPointUpdate(m_BombItPlayers[r_GameInformation.Player.PlayerNumber-1].RequestPlaceBomb(), r_GameInformation.Player.PlayerNumber);
            }
        }
    }
}
