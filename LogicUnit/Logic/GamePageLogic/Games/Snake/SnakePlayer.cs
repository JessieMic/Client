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
    public class SnakePlayer
    {
        private int[,] m_Board;
        private List<Direction> m_DirectionsForTail = new List<Direction>();
        private List<GameObject_> m_SnakeParts;
        private int m_ObjectNumber;
        public bool IsSnakeMoving = false;
        public Direction Direction { get; set; } = Direction.Stop;
        public SnakePlayer(ref int[,] i_Board)
        {
            m_Board = i_Board;
        }

        public void AddSnakePart(GameObject_ i_SnakePart)
        {
            m_SnakeParts.Add(i_SnakePart);
        }

        public void Eat(GameObject_ i, Direction i_LastDirection, Direction i_CurrentDirection)
        {
            addANewSnakeBodyPart(i);
            m_SnakeParts[0].SetImageDirection(i_CurrentDirection);

            if (i_LastDirection != i_CurrentDirection)
            {
                turnBody(i_LastDirection, i_CurrentDirection);
                m_SnakeParts[1].ImageSource = "snakeplayer" + m_ObjectNumber.ToString() + "turn.png";
                m_DirectionsForTail.Add(i_CurrentDirection);
            }
            else
            {
                m_SnakeParts[1].ImageSource = "snakeplayer" + m_ObjectNumber.ToString() + "body.png";
                m_SnakeParts[1].SetImageDirection(i_CurrentDirection);
            }
        }

        private void addANewSnakeBodyPart(GameObject_ i_NewSnakePart)
        {
            m_SnakeParts.Insert(0, i_NewSnakePart);
        }

        public void Move(Point i_NewPoint, Direction i_LastDirection, Direction i_CurrentDirection)
        {
            removeTail();
            ShiftPoints();
            addHead(i_NewPoint);
            moveTail();
            moveBody(i_CurrentDirection);

            if (i_LastDirection != i_CurrentDirection)
            {
                m_SnakeParts[0].SetImageDirection(i_CurrentDirection);//Set head to a new direction
                turnBody(i_LastDirection, i_CurrentDirection);
                m_SnakeParts[1].ImageSource = "snakeplayer" + m_ObjectNumber.ToString() + "turn.png";
                m_DirectionsForTail.Add(i_CurrentDirection);
            }
        }

        private void ShiftPoints()
        {
            for(int i = m_SnakeParts.Count-1; i > 0; i--)
            {
                m_SnakeParts[i].PointOnGrid = m_SnakeParts[i - 1].PointOnGrid;
                m_SnakeParts[i].PointOnScreen = m_SnakeParts[i - 1].PointOnScreen;
            }
        }

        private void turnBody(Direction i_LastDirection, Direction i_CurrentDirection)
        {
            if ((i_LastDirection == Direction.Right && i_CurrentDirection == Direction.Up)
               || (i_LastDirection == Direction.Down && i_CurrentDirection == Direction.Left))
            {
                m_SnakeParts[1].Rotatation = 180;
            }
            else if ((i_LastDirection == Direction.Right && i_CurrentDirection == Direction.Down)
                    || (i_LastDirection == Direction.Up && i_CurrentDirection == Direction.Left))
            {
                m_SnakeParts[1].Rotatation = 90;
            }
            else if ((i_LastDirection == Direction.Down && i_CurrentDirection == Direction.Right)
                     || (i_LastDirection == Direction.Left && i_CurrentDirection == Direction.Up))
            {
                m_SnakeParts[1].Rotatation = 270;
            }
            else
            {
                m_SnakeParts[1].Rotatation = 0;
            }
        }

        private void moveBody(Direction i_CurrentDirection)
        {
            for (int i = m_SnakeParts.Count - 2; i > 1; i--)
            {
                m_SnakeParts[i].ImageSource = m_SnakeParts[i - 1].ImageSource;
                copyNextImageDirection(i);
            }

            m_SnakeParts[1].ImageSource = "snakeplayer" + m_ObjectNumber.ToString() + "body.png";
            m_SnakeParts[1].SetImageDirection(i_CurrentDirection);
        }

        private void moveTail()
        {
            int numberOfSnakeParts = m_SnakeParts.Count;

            if (m_SnakeParts[numberOfSnakeParts].ImageSource == "snakeplayer" + m_ObjectNumber.ToString() + "body.png")
            {
                copyNextImageDirection(numberOfSnakeParts - 1);
            }
            else
            {
                m_SnakeParts[numberOfSnakeParts-1].SetImageDirection(m_DirectionsForTail[0]);
                m_DirectionsForTail.RemoveAt(0);
                m_SnakeParts[numberOfSnakeParts - 1].ImageSource = "snakeplayer" + m_ObjectNumber.ToString() + "tail.png";
            }
        }

        private void copyNextImageDirection(int i_Index)
        {
            m_SnakeParts[i_Index].Rotatation = m_SnakeParts[i_Index - 1].Rotatation;
        }

        private Direction getDirectionOfTurnPart()
        {
            Direction direction = Direction.Down;

            if (m_SnakeParts[m_SnakeParts.Count - 2].Rotatation != 0 ||
                m_SnakeParts[m_SnakeParts.Count - 2].ScaleY != 1)
            {
                direction = Direction.Up;
            }

            return direction;
        }

        private bool isPartTurnBodyPart()
        {
            bool isPartTurnBodyPart = false;

            if (m_SnakeParts[m_SnakeParts.Count - 2].ImageSource[12] == 't')
            {
                isPartTurnBodyPart = true;
            }

            return isPartTurnBodyPart;
        }

        public Point getSnakeHead()
        {
            return m_SnakeParts.First().PointOnGrid;//PointsOnGrid.First());
        }

       

        public void addHead(Point i_Point)
        {
            m_SnakeParts[0].PointOnGrid = i_Point;
            m_SnakeParts[0].PointOnScreen = i_Point;
            m_Board[i_Point.Column, i_Point.Row] = m_ObjectNumber + 2;
        }

        public void removeTail()
        {
            Point tail = m_SnakeParts.Last().PointOnGrid;
            if (m_Board[tail.Column, tail.Row] == m_ObjectNumber)
            {
                m_Board[tail.Column, tail.Row] = 0;
            }
        }

        public Point GetOneMoveAhead()
        {
            Point newHeadPoint = getSnakeHead().Move(Direction);

            return newHeadPoint;
        }

        public int whatSnakeWillHit(Point i_Point, bool i_IsPointOnTheBoard)//new snake head
        {
            int res;
            Point newHeadPoint = GetOneMoveAhead();

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
                res = m_Board[i_Point.Column, i_Point.Row];
            }

            return res;
        }

        private Point getSnakeTail()
        {
            return m_SnakeParts.Last().PointOnGrid;
        }

        public void DeleteSnake()
        {
            foreach (var part in m_SnakeParts)
            {
                Point point = part.PointOnGrid;
                m_Board[point.Column, point.Row] = (int)eBoardObjectSnake.Empty;
            }
        }
    }
}
