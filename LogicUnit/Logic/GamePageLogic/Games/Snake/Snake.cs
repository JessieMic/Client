//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
////using AuthenticationServices;
////using LogicUnit.Logic.GamePageLogic;
//using Objects;
//using Objects.Enums;
//using Objects.Enums.BoardEnum;
//using Point = Objects.Point;
//using System.Diagnostics;
//using System.Text.Json;

//namespace LogicUnit.Logic.GamePageLogic.Games.Snake
//{
//    public class Snake : Game
//    {
//        private List<SnakeObject> m_PlayersSnakes = new List<SnakeObject>();
//        //private List<SnakePlayer> m_SnakePlayers = new List<SnakePlayer>();
//        private Food m_food = new Food();
//        private List<Direction> m_SnakeLastDirection = new List<Direction>();
//        //private List<SnakeObject>[] m_PastVersions = new List<SnakeObject>[4];
//        protected Dictionary<int, List<SnakeObject>> m_Past = new Dictionary<int, List<SnakeObject>>();
//        bool flag = false;

//        public Snake()
//        {

//            m_GameName = "Snake";
//            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
//            m_Buttons.m_TypeMovementButtons = eTypeOfGameMovementButtons.AllDirections;
//            //m_Buttons.m_AmountOfExtraButtons = 2;
//            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
//            m_scoreBoard.m_ShowScoreBoardByOrder = true;
//            //for (int i = 0; i < 4; i++)
//            //{
//            //    m_PastVersions[i] = new List<SnakeObject>();
//            //}
//        }

//        public override void RunGame()
//        {

//            Thread newThread = new(GameLoop) { Name = "SnakeLoop" };
//            newThread.Start();
//        }

//        protected override Point getPlayerCurrentPointPoint(int i_Player)
//        {
//            return m_PlayersSnakes[i_Player - 1].getSnakeHead();
//        }

//        private List<SnakeObject> cloneSnakes()
//        {
//            List<SnakeObject> clonedSnakesStates = new List<SnakeObject>();
//            foreach (SnakeObject snake in m_PlayersSnakes)
//            {
//                //clonedSnakesStates.Add(snake.Clone);
//            }

//            return clonedSnakesStates;
//        }

//        protected override void Draw()
//        {
//            if (m_GameStatus == eGameStatus.Running)
//            {
//                m_gameObjectsToUpdate = new List<GameObject>();
//                m_GameObjectsToAdd = new List<GameObject>();
//                if (m_GameStopwatch.Elapsed.Milliseconds >= 400)
//                {
//                    //OnUpdatesReceived();
//                    m_LoopNumber++;
//                    moveSnakes();
//                    if(!m_Past.ContainsKey(m_LoopNumber))
//                    {
//                        m_Past.Add(m_LoopNumber, cloneSnakes());
//                    }
//                    if (m_GameObjectsToAdd.Count != 0)
//                    {
//                        OnAddScreenObjects();
//                    }

//                    if (m_gameObjectsToUpdate.Count != 0)
//                    {
//                        OnUpdateScreenObject();
//                    }
//                    m_GameStopwatch.Restart();
//                }
//            }
//        }

//        private void updatefoodd()
//        {
//            //Point p = r_LiteNetClient.PlayersData[m_GameInformation.AmountOfPlayers+1].PlayerPointData;
//            //if (p != m_food.m_PointsOnGrid[0])
//            //{
//            //  updateFoodToNewPoint(p);
//            //OnShowGameObjects(m_food.ID);
//            // }
//        }

//        //protected override void OBgameLoop()
//        //{
//        //    if(m_GameStatus == eGameStatus.Running)
//        //    {

//        //        m_gameObjectsToUpdate = new List<GameObject>();
//        //        m_GameObjectsToAdd = new List<GameObject>();
//        //        moveSnakes();
//        //        if (flag)
//        //        {
//        //            updatefoodd();
//        //        }
//        //        if (m_GameObjectsToAdd.Count != 0)
//        //        {
//        //            OnAddScreenObjects();
//        //        }

//        //        if (m_gameObjectsToUpdate.Count != 0)
//        //        {
//        //            OnUpdateScreenObject();
//        //        }
//        //    }
//        //}


//        protected override void AddGameObjects()
//        {
//            //addFoor();
//            addFood();
//            for (int player = 1; player <= m_GameInformation.AmountOfPlayers; player++)
//            {
//                addPlayerObjects(player);
//            }

//            //if(m_Player.ButtonThatPlayerPicked == 1)
//            //{

//            //}
//        }

//        private void addPlayerObjects(int i_Player)
//        {
//            Point point = new Point(0, 1);
//            int until = 0;
//            int inc;
//            bool toCombine = false;
//            SnakeObject snake = new SnakeObject(ref m_Board);

