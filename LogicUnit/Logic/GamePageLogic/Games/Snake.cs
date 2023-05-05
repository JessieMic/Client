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

namespace LogicUnit
{
    public class Snake : Game
    {
        public Snake()
        {
            m_GameName = "Snake";
            m_AmountOfLivesPlayersGetAtStart = 1;
            m_TypeOfGameButtons = eTypeOfGameButtons.MovementButtonsForAllDirections;
        }

        public override async void RunGame()
        {
            await gameLoop();
        }

        protected override async Task gameLoop()
        {
            while (m_GameStatus == eGameStatus.Running)
            {
                await Task.Delay(700);

                m_ScreenObjectUpdate = new List<ScreenObjectUpdate>();
                m_ScreenObjectList = new List<ScreenObjectAdd>();
                moveSnakes();
                OnUpdateScreenObject(m_ScreenObjectUpdate);
                if (m_ScreenObjectList.Count != 0)
                {
                    OnAddScreenObjects(m_ScreenObjectList);
                }
            }
        }

        protected override void ChangeGameObject(int i_ObjectNumber, Direction i_Direction, Point i_Point)
        {
            updateFoodToNewPoint(i_Point);
        }

        protected override void AddGameObjects()
        {
            for(int player = 1; player <= m_GameInformation.AmountOfPlayers; player++)
            {
                addPlayerObjects(player);
            }
            addFood();
        }

        private void addPlayerObjects(int i_Player)
        {
            Point point = new Point(0,1);
            int until = 0;
            int inc;
            bool toInitialize = true;

            if(m_GameInformation.ScreenInfoOfAllPlayers[i_Player-1].m_Position.Column == eColumnPosition.LeftColumn)// (i_Player <= 2)
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
                addGameBoardObject(eScreenObjectType.Player,point,i_Player,i_Player+2,"body", toInitialize);
                toInitialize = false;
                point.m_Column += inc;
            }
        }

        private void addFood()
        {
            List<Point> emptyPositions = getEmptyPositions();
            Point randomPoint = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];

