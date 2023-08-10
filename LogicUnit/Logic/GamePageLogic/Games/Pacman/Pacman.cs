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
                        m_GameObjectsToAdd.Add(new Food(new Point(col, row)));
                    }
                    else if (m_Board[col, row] == 2)
                    {
                        m_GameObjectsToAdd.Add(new Cherry(new Point(col, row)));
                    }
                }
            }
        }

        private void pacmanAteCherry()
        {
            foreach(var player in m_PacmanPlayers)
            {
                double currentTime = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
                player.InitiateCherryTime(currentTime);
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
            m_PacmanPlayers[0]=player1;
            m_GameObjectsToAdd.Add(player1);
            player1.AteBerry += pacmanAteCherry;

            for (int i = 2; i <= m_GameInformation.AmountOfPlayers; i++)
            {
                GhostObject newGhost = new GhostObject(
                    i,
                    m_BoardSizeByGrid.Width,
                    m_BoardSizeByGrid.Height,
                    m_Board);

                m_GameObjectsToAdd.Add(newGhost);
                m_PacmanPlayers[i-1] = newGhost;
            }
        }

        protected override void PlayerLostALife(object sender, int i_Player)
        {
            double startTimeOfDeathAnimation = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            
            if(i_Player==1)//Pacman got hit so we reset all positions
            {
                
                foreach(var player in m_PacmanPlayers)
                {
                    player.ResetPosition(startTimeOfDeathAnimation);
                }
            }
            else
            {
                m_PacmanPlayers[i_Player-1].ResetPosition(startTimeOfDeathAnimation);
            }
            base.PlayerLostALife(sender,i_Player);
        }
    }
}
