using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;
namespace Objects
{
    [Serializable]
    public class Direction
    {
        public int ColumnOffset { get; }
        public int RowOffset { get; }

        public readonly static Direction Left = new Direction(-1, 0);
        public readonly static Direction Right = new Direction(1, 0);
        public readonly static Direction Up = new Direction(0, -1);
        public readonly static Direction Down = new Direction(0, 1);
        public readonly static Direction Stop = new Direction(0, 0);
        private Direction(int i_Column, int i_Row)
        {
           ColumnOffset = i_Column;
           RowOffset = i_Row;
        }

        public List<Direction> GetAllDirections()
        {
            List<Direction> allDirections = new List<Direction>();
            allDirections.Add(Direction.Down);
            allDirections.Add(Direction.Right);
            allDirections.Add(Direction.Left);
            allDirections.Add(Direction.Up);

            return allDirections;
        }

        public static Direction getDirection(string i_Button)
        {

            if (i_Button == eButton.Up.ToString())
            {
                return Direction.Up;
            }
            else if(i_Button == eButton.Stop.ToString())
            {
                return Direction.Stop;
            }
            else if (i_Button == eButton.Down.ToString())
            {
                return Direction.Down;
            }
            else if (i_Button == eButton.Right.ToString())
            {
                return Direction.Right;
            }
            else
            {   
                return Direction.Left;
            }
        }

        public static Direction getDirection(int i_Button)
        {
            if(i_Button == (int)eButton.Stop)
            {
                return Direction.Stop;
            }
            else if (i_Button == (int)eButton.Up)
            {
                return Direction.Up;
            }
            else if (i_Button == (int)eButton.Down)
            {
                return Direction.Down;
            }
            else if (i_Button == (int)eButton.Right)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }

        public Direction OppositeDirection()
        {
            return new Direction(-ColumnOffset, -RowOffset);
        }
        public bool IsOppositeDirection(Direction i_Direction)
        {
            bool result = false;

            if(ColumnOffset == -i_Direction.ColumnOffset && RowOffset == -i_Direction.RowOffset)
            {
                result = true;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction && ColumnOffset == direction.ColumnOffset
                                              && RowOffset == direction.RowOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ColumnOffset, RowOffset);
        }
    }
}
