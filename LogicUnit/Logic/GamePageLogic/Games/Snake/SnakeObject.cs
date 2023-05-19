using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Snake
{
    public class SnakeObject : GameObject
    {
        private int[,] m_Board;

        public SnakeObject(ref int[,] i_Board)
        {
            m_Board = i_Board;
        }

        public Point getSnakeHead()
        {
            return m_PointsOnGrid.First();
        }

        public Point getSnakeTail()
        {
            return m_PointsOnGrid.Last();
        }

        public void addHead(Point i_Point)
        {
            AddPointTop(i_Point);
            //m_Board[i_Point.m_Column, i_Point.m_Row] = i_Player + 2;
        }

        public void removeTail()
        {
            Point tail = getSnakeTail();
            m_Board[tail.m_Column, tail.m_Row] = 0;
            PopPoint();
        }

        public Point GetOneMoveAhead()
        {
            Point newHeadPoint = getSnakeHead().Move(m_Direction);

            return newHeadPoint;
        }

        public int whatSnakeWillHit(Point i_Point,bool i_IsPointOnTheBoard)//new snake head
        {
            int res;
            Point newHeadPoint = getSnakeHead().Move(m_Direction);

            if (!i_IsPointOnTheBoard)
            {
                res = (int)eBoardObjectSnake.OutOfBounds;
            }
            else if (i_Point == getSnakeTail())
            {
                res = (int)eBoardObjectSnake.Empty;
            }
            else
            {
                res = m_Board[i_Point.m_Column, i_Point.m_Row];
            }

            return res;
        }

        public void deleteSnake()
        {
            foreach (var point in m_PointsOnGrid)
            {
                m_Board[point.m_Column, point.m_Row] = (int)eBoardObjectSnake.Empty;
            }
        }
    }
}

