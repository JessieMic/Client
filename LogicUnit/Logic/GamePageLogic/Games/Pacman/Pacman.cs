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
                        m_GameObjectsToAdd.Add(new Boarder(new Point(col, row)));
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

            if(m_AmountOfPlayers == 3)
            {
                int y = m_ScreenMapping.m_Boundaries.Height;
                int x = m_ScreenMapping.m_Boundaries.Width;

                for(int i = y; i < m_BoardSizeByGrid.Height; i++)
                {
                    m_GameObjectsToAdd.Add(new Boarder(new Point(x, i)));
                }
                for (int i = x; i < m_BoardSizeByGrid.Width; i++)
                {
                    m_GameObjectsToAdd.Add(new Boarder(new Point(i, y)));
                }
            }
        }

        protected override void SpecialUpdateReceived(int i_WhatHappened, int i_Player)
        {
            if (i_WhatHappened == (int)ePacmanSpecialEvents.GotHit)
            {
                PlayerGothit(i_Player);
            }
            else if (i_WhatHappened == (int)ePacmanSpecialEvents.AteCherry)
            {
                pacmanAteCherry();
            }
            else
            {
                m_FoodCounterForPlayerScreen++;
                System.Diagnostics.Debug.WriteLine($"(FOOD) - Player num- {m_GameInformation.m_Player.PlayerNumber} CURR NUM {m_FoodCounterForPlayerScreen}");
                if (m_FoodCounterForPlayerScreen == m_GameInformation.AmountOfPlayers)
                {
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
    }
}