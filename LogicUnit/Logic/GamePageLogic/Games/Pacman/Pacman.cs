using Objects.Enums;
using Point = Objects.Point;
using Objects;
using Objects.Enums.BoardEnum;
using System.Numerics;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class Pacman : Game
    {
        private PacmanObject m_Pacman;
        private List<GameObject> m_Ghosts = new List<GameObject>();

        public Pacman()
        {
            m_GameName = "Pacman";
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1; // was 3
            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
        }


        protected virtual void ChangeDirection(Direction i_Direction, int i_Player, int i_LoopNumber)
        {
            //m_Pacman.ChangePosition();
        }

        protected override void AddGameObjects()
        {
            m_Pacman = new PacmanObject();
            m_GameObjectsToAdd.Add(m_Pacman);
            m_Ghosts.Add(new GhostObject());
            m_GameObjectsToAdd.Add(m_Ghosts[0]);
            m_GameObjectsToAdd.Add(new Boarder(new Point(1,1)));
            m_GameObjectsToAdd.Add(new Boarder(new Point(1, 3)));
            m_GameObjectsToAdd.Add(new Passage(new Point(0, 2)));
            //m_CollisionManager.AddObjectToMonitor(new Passage(new Point(4, 3)));
            for (int i = 1; i < m_GameInformation.AmountOfPlayers; i++)
            {
                //addPlayerObjects(i);
            }

            //addFood();
        }

        private void addFood()
        {
            int i = 0;
            Point point;
            Food food = new Food();

            for (int col = 0; col < m_BoardSizeByGrid.Width; col++)
            {
                for (int row = 0; row < m_BoardSizeByGrid.Height; row++)
                {
                    if (m_Board[col, row] == 0)
                    {
                        //point = new Point(col,row);
                        //food.set(addGameBoardObject(eScreenObjectType.Object, point, 1,
                        //    (int)eBoardObjectPacman.Food, eBoardObjectPacman.Food.ToString()));
                        ////m_Food.Add(food);
                        //m_Food.Add(point, food);
                    }
                }
            }
        }

        private void addPlayerObjects(int i_Player)
        {
            Point point = new Point(1, 1);
            eColumnPosition playerCol = m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Column;
            eRowPosition playerRow = m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Row;

            if (playerCol == eColumnPosition.RightColumn)
            {
                point.Column = m_BoardSizeByGrid.Width - 2;
            }

            if (playerRow == eRowPosition.LowerRow)
            {
                point.Row = m_BoardSizeByGrid.Height - 2;
            }

            GameObject gameObject = addGameBoardObject(eScreenObjectType.Player, point, i_Player, i_Player, "body");
            gameObject.FadeWhenObjectIsRemoved();

            if (i_Player % 2 == 1)
            {
                //PacmanObject pacman = new PacmanObject();
                //m_GameObjectsToAdd.Add(pacman);
                //m_AllPlayers.Add(pacman);
                //m_MoveableGameObjects.Add(pacman);
            }
            else
            {
                //GhostObject ghost = new GhostObject(ref m_Board);
                //ghost.set(gameObject);
                ////m_GhostPlayers.Add(ghost);
                //m_AllPlayers.Add(ghost);
            }
        }

        private void moveObjects()
        {
            //int pacmanNum = 1;

            //foreach (var player in m_AllPlayers)
            //{
            //    if (player is PacmanObject)
            //    {
            //        PacmanObject pacman = player as PacmanObject;
            //        if (m_Hearts.m_AmountOfLivesPlayerHas[pacmanNum - 1] > 0 && pacman.m_Direction != Direction.Stop)
            //        {
            //            Point newPoint = pacman.GetOneMoveAhead();
            //            int hit = pacman.WhatPacmanWillHit(newPoint, isPointOnBoard(newPoint));

            //            if (hit == (int)eBoardObjectPacman.OutOfBounds)
            //            {
            //                //stop pacman
            //            }
            //            else if (hit == (int)eBoardObjectPacman.Empty) // normal move
            //            {
            //                pacman.Move(newPoint);
            //            }
            //            else if (hit == (int)eBoardObjectPacman.Food)
            //            {
            //                //pacman.Move(newPoint);
            //                //removeFood(newPoint);
            //                //score++
            //            }
            //            else if (hit == (int)eBoardObjectPacman.Ghost1 || hit == (int)eBoardObjectPacman.Ghost2)
            //            {
            //                pacmanGotHit(pacmanNum);
            //            }

            //            m_gameObjectsToUpdate.Add(pacman);
            //        }
            //    }

            //    pacmanNum++;
            //}
        }

        protected override void ChangeDirection(Direction i_Direction, int i_Player, Point i_Point)
        {
            //m_AllPlayers[i_Player - 1].m_Direction = i_Direction;
        }

        private void pacmanGotHit(int i_Player)
        {
            m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);
            //OnDeleteGameObject(m_PacmanPlayers[i_Player - 1]);
            //OnDeleteGameObject(m_AllPlayers[i_Player - 1]);
            PlayerLostALife(i_Player);
        }

        private void removeFood(Point i_Point)
        {
            //if (m_Food[i_Point] != null)
            //{
            //    Food food = m_Food[i_Point];

            //    food.PopPoint();
            //    OnDeleteGameObject(m_Food[i_Point]);
            //}

        }

        //protected override void ChangeGameObject(int i_ObjectNumber, Direction i_Direction, Point i_Point)
        //{
        //    removeFoodFromPoint(i_Point);
        //}

        //private void removeFoodFromPoint(Point i_Point)
        //{
        //    Food foodToRemove = m_Food[i_Point];
        //    foodToRemove.PopPoint();
        //}
    }
}
