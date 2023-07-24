using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public struct Point
    {
        public int m_Column;
        public int m_Row;

        public Point()
        {
        }

        public Point(int i_Column, int i_Row)
        {
            m_Column = i_Column;
            m_Row = i_Row;
        }

        public Point SetAndGetPoint(int i_Column, int i_Row)
        {
            m_Column = i_Column;
            m_Row = i_Row;

            return this;
        }

        public Point Move(Direction i_Direction)
        {
            return new Point(m_Column + i_Direction.m_ColumnOffset, m_Row+i_Direction.m_RowOffset);
        }

        public static bool operator ==(Point i_P1, Point i_P2)
        {
            return i_P1.m_Column == i_P2.m_Column && i_P1.m_Row == i_P2.m_Row;
        }

        public static bool operator !=(Point i_P1, Point i_P2)
        {
            return i_P1.m_Column != i_P2.m_Column || i_P1.m_Row != i_P2.m_Row;
        }

    }
}
