using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//using AuthenticationServices;
//using LogicUnit.Logic.GamePageLogic;
using Objects;
using Objects.Enums;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Snake
{
    public class Snake : Game
    {
        private List<SnakeObject> m_PlayersSnakes = new List<SnakeObject>();
        private Food m_food = new Food();
        private List<Direction> m_SnakeLastDirection = new List<Direction>();
        int o = 0;
        public Snake()
        {
            m_GameName = "Snake";
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
            //m_Buttons.m_AmountOfExtraButtons = 2;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
            m_scoreBoard.m_ShowScoreBoardByOrder = true;
        }

        private void Init()
        {

        }

        public override async void RunGame()
        {
            Thread newThread = new(actualGameLoop) { Name = "SnakeLoop" };
            newThread.Start();
            //Task.Run(actualGameLoop);
            //Task.Run(actualGameLoop);

        }

        private void actualGameLoop()

        //private async Task actualGameLoop()
        {
            while(m_GameStatus != eGameStatus.Restarted && m_GameStatus != eGameStatus.Ended)
            {
                if(m_GameStatus == eGameStatus.Running)
                {
                    lock (m_PlayersDirectionsFromServer)
                    {
                        foreach (int player in m_PlayersDirectionsFromServer.Keys.Where(i_Player => i_Player != m_Player.ButtonThatPlayerPicked))
                        {
                            ChangeDirection(m_PlayersDirectionsFromServer[player], player);
                        }
                    }
                    gameLoop();
                    Thread.Sleep(200);
                    //await Task.Delay(500);
                }
                Thread.Sleep(200);
            }
        }

        protected override void gameLoop()
        {
            if(m_GameStatus == eGameStatus.Running)
            {
                m_gameObjectsToUpdate = new List<GameObject>();
                m_GameObjectsToAdd = new List<GameObject>();
                moveSnakes();
                if (m_GameObjectsToAdd.Count != 0)
                {
                    OnAddScreenObjects();
                }

                if (m_gameObjectsToUpdate.Count != 0)
                {
                    OnUpdateScreenObject();
                }
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
                point.m_Column = m_BoardOurSize.m_Width - 4;
                until = m_BoardOurSize.m_Width - 1;
                inc = 1;
            }

            if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Row == eRowPosition.LowerRow)
            {
                point.m_Row = m_BoardOurSize.m_Height - 2;
            }

            int i = 0;
            while (point.m_Column != until) //(i == 0)//
            {
                i++;
                GameObject gameObject = addGameBoardObject_(eScreenObjectType.Player, point, i_Player, i_Player + 2,getBodyPartString(i) );
                gameObject.FadeWhenObjectIsRemoved();

                if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player-1].Position.Column == eColumnPosition.RightColumn)
                {
                    gameObject.SetImageDirection(0,Direction.Left);
                    snake.m_Direction = Direction.Left;
                }
                else
                {
                    snake.m_Direction = Direction.Right;
                }

                if (!toCombine)
                {
                    snake.set(gameObject);
                    m_PlayersSnakes.Add(snake);
                    m_SnakeLastDirection.Add(snake.m_Direction);
                }
                else
                {
                    snake.CombineGameObjects(gameObject);
                }
                toCombine = true;
                point.m_Column += inc;
            }
        }

        private string getBodyPartString(int i_Index)
        {
            string bodyPart = eSnakeBodyParts.Head.ToString();

            if(i_Index == 3)
            {
                bodyPart = eSnakeBodyParts.Tail.ToString();
            }
            else if(i_Index == 2)
            {
                bodyPart = eSnakeBodyParts.Body.ToString();
            }

            return bodyPart;
        }

        private void addFood()
        {
            List<Point> emptyPositions = getEmptyPositions();
            Point randomPoint = new Point(5, 1);//emptyPositions[m_randomPosition.Next(emptyPositions.Count)];

            //r_LiteNetClient.Send(randomPoint, (int)i_Button);

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
                m_GameStatus = eGameStatus.Tie;
                //point = new Point(-1, -1);
            }

            //notifyGameObjectUpdate(eScreenObjectType.Object, 1, null, point);
        }

        public List<Point> getEmptyPositions()
        {
            List<Point> res = new List<Point>();

            for (int col = 0; col < m_BoardOurSize.m_Width; col++)
            {
                for (int row = 0; row < m_BoardOurSize.m_Height; row++)
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
            if (i_Direction != Direction.Stop)
            {
                m_PlayersSnakes[i_Player - 1].m_Direction = i_Direction;
            
                m_PlayersSnakes[i_Player - 1].m_IsObjectMoving = true;
            }
            

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
            Direction currentDirection;
            foreach (var snake in m_PlayersSnakes)
            {
                if (m_Hearts.m_AmountOfLivesPlayerHas[player - 1] > 0 && snake.m_IsObjectMoving)
                {
                    currentDirection = snake.m_Direction;
                    Point newHeadPoint = snake.GetOneMoveAhead();
                    int hit = snake.whatSnakeWillHit(newHeadPoint, isPointOnBoard(newHeadPoint));

                    if (o<7 && o>=3 && m_Player.ButtonThatPlayerPicked == 1)//(hit == (int)eBoardObjectSnake.OutOfBounds || hit >= 3)
                    {
                        snake.Eat(addGameBoardObject_(eScreenObjectType.Player, newHeadPoint, player, player + 2,
                            eSnakeBodyParts.Head.ToString()), m_SnakeLastDirection[player - 1], currentDirection);
                        //PlayerGotHit(player);
                        
                    }
                    else if (hit == (int)eBoardObjectSnake.Empty)//Normal move
                    {
                        snake.Move(newHeadPoint, m_SnakeLastDirection[player-1],currentDirection);
                    }
                    else if (hit == (int)eBoardObjectSnake.Food)//Eats food
                    {
                        snake.Eat(addGameBoardObject_(eScreenObjectType.Player, newHeadPoint, player, player+2,
                            eSnakeBodyParts.Head.ToString()), m_SnakeLastDirection[player - 1], currentDirection);

                        if (m_Player.ButtonThatPlayerPicked == 1)
                        {
                            getNewPointForFood();
                        }
                        
                    }
                    o++;
                    m_gameObjectsToUpdate.Add(snake);
                    m_SnakeLastDirection[player - 1] = currentDirection;
                    snake.m_Direction = currentDirection;
                }
                player++;
            }
        }

        private void PlayerGotHit(int i_Player)
        {
            //m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);
            OnDeleteGameObject(m_PlayersSnakes[i_Player - 1]);
            PlayerLostALife(i_Player);//general ui update
        }
    }
}

//if (m_DirectionsBuffer[player - 1].Count > 0)
//{
//    m_PlayersSnakes[player - 1].m_Direction = m_DirectionsBuffer[player - 1].First();
//    m_DirectionsBuffer[player - 1].RemoveAt(0);
//}