//            if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Column == eColumnPosition.LeftColumn)// (i_Player <= 2)
//            {
//                point.Column = 3;//column = 3;
//                until = 0;
//                inc = -1;
//            }
//            else
//            {
//                point.Column = m_BoardOurSize.m_Width - 4;
//                until = m_BoardOurSize.m_Width - 1;
//                inc = 1;
//            }

//            if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Row == eRowPosition.LowerRow)
//            {
//                point.Row = m_BoardOurSize.m_Height - 2;
//            }

//            int i = 0;
//            while (point.Column != until) //(i == 0)//
//            {
//                i++;
//                GameObject gameObject = addGameBoardObject_(eScreenObjectType.Player, point, i_Player, i_Player + 2, getBodyPartString(i));
//                gameObject.FadeWhenObjectIsRemoved();

//                if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].Position.Column == eColumnPosition.RightColumn)
//                {
//                    gameObject.SetImageDirection(0, Direction.Left);
//                    snake.m_Direction = Direction.Left;
//                }
//                else
//                {
//                    snake.m_Direction = Direction.Right;
//                }

//                if (!toCombine)
//                {
//                    snake.set(gameObject);
//                    m_PlayersSnakes.Add(snake);
//                    m_SnakeLastDirection.Add(snake.m_Direction);
//                }
//                else
//                {
//                    snake.CombineGameObjects(gameObject);
//                }
//                toCombine = true;
//                point.Column += inc;
//            }
//        }

//        //private void addPlayerObjects(int i_Player)
//        //{
//        //    Point point = new Point(0, 1);
//        //    int until = 0;
//        //    int inc;
//        //    SnakePlayer snake = new SnakePlayer(ref m_Board);

//        //    m_SnakePlayers.Add(snake);

//        //    if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Column == eColumnPosition.LeftColumn)// (i_Player <= 2)
//        //    {
//        //        point.Column = 3;//column = 3;
//        //        until = 0;
//        //        inc = -1;
//        //    }
//        //    else
//        //    {
//        //        point.Column = m_BoardOurSize.m_Width - 4;
//        //        until = m_BoardOurSize.m_Width - 1;
//        //        inc = 1;
//        //    }

//        //    if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].m_Position.Row == eRowPosition.LowerRow)
//        //    {
//        //        point.Row = m_BoardOurSize.m_Height - 2;
//        //    }

//        //    int i = 0;
//        //    while (point.Column != until) //(i == 0)//
//        //    {
//        //        i++;
//        //        GameObject gameObject_ = addGameBoardObject_(eScreenObjectType.Player, point, i_Player, i_Player + 2, getBodyPartString(i));
//        //        gameObject_.FadeWhenObjectIsRemoved();

//        //        if (m_GameInformation.ScreenInfoOfAllPlayers[i_Player - 1].Position.Column == eColumnPosition.RightColumn)
//        //        {
//        //            gameObject_.SetImageDirection(0, Direction.Left);
//        //            snake.Direction = Direction.Left;
//        //        }
//        //        else
//        //        {
//        //            snake.Direction = Direction.Right;
//        //        }
//        //        ///////////////////////snake.AddSnakePart(gameObject);
//        //        point.Column += inc;
//        //    }
//        //    m_SnakeLastDirection.Add(snake.Direction);
//        //}

//        private string getBodyPartString(int i_Index)
//        {
//            string bodyPart = eSnakeBodyParts.Head.ToString();

//            if (i_Index == 3)
//            {
//                bodyPart = eSnakeBodyParts.Tail.ToString();
//            }
//            else if (i_Index == 2)
//            {
//                bodyPart = eSnakeBodyParts.Body.ToString();
//            }

//            return bodyPart;
//        }

//        private void addFood()
//        {
//            Point randomPoint = new Point(5, 1);//emptyPositions[m_randomPosition.Next(emptyPositions.Count)];

//            m_food.set(addGameBoardObject_(
//                eScreenObjectType.Object, randomPoint, 1, (int)eBoardObjectSnake.Food,
//                eBoardObjectSnake.Food.ToString()));
//            if (m_Player.ButtonThatPlayerPicked == 1)
//            {
//                //SendServerObjectUpdate(eButton.ButtonA, 5, 1);
//            }
//        }

//        private void updateFoodToNewPoint(Point i_Point)
//        {
//            m_food.PopPoint();
//            m_food.AddPointTop(i_Point);
//            m_gameObjectsToUpdate.Add(m_food);
//            m_Board[i_Point.Column, i_Point.Row] = (int)eBoardObjectSnake.Food;
//        }

//        private void getNewPointForFood()
//        {
//            List<Point> emptyPositions = getEmptyPositions();
//            Point point;

//            if (emptyPositions.Count != 0)
//            {
//                point = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];
//            }
//            else
//            {
//                m_GameStatus = eGameStatus.Tie;
//                point = new Point(-1, -1);
//            }

