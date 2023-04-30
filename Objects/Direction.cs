using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;
namespace Objects
{
    public class Direction
    {
        public int m_ColumnOffset { get; }
        public int m_RowOffset { get; }

        public readonly static Direction Left = new Direction(-1, 0);
        public readonly static Direction Right = new Direction(1, 0);
        public readonly static Direction Up = new Direction(0, -1);
        public readonly static Direction Down = new Direction(0, 1);
        public readonly static Direction Stop = new Direction(0, 0);
        private Direction(int i_Column, int i_Row)
        {
           m_ColumnOffset = i_Column;
           m_RowOffset = i_Row;
        }

        public static Direction getDirection(string i_Button)
        {
            if (i_Button == eButton.Up.ToString())
            {
                return Direction.Up;
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

        public Direction OppositeDirection()
        {
            return new Direction(-m_ColumnOffset, -m_RowOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction && m_ColumnOffset == direction.m_ColumnOffset
                                              && m_RowOffset == direction.m_RowOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_ColumnOffset, m_RowOffset);
        }
    }
}
