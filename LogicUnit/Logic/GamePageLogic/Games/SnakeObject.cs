using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games
{
    public class SnakeObject : GameObject
    {
        //public moveSnake()
        //{

        //}

        public Point getSnakeHead(int i_Player)
        {
            return m_PointsOnGrid.First();
        }

        //private Point getSnakeTail(int i_Player)
        //{
        //    return m_PlayerGameObjects[i_Player - 1].m_PointsOnGrid.Last();
        //}

        //private void addHead(Point i_Point, int i_Player)
        //{
        //    m_PlayerGameObjects[i_Player - 1].AddPointTop(i_Point);
        //    m_Board[i_Point.m_Column, i_Point.m_Row] = i_Player + 2;
        //}

        //private void removeTail(int i_Player)
        //{
        //    Point tail = getSnakeTail(i_Player);
        //    m_Board[tail.m_Column, tail.m_Row] = 0;
        //    m_PlayerGameObjects[i_Player - 1].PopPoint();
        //}
    }
}

