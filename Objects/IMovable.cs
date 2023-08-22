using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Objects.Point;

namespace Objects
{
    public interface IMovable
    {
        public Direction Direction { get; set; }// = Direction.Stop;
        public Direction RequestedDirection { get; set; }// = Direction.Stop;
        public bool IsObjectMoving { get; set; }// = true;
        public bool WantToTurn { get; set; } //= false;
        public Point PointOnScreen { get; set; }
        public int[,] Board { get; set; }
        public int ObjectNumber { get; set; }
        public void SetImageDirection(Direction i_Direction);
        public Point GetScreenPoint(Point i_Point, bool i_IsGrided);
        public Point GetPointOnGrid();
        public void OnUpdatePosition(Point i_Point);
    }
}