//            //SendServerObjectUpdate(eButton.ButtonA, point.Column, point.Row);
//        }

//        public List<Point> getEmptyPositions()
//        {
//            List<Point> res = new List<Point>();

//            for (int col = 0; col < m_BoardOurSize.m_Width; col++)
//            {
//                for (int row = 0; row < m_BoardOurSize.m_Height; row++)
//                {
//                    if (m_Board[col, row] == 0)
//                    {
//                        res.Add(new Point(col, row));
//                    }
//                }
//            }

//            return res;
//        }

//        bool checkIfPlayerIsSyncWithChangedPoint(int i_Player, Point i_Point)
//        {
//            return getPlayerCurrentPointPoint(i_Player) == i_Point;
//        }

//        private bool fixDelay(int i_Player, Point i_Point)
//        {
//            int amountOfSkips = 0;
//            bool foundSkip = false;
//            Point futurePoint = getPlayerCurrentPointPoint(i_Player);

//            //while (!foundSkip && amountOfSkips < 9)
//            //{
//            //    if (futurePoint.Move(m_PlayersSnakes[i_Player - 1].m_Direction) == i_Point)
//            //    {
//            //        foundSkip = true;
//            //        for (int i = 0; i <= amountOfSkips; i++)
//            //        {
//            //            moveSnakes();
//            //        }
//            //    }
//            //    else if (amountOfSkips < m_PastVersions[i_Player - 1].Count && i_Point == m_PastVersions[i_Player - 1][amountOfSkips].getSnakeHead())
//            //    {
//            //        for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
//            //        {
//            //            m_PlayersSnakes[i] = m_PastVersions[i][amountOfSkips];
//            //        }
//            //        foundSkip = true;
//            //    }
//            //    amountOfSkips++;
//            //}

//            return foundSkip;
//        }

//        protected override void ChangeDirection(Direction i_Direction, int i_Player, int i_LoopNumber)
//        {
//            if(i_Direction != Direction.Stop)
//            {
//                //if(i_LoopNumber < m_LoopNumber)
//                //{
//                   // m_PlayersSnakes = m_Past[i_LoopNumber];
//                   // m_LoopNumber = i_LoopNumber;
//                //}
//               // else
//                //{
//               //////// //while (i_LoopNumber > m_LoopNumber)
//               ////// {
//                /// //   m_LoopNumber++;
//                //////    moveSnakes();
//                ///////}
                 
//               // }

//                if (canChangeDirection(i_Direction, i_Player))
//                {
//                    m_PlayersSnakes[i_Player - 1].m_Direction = i_Direction;
//                    m_PlayersSnakes[i_Player - 1].m_IsObjectMoving = true;
//                }
//            }
//        }

//        private void addFoor()
//        {
//            int i = 0;
//            Point point;


//            for (int col = 0; col < m_BoardOurSize.m_Width; col++)
//            {
//                for (int row = 0; row < m_BoardOurSize.m_Height; row++)
//                {
//                    if (m_Board[col, row] == 0)
//                    {
//                        GameObject floor = new GameObject();
//                        point = new Point(col, row);
//                        floor.set(addGameBoardObject_(eScreenObjectType.Object, point, 1,
//                            (int)eBoardObjectPacman.Food, eBoardObjectPacman.Food.ToString()));
//                        //m_Food.Add(food);
//                        floor.ImageSource = "floor.png";
//                    }
//                }
//            }
//        }

//        //protected override void ChangeDirection(Direction i_Direction, int i_Player, Point i_PointOfWhenWeChanged)
//        //{
//        //    if (i_Direction != Direction.Stop)
//        //    {
//        //        if (canChangeDirection(i_Direction, i_Player))
//        //        {
//        //            if (checkIfPlayerIsSyncWithChangedPoint(i_Player, i_PointOfWhenWeChanged))
//        //            {
//        //                m_PlayersSnakes[i_Player - 1].m_Direction = i_Direction;
//        //                m_PlayersSnakes[i_Player - 1].m_IsObjectMoving = true;
//        //            //}
//        //            //else
//        //            //{
//        //            //    if (fixDelay(i_Player, i_PointOfWhenWeChanged))
//        //            //    {
//        //            //        m_PlayersSnakes[i_Player - 1].m_Direction = i_Direction;
//        //            //        m_PlayersSnakes[i_Player - 1].m_IsObjectMoving = true;
//        //            //    }
//        //            //    //else
//        //            //    //{
//        //            //    //    throw new Exception("One of the players has connection issues");
//        //            //    //}
//        //            //}
//        //        }
//        //    }


//        //    //if (canChangeDirection(i_Direction, i_Player))
//        //    //{
//        //    //    m_DirectionsBuffer[i_Player - 1].Add(i_Direction);
//        //    //}
//        //}

