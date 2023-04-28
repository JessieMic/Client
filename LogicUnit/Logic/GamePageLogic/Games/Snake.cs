using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic;
using Objects;
using Objects.Enums;
using Objects.Enums.Snake;
using Point = Objects.Point;
namespace LogicUnit
{
    public class Snake : Game
    {
        public Snake()
        {
            
        }

        public override void RunGame()
        {

        }

        protected override void setGameBoardAndGameObjects()
        {

        }

        protected override void AddGameObjects()
        {
            addPlayerObjects();
            addFood();
        }

        private void addPlayerObjects()
        {
            m_PlayerGameObjects.Add(new GameObject(eScreenObjectType.PlayerObject,1,m_ScreenMapping.m_GameBoardGridSize,m_ScreenMapping.m_ValueToAdd));
            for (int col = 3; col < 7; col++)
            {
                ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, new Point(col,1), m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1);
                m_PlayerGameObjects[0].SetObject(ref obj);
                m_ScreenObjectList.Add(obj);
                m_Board[col, 1] = (int)eBoardObject.Snake1;
            }
        }

        private void addFood()
        {
            List<Point> emptyPositions = getEmptyPositions();
            Point randomPoint = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];

            m_gameObjects.Add(new GameObject(eScreenObjectType.GameObject, 1, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd));
            ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.GameObject, null, randomPoint, m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1);
            m_gameObjects[0].SetObject(ref obj);
            m_ScreenObjectList.Add(obj);
            m_Board[randomPoint.m_Column, randomPoint.m_Row] = (int)eBoardObject.Food;
        }

        private void updateFoodToNewPoint()
        {///////////////////////////////////////delete from boardddd////////////////////////////////////////
            List<Point> emptyPositions = getEmptyPositions();
            if(emptyPositions.Count != 0)
            {
                Point randomPoint = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];
                List<ScreenObjectUpdate> update = new List<ScreenObjectUpdate>();
                m_gameObjects[0].PopPoint();
                m_gameObjects[0].AddPointTop(randomPoint);
                update.Add(m_gameObjects[0].GetObjectUpdate());
                OnUpdateScreenObject(update);
            }
        }

        private void initializeGame()
        {
            m_TypeOfGameButtons = eTypeOfGameButtons.MovementButtonsForAllDirections;
            m_AmountOfLivesTheClientHas = 3;
        }

        private List<Point> getEmptyPositions()
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

        private void ChangeDirection(Direction i_Direction, int i_Player)
        {
            ///
        }

        private int whatSnakeWillHit(Point i_Point,int i_Player)//new snake head
        {
            int res;

            if (!isPointOnBoard(i_Point))
            {
                res = (int)eBoardObject.OutOfBounds;
            }
            else if(i_Point == getSnakeTail(i_Player))
            {
                res = (int)eBoardObject.Empty;
            }
            else
            {
                res = m_Board[i_Point.m_Column, i_Point.m_Row];
            }

            return res;
        }

        private void moveSnake(int i_Player)
        {
            Point newHeadPoint = getSnakeHead(i_Player).Move(m_PlayerGameObjects[i_Player - 1].m_Direction);
            int hit = whatSnakeWillHit(newHeadPoint,i_Player);

            if(hit == (int)eBoardObject.OutOfBounds || hit >= 3)
            {
                m_GameStatus = eGameStatus.Ended;
            }
            else if (hit == (int)eBoardObject.Empty)
            {
                removeTail(i_Player);    
                addHead(newHeadPoint,i_Player);
            }
            else if(hit == (int)eBoardObject.Food)
            {
                addHead(newHeadPoint,i_Player);
                //score ++
                //update food pos/////////////////////////////
            }
        }

    }
}
