using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Direction(int i_Column, int i_Row)
        {
           m_ColumnOffset = i_Column;
           m_RowOffset = i_Row;
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
