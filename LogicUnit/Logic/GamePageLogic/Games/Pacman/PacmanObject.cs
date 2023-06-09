using Objects;
using Objects.Enums.BoardEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class PacmanObject : GameObject
    {
        private int[,] m_Board;

        public PacmanObject(ref int[,] i_Board)
        {
            m_Board = i_Board;
        }

        public Point GetOneMoveAhead()
        {
            Point newPoint = getPacmanPoint().Move(m_Direction);

            return newPoint;
        }

        private Point getPacmanPoint()
        {
            return m_PointsOnGrid.First();
        }

        public int WhatPacmanWillHit(Point i_Point, bool i_IsPointInsideBoard)
        {
            int res;
            Point newHeadPoint = getPacmanPoint().Move(m_Direction);

            if (!i_IsPointInsideBoard)
            {
                res = (int)eBoardObjectPacman.OutOfBounds;
            }
            else
            {
                res = m_Board[i_Point.m_Column, i_Point.m_Row];
            }

            return res;
        }

        public void Move(Point i_NewPoint)
        {
            AddPointTop(i_NewPoint);

            Point currPoint = getPacmanPoint();

            m_Board[currPoint.m_Column, currPoint.m_Row] = 0;
            PopPoint();
        }
    }
}
