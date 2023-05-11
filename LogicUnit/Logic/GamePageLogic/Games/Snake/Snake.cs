using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//using LogicUnit.Logic.GamePageLogic;
using Objects;
using Objects.Enums;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;
using Size = Objects.Size;

namespace LogicUnit.Logic.GamePageLogic.Games.Snake
{
    public class Snake : Game
    {
        private List<SnakeObject> m_PlayersSnakes = new List<SnakeObject>();
        private Food m_food = new Food();

        public Snake()
        {
            m_GameName = "Snake";
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
            m_Buttons.m_AmountOfExtraButtons = 2;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
            m_scoreBoard.m_ShowScoreBoardByOrder = true;
        }

        public override async void RunGame()
        {
            await gameLoop();
        }

        protected override async Task gameLoop()
        {
            while (m_GameStatus == eGameStatus.Running)
            {
                await Task.Delay(400);

                m_gameObjectsToUpdate = new List<GameObject>();
                m_GameObjectsToAdd = new List<GameObject>();
                moveSnakes();
                OnUpdateScreenObject();

                if (m_GameObjectsToAdd.Count != 0)
                {
                    OnAddScreenObjects();
                }
            }
        }

        private void updateSnake()
        {
            foreach (var snake in m_PlayersSnakes)
            {
                snake.update();
            }
        }

        protected override void ChangeGameObject(int i_ObjectNumber, Direction i_Direction, Point i_Point)
        {
            updateFoodToNewPoint(i_Point);
        }

        protected override void AddGameObjects()
        {
            for (int player = 1; player <= m_GameInformation.AmountOfPlayers; player++)
            {
                addPlayerObjects(player);
            }
            addFood();
        }

        private void addPlayerObjects(int i_Player)
        {
            Point point = new Point(0, 1);
            int until = 0;
            int inc;
            bool toCombine = false;
            SnakeObject snake = new SnakeObject(ref m_Board);

            if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Column == eColumnPosition.LeftColumn)// (i_Player <= 2)
            {
                point.m_Column = 3;//column = 3;
                until = 0;
                inc = -1;
            }
            else
            {
                point.m_Column = m_BoardSize.m_Width - 4;
                until = m_BoardSize.m_Width - 1;
                inc = 1;
            }

            if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Row == eRowPosition.LowerRow)
            {
                point.m_Row = m_BoardSize.m_Height - 2;
            }

            while (point.m_Column != until)
            {
                GameObject gameObject = addGameBoardObject_(eScreenObjectType.Player, point, i_Player, i_Player + 2, "body");
                if (!toCombine)
                {
                    snake.set(gameObject);
                    m_PlayersSnakes.Add(snake);
                }
                else
                {
                    snake.CombineGameObjects(gameObject);
                }
                toCombine = true;
                point.m_Column += inc;
            }
        }

        private void addFood()
        {
            List<Point> emptyPositions = getEmptyPositions();
            Point randomPoint = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];

            m_food.set(addGameBoardObject_(
                eScreenObjectType.Object, randomPoint, 1, (int)eBoardObjectSnake.Food,
                eBoardObjectSnake.Food.ToString()));
        }
        private void updateFoodToNewPoint(Point i_Point)
        {
            m_food.PopPoint();
            m_food.AddPointTop(i_Point);
            m_gameObjectsToUpdate.Add(m_food);
            m_Board[i_Point.m_Column, i_Point.m_Row] = (int)eBoardObjectSnake.Food;
        }

        private void getNewPointForFood()
        {
            List<Point> emptyPositions = getEmptyPositions();
            Point point;

            if (emptyPositions.Count != 0)
            {
                point = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];
            }
            else
            {
                point = new Point(-1, -1);
            }

            notifyGameObjectUpdate(eScreenObjectType.Object, 1, null, point);
        }

        public List<Point> getEmptyPositions()
        {
            List<Point> res = new List<Point>();

            for (int col = 0; col < m_BoardSize.m_Width; col++)
            {
                for (int row = 0; row < m_BoardSize.m_Height; row++)
                {
                    if (m_Board[col, row] == 0)
                    {
                        res.Add(new Point(col, row));
                    }
                }
            }

            return res;
        }

        protected override void ChangeDirection(Direction i_Direction, int i_Player)
        {
            m_PlayersSnakes[i_Player - 1].m_Direction = i_Direction;
            //if (canChangeDirection(i_Direction, i_Player))
            //{
            //    m_DirectionsBuffer[i_Player - 1].Add(i_Direction);
            //}
        }

        private Direction getLastDirection(int i_Player)
        {
            Direction result;
            if (m_DirectionsBuffer[i_Player - 1].Count == 0)
            {
                result = m_PlayersSnakes[i_Player - 1].m_Direction;
            }
            else
            {
                result = m_DirectionsBuffer[i_Player].Last();
            }

            return result;
        }

        private bool canChangeDirection(Direction i_NewDirection, int i_Player)
        {
            bool result = true;

            if (m_DirectionsBuffer[i_Player - 1].Count == 2)
            {
                result = false;
            }

            Direction lastDirection = getLastDirection(i_Player);

            return i_NewDirection != lastDirection && i_NewDirection != lastDirection.OppositeDirection();
        }

        private void moveSnakes()
        {
            int player = 1;

            foreach (var snake in m_PlayersSnakes)
            {
                if (m_Hearts.m_AmountOfLivesPlayerHas[player - 1] > 0 && snake.m_Direction != Direction.Stop)
                {
                    Point newHeadPoint = snake.GetOneMoveAhead();
                    int hit = snake.whatSnakeWillHit(newHeadPoint, isPointOnBoard(newHeadPoint));

                    if (hit == (int)eBoardObjectSnake.OutOfBounds || hit >= 3)
                    {
                        //PlayerGotHit(player);
                    }

                    else if (hit == (int)eBoardObjectSnake.Empty)//Normal move
                    {
                        snake.removeTail();//removeTail(player);
                        snake.addHead(newHeadPoint);
                    }
                    else if (hit == (int)eBoardObjectSnake.Food)//Eats food
                    {
                        snake.addHead(newHeadPoint);


                        snake.CombineGameObjects(addGameBoardObject_(eScreenObjectType.Player, newHeadPoint, player, player, "body"));
                        //score ++

                        if (m_Player.ButtonThatPlayerPicked == 1)
                        {
                            getNewPointForFood();
                        }
                    }
                    m_gameObjectsToUpdate.Add(snake);
                }
                player++;
            }
        }

        private void PlayerGotHit(int i_Player)
        {
            //OnDeleteGameObject(m_PlayersSnakes[i_Player-1]);
            PlayerLostALife(i_Player);//general ui update
        }
    }
}

//if (m_DirectionsBuffer[player - 1].Count > 0)
//{
//    m_PlayersSnakes[player - 1].m_Direction = m_DirectionsBuffer[player - 1].First();
//    m_DirectionsBuffer[player - 1].RemoveAt(0);
//}