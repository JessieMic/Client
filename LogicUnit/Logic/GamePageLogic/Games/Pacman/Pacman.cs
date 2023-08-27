using Objects.Enums;
using Point = Objects.Point;
using Objects;
using Objects.Enums.BoardEnum;
using System.Numerics;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class Pacman : Game
    {

        private IPacmanGamePlayer[] m_PacmanPlayers;
        PacmanBoardFactory m_PacmanBoard = new PacmanBoardFactory();
        private int m_FoodCounterForPlayerScreen = 0;
        private int m_AmountOfScreenThatHaveNoFood = 0;

        public Pacman()
        {
            m_GameName = "pacman";
            m_MoveType = eMoveType.ClicKOnce;
            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 2;
            m_PacmanPlayers = new IPacmanGamePlayer[m_GameInformation.AmountOfPlayers];
        }

        void createBoard()
        {
            m_Board = m_PacmanBoard.BuildBoardMatrix(m_BoardSizeByGrid.Width, m_BoardSizeByGrid.Height);
            for (int col = 0; col < m_BoardSizeByGrid.Width; col++)
            {
                for (int row = 0; row < m_BoardSizeByGrid.Height; row++)
                {
                    if (m_Board[col, row] == 1)
                    {
                        addBoarder(new Point(col, row));
                    }
                    else if (m_Board[col, row] == 0)
                    {
                        m_GameObjectsToAdd.Add(new Food(new Point(col, row), ref m_FoodCounterForPlayerScreen));
                    }
                    else if (m_Board[col, row] == 2)
                    {
                        m_GameObjectsToAdd.Add(new Cherry(new Point(col, row)));
                    }
                }
            }

            addBoarderFor3Players();
        }

        protected override void addBoarder(Point i_Point)
        {
            m_GameObjectsToAdd.Add(new Boarder(new Point(i_Point.Column, i_Point.Row), "pacman_boarder.png"));
        }

        protected override void SpecialUpdateReceived(SpecialUpdate i_SpecialUpdate)
        {
            if (i_SpecialUpdate.Update == (int)ePacmanSpecialEvents.GotHit)
            {
                PlayerGothit(i_SpecialUpdate.Player_ID);
            }
            else if (i_SpecialUpdate.Update == (int)ePacmanSpecialEvents.AteCherry)
            {
                pacmanAteCherry();
            }
            else if(i_SpecialUpdate.Update > 6)
            {
                base.SpecialUpdateReceived(i_SpecialUpdate);
            }
            else
            {
                m_FoodCounterForPlayerScreen++;
                System.Diagnostics.Debug.WriteLine($"(FOOD) - Player num- {m_GameInformation.Player.PlayerNumber} CURR NUM {m_FoodCounterForPlayerScreen}");
                if (m_FoodCounterForPlayerScreen == m_GameInformation.AmountOfPlayers)
                {
                    m_EndGameText = "Pacman won!!";
                    m_ScoreBoard.ShowScoreBoard(m_EndGameText, m_PauseMenu);
                    m_GameStatus = eGameStatus.Ended;
                }
            }
        }

        private void PlayerGothit(int i_Player)
        {
            double startTimeOfDeathAnimation = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            if (i_Player == 1)
            {
                stopMovement(m_Player.PlayerNumber);
                foreach (var player in m_PacmanPlayers)
                {
                    player.ResetPosition(startTimeOfDeathAnimation);
                }
            }
            else
            {
                m_PacmanPlayers[i_Player - 1].AmountOfLives--;
                m_PacmanPlayers[i_Player - 1].ResetPosition(startTimeOfDeathAnimation);
                stopMovement(i_Player);
            }
            base.PlayerLostALife(null, i_Player);
        }

        private void pacmanAteCherry()
        {
            double startTimeOfCherryTime = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            foreach (var player in m_PacmanPlayers)
            {
                player.InitiateCherryTime(startTimeOfCherryTime);
            }
        }

        protected override void AddGameObjects()
        {
            createBoard();
            addPlayerObjects();
        }

        private void addPlayerObjects()
        {
            PacmanObject player1 = new PacmanObject(m_Board);
            m_PacmanPlayers[0] = player1;
            m_GameObjectsToAdd.Add(player1);

            for (int i = 2; i <= m_GameInformation.AmountOfPlayers; i++)
            {
                GhostObject newGhost = new GhostObject(
                    i,
                    m_BoardSizeByGrid.Width,
                    m_BoardSizeByGrid.Height,
                    m_Board);

                m_GameObjectsToAdd.Add(newGhost);
                m_PacmanPlayers[i - 1] = newGhost;
            }
        }

        protected override void checkForGameStatusUpdate()
        {
            if(m_GameStatus != eGameStatus.Ended)
            {
                if (m_Hearts.m_AmountOfLivesPlayerHas[0] == 0)
                {
                    if (m_AmountOfPlayers > 2)
                    {
                        m_EndGameText = "Ghosts won!!";
                    }
                    else
                    {
                        m_EndGameText = "Ghost won!!";
                    }
                    m_ScoreBoard.ShowScoreBoard(m_EndGameText, m_PauseMenu);
                    m_GameStatus = eGameStatus.Ended;
                }
                else if (m_Hearts.m_AmountOfPlayersThatAreAlive == 1)
                {
                    m_EndGameText = "Pacman won!!";
                    m_ScoreBoard.ShowScoreBoard(m_EndGameText, m_PauseMenu);
                    m_GameStatus = eGameStatus.Ended;
                }
            }
        }
    }
}