            addGameBoardObject(
                eScreenObjectType.Object, randomPoint, 1, (int)eBoardObjectSnake.Food,
                eBoardObjectSnake.Food.ToString(), true);
        }
        private void updateFoodToNewPoint(Point i_Point)
        {
            m_gameObjects[0].PopPoint();
            m_gameObjects[0].AddPointTop(i_Point);
            m_ScreenObjectUpdate.Add(m_gameObjects[0].GetObjectUpdate());
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

            notifyGameObjectUpdate(eScreenObjectType.Object,1,null,point);
        }

        public List<Point> getEmptyPositions()
        {
            List<Point> res = new List<Point>();

            for(int col = 0; col < m_BoardSize.m_Width; col++)
            {
                for(int row = 0 ;row < m_BoardSize.m_Height; row++)
                {
                    if(m_Board[col, row] == 0)
                    {
                        res.Add(new Point(col,row));
                    }
                }
            }

            return res;
        }

        private Point getSnakeHead(int i_Player)
        {
           return  m_PlayerGameObjects[i_Player - 1].m_PointsOnGrid.First();
        }

        private Point getSnakeTail(int i_Player)
        {
            return m_PlayerGameObjects[i_Player - 1].m_PointsOnGrid.Last();
        }

        private void addHead(Point i_Point, int i_Player)
        {
            m_PlayerGameObjects[i_Player-1].AddPointTop(i_Point);
            m_Board[i_Point.m_Column, i_Point.m_Row] = i_Player + 2;
        }

        private void removeTail(int i_Player)
        {
            Point tail = getSnakeTail(i_Player);
            m_Board[tail.m_Column, tail.m_Row] = 0;
            m_PlayerGameObjects[i_Player - 1].PopPoint();
        }

        //protected override void ChangeDirection(Direction i_Direction, int i_Player)
        //{
        //    if(canChangeDirection(i_Direction, i_Player))
        //    {
        //        m_DirectionsBuffer[i_Player - 1].Add(i_Direction);
        //    }
        //}

        private Direction getLastDirection(int i_Player)
        {
            Direction result;
            if(m_DirectionsBuffer[i_Player - 1].Count == 0)
            {
                result = m_PlayerGameObjects[i_Player - 1].m_Direction;
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

            if(m_DirectionsBuffer[i_Player - 1].Count == 2)
            {
                result = false;
            }

            Direction lastDirection = getLastDirection(i_Player);

            return i_NewDirection != lastDirection && i_NewDirection != lastDirection.OppositeDirection();
        }

        private int whatSnakeWillHit(Point i_Point,int i_Player)//new snake head
        {
            int res;

            if (!isPointOnBoard(i_Point))
            {
                res = (int)eBoardObjectSnake.OutOfBounds;
            }
            else if(i_Point == getSnakeTail(i_Player))
            {
                res = (int)eBoardObjectSnake.Empty;
            }
            else
            {
                res = m_Board[i_Point.m_Column, i_Point.m_Row];
            }

            return res;
        }

        private void moveSnakes()
        {
            for(int player = 1; player <= m_GameInformation.AmountOfPlayers; player++)
            {
                if(m_AmountOfLivesPlayerHas[player - 1] > 0 && m_PlayerGameObjects[player - 1].m_Direction != Direction.Stop)
                {
                    if (m_DirectionsBuffer[player - 1].Count > 0)
                    {
                        m_PlayerGameObjects[player - 1].m_Direction = m_DirectionsBuffer[player - 1].First();
                        m_DirectionsBuffer[player - 1].RemoveAt(0);
                    }

                    Point newHeadPoint = getSnakeHead(player).Move(m_PlayerGameObjects[player - 1].m_Direction);
                    int hit = whatSnakeWillHit(newHeadPoint, player);

                    if (hit == (int)eBoardObjectSnake.OutOfBounds || hit >= 3)
                    {
                        PlayerGotHit(player);
                    }

                    else if (hit == (int)eBoardObjectSnake.Empty)//Normal move
                    {   
                        removeTail(player);
                        addHead(newHeadPoint, player);
                    }
                    else if (hit == (int)eBoardObjectSnake.Food)//Eats food
                    {   
                        addHead(newHeadPoint, player);


                        addGameBoardObject(eScreenObjectType.Player,newHeadPoint,player,player,"body",false);
                        //score ++

                        if(m_Player.ButtonThatPlayerPicked == 1)
                        {
                            getNewPointForFood();
                        }
                    }
                    m_ScreenObjectUpdate.Add(m_PlayerGameObjects[player - 1].GetObjectUpdate());
                }
            }
        }

        private void PlayerGotHit(int i_Player)
        {
            OnDeleteGameObject(i_Player);
            foreach (var point in m_PlayerGameObjects[i_Player-1].m_PointsOnGrid)
            {
                m_Board[point.m_Column, point.m_Row] = (int)eBoardObjectSnake.Empty;
            }
            PlayerLostALife(i_Player);//general ui update
        }
    }
}




//    for (int col = 3; col > 0; col--)
//{
//    ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, new Point(col,1), m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1);
//    m_PlayerGameObjects[0].SetObject(ref obj);
//    m_ScreenObjectList.Add(obj);
//    m_Board[col, 1] = (int)eBoardObjectSnake.Snake1;
//}

//m_PlayerGameObjects.Add(new GameObject(eScreenObjectType.PlayerObject, 2, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd));
//for (int col = 3; col > 0; col--)
//{
//    ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, new Point(col, m_BoardSize.m_Height-2), m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 2);
//    m_PlayerGameObjects[1].SetObject(ref obj);
//    m_ScreenObjectList.Add(obj);
//    m_Board[col, m_BoardSize.m_Height - 2] = (int)eBoardObjectSnake.Snake2;
//}

//if (m_GameInformation.AmountOfPlayers >= 3)
//{
//    m_PlayerGameObjects.Add(new GameObject(eScreenObjectType.PlayerObject, 3, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd));
//    for (int col = m_BoardSize.m_Width - 4; col < m_BoardSize.m_Width - 1; col++)
//    {
//        ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, new Point(col, 1), m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 3);
//        m_PlayerGameObjects[2].SetObject(ref obj);
//        m_ScreenObjectList.Add(obj);
//        m_Board[col, 1] = (int)eBoardObjectSnake.Snake3;
//    }
//}

//if (m_GameInformation.AmountOfPlayers == 4)
//{
//    m_PlayerGameObjects.Add(new GameObject(eScreenObjectType.PlayerObject, 4, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd));
//    for (int col = m_BoardSize.m_Width - 4; col < m_BoardSize.m_Width - 1; col++)
//    {
//        ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, new Point(col, m_BoardSize.m_Height - 2), m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 4);
//        m_PlayerGameObjects[3].SetObject(ref obj);
//        m_ScreenObjectList.Add(obj);
//        m_Board[col, m_BoardSize.m_Height - 2] = (int)eBoardObjectSnake.Snake4;
//    }
//}