//        //protected override void ChangeDirection(Direction i_Direction, int i_Player)
//        //{
//        //    if (i_Direction != Direction.Stop)
//        //    {
//        //        if (canChangeDirection(i_Direction, i_Player))
//        //        {
//        //            m_SnakePlayers[i_Player - 1].Direction = i_Direction;
//        //            m_SnakePlayers[i_Player - 1].IsSnakeMoving = true;
//        //        }
//        //    }


//        //    //if (canChangeDirection(i_Direction, i_Player))
//        //    //{
//        //    //    m_DirectionsBuffer[i_Player - 1].Add(i_Direction);
//        //    //}
//        //}

//        private Direction getLastDirection(int i_Player)
//        {

//            return m_PlayersSnakes[i_Player - 1].m_Direction;


//            Direction result;
//            if (m_DirectionsBuffer[i_Player - 1].Count == 0)
//            {
//                result = m_PlayersSnakes[i_Player - 1].m_Direction;
//            }
//            else
//            {
//                result = m_DirectionsBuffer[i_Player].Last();
//            }

//            return result;
//        }

//        private bool canChangeDirection(Direction i_NewDirection, int i_Player)
//        {
//            bool result = true;

//            //if (m_DirectionsBuffer[i_Player - 1].Count == 2)
//            //{
//            //    result = false;
//            //}

//            Direction lastDirection = m_PlayersSnakes[i_Player - 1].m_Direction; //getLastDirection(i_Player);
//            //if(i_NewDirection == lastDirection.OppositeDirection())
//            //{
//            //    result = false;
//            //}
//            //else
//            //{

//            //    result = true;
//            //}

//            return !i_NewDirection.IsOppositeDirection(lastDirection);
//        }


//        private void moveSnakes()
//        {
//            int player = 1;
//            Direction currentDirection;
//            foreach (var snake in m_PlayersSnakes)
//            {
//                if (m_Hearts.m_AmountOfLivesPlayerHas[player - 1] > 0 && snake.m_IsObjectMoving)
//                {
//                    currentDirection = snake.m_Direction;
//                    Point newHeadPoint = snake.GetOneMoveAhead();
//                    int hit = snake.whatSnakeWillHit(newHeadPoint, isPointOnBoard(newHeadPoint));

//                    if (false)//(hit == (int)eBoardObjectSnake.OutOfBounds || hit > 2)
//                    {
//                        //snake.Move(newHeadPoint, m_SnakeLastDirection[player - 1], currentDirection);
//                        //PlayerGotHit(player);

//                    }
//                    else if (hit == (int)eBoardObjectSnake.Empty)//Normal move
//                    {
//                        //snake.Eat(addGameBoardObject_(eScreenObjectType.Player, newHeadPoint, player, player + 2,
//                        //    eSnakeBodyParts.Head.ToString()), m_SnakeLastDirection[player - 1], currentDirection);
//                        snake.Move(newHeadPoint, m_SnakeLastDirection[player - 1], currentDirection);
//                    }
//                    else if (hit == (int)eBoardObjectSnake.Food)//Eats food
//                    {
//                        if (flag)
//                        {
//                            int a = 6;
//                        }
//                        m_Board[newHeadPoint.Column, newHeadPoint.Row] = player + 2;
//                        // snake.Move(newHeadPoint, m_SnakeLastDirection[player - 1], currentDirection);
//                        snake.Eat(addGameBoardObject_(eScreenObjectType.Player, newHeadPoint, player, player + 2,
//                            eSnakeBodyParts.Head.ToString()), m_SnakeLastDirection[player - 1], currentDirection);
//                        m_Board[newHeadPoint.Column, newHeadPoint.Row] = player + 2;
//                        if (m_Player.ButtonThatPlayerPicked == 1)
//                        {
//                            getNewPointForFood();
//                        }
//                        //OnHideGameObjects(m_food.ID);
//                        flag = true;
//                        m_Board[newHeadPoint.Column, newHeadPoint.Row] = player + 2;
//                        // updateFoodToNewPoint(new Point(0, 0));
//                    }
                    
//                    m_gameObjectsToUpdate.Add(snake);
//                    m_SnakeLastDirection[player - 1] = currentDirection;
//                    snake.m_Direction = currentDirection;
//                }
//                player++;
//            }
//        }

//        private void PlayerGotHit(int i_Player)
//        {
//            //m_Hearts.setPlayerLifeAndGetGameStatus(i_Player);
//            OnDeleteGameObject(m_PlayersSnakes[i_Player - 1]);
//            PlayerLostALife(i_Player);//general ui update
//        }

//    }
//}

////if (m_DirectionsBuffer[player - 1].Count > 0)
////{
////    m_PlayersSnakes[player - 1].m_Direction = m_DirectionsBuffer[player - 1].First();
////    m_DirectionsBuffer[player - 1].RemoveAt(0);
////}