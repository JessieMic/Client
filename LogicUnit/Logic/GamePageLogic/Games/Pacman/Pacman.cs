using Objects.Enums;
using Point = Objects.Point;
using Objects;
using Objects.Enums.BoardEnum;
using System.Numerics;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class Pacman : Game
    {
        //Ghost m_Ghost;
        //private List<PacmanObject> m_PacmanPlayers = new List<PacmanObject>();
        //private List<GhostObject> m_GhostPlayers = new List<GhostObject>();
        //private List<Food> m_Food = new List<Food>();
        private Dictionary<Point, Food> m_Food = new Dictionary<Point, Food>();
        private List<GameObject> m_AllPlayers = new List<GameObject>();

        public Pacman()
        {
            m_GameName = "Pacman"; // was Snake
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1; // was 3
            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
        }

        public override void RunGame()
        {
            //gameLoop();
        }

        protected override async Task gameLoop() // copied from snake
        {
            m_gameObjectsToUpdate = new List<GameObject>(); // -> objects that need to be updated (pacman that moves)
            m_GameObjectsToAdd = new List<GameObject>(); // -> new objects on screen
            moveObjects();
            if (m_GameObjectsToAdd.Count != 0)
            {
                OnAddScreenObjects();
            }

            if (m_gameObjectsToUpdate.Count != 0)
            {
                OnUpdateScreenObject();
            }

            //while (m_GameStatus == eGameStatus.Running)
            //{
            //    //await Task.Delay(400);

            //    m_Ghost.m_Direction = Direction.Right;
            //    m_Ghost.MoveSameDirection();
            //   // await Task.Delay(400);
            //    m_Ghost.m_Direction = Direction.Down;
            //    m_Ghost.MoveSameDirection();
            //    //await Task.Delay(400);
            //    m_Ghost.MoveToPoint(new Point(1, 1));

            //}
        }

        protected override void AddGameObjects()
        {
            //m_Ghost = new Ghost();
            //GameObject obj = addGameBoardObject_(eScreenObjectType.Player, new Point(1, 1), 1, 1, "body");
            //m_Ghost.set(obj);

            for (int i = 1; i <= m_GameInformation.AmountOfPlayers; i++)
            {
                addPlayerObjects(i);
            }

            addFood();
        }

        private void addFood()
        {
            int i = 0;
            Point point;
            Food food = new Food();

            for (int col = 0; col < m_BoardSize.m_Width; col++)
            {
                for (int row = 0; row < m_BoardSize.m_Height; row++)
                {
                    if (m_Board[col, row] == 0)
                    {
                        point = new Point(col,row);
                        food.set(addGameBoardObject_(eScreenObjectType.Object, point, 1,
                            (int)eBoardObjectPacman.Food, eBoardObjectPacman.Food.ToString()));
                        //m_Food.Add(food);
                        m_Food.Add(point, food);
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
                point.m_Column = m_BoardSize.m_Width - 2;
            }

            if (playerRow == eRowPosition.LowerRow)
            {
                point.m_Row = m_BoardSize.m_Height - 2;
            }

            GameObject gameObject = addGameBoardObject_(eScreenObjectType.Player, point, i_Player, i_Player, "body");
            gameObject.FadeWhenObjectIsRemoved();

            if (i_Player % 2 == 1)
            {
                PacmanObject pacman = new PacmanObject(ref m_Board);
                pacman.set(gameObject);
                //m_PacmanPlayers.Add(pacman);
                m_AllPlayers.Add(pacman);
            }
            else
            {
                GhostObject ghost = new GhostObject(ref m_Board);
                ghost.set(gameObject);
                //m_GhostPlayers.Add(ghost);
                m_AllPlayers.Add(ghost);
            }
        }

        private void moveObjects()
        {
            int pacmanNum = 1;

            foreach (var player in m_AllPlayers)
            {
                if (player is PacmanObject)
                {
                    PacmanObject pacman = player as PacmanObject;
                    if (m_Hearts.m_AmountOfLivesPlayerHas[pacmanNum - 1] > 0 && pacman.m_Direction != Direction.Stop)
                    {
                        Point newPoint = pacman.GetOneMoveAhead();
                        int hit = pacman.WhatPacmanWillHit(newPoint, isPointOnBoard(newPoint));

                        if (hit == (int)eBoardObjectPacman.OutOfBounds)
                        {
                            //stop pacman
                        }
                        else if (hit == (int)eBoardObjectPacman.Empty) // normal move
                        {
                            pacman.Move(newPoint);
                        }
                        else if (hit == (int)eBoardObjectPacman.Food)
                        {
                            pacman.Move(newPoint);
                            //removeFood(newPoint);
                            //score++
                        }
                        else if (hit == (int)eBoardObjectPacman.Ghost1 || hit == (int)eBoardObjectPacman.Ghost2)
                        {
                            pacmanGotHit(pacmanNum);
                        }

                        m_gameObjectsToUpdate.Add(pacman);
                    }
                }

                pacmanNum++;
            }
        }

        protected override void ChangeDirection(Direction i_Direction, int i_Player)
        {
            m_AllPlayers[i_Player - 1].m_Direction = i_Direction;
        }

        private void pacmanGotHit(int i_Player)
        {
            m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);
            //OnDeleteGameObject(m_PacmanPlayers[i_Player - 1]);
            OnDeleteGameObject(m_AllPlayers[i_Player - 1]);
            PlayerLostALife(i_Player);
        }

        private void removeFood(Point i_Point)
        {
            if (m_Food[i_Point] != null)
            {
                Food food = m_Food[i_Point];

                food.PopPoint();
                OnDeleteGameObject(m_Food[i_Point]);
            }

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
