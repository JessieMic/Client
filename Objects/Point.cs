using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public struct Point
    {
        public double Column { get; set; }
        public double Row { get; set; }

        public Point()
        {
        }

        public Point(double i_Column, double i_Row)
        {
            Column = i_Column;
            Row = i_Row;
        }

        public Point SetAndGetPoint(double i_Column, double i_Row)
        {
            Column = i_Column;
            Row = i_Row;

            return this;
        }

        public Point(Point p)
        {
            new Point(p.Column, p.Row);
        }

        public Point Move(Direction i_Direction)
        {
            return new Point(Column + i_Direction.ColumnOffset, Row+i_Direction.RowOffset);
        }

        public static bool operator ==(Point i_P1, Point i_P2)
        {
            return i_P1.Column == i_P2.Column && i_P1.Row == i_P2.Row;
        }

        public static bool operator !=(Point i_P1, Point i_P2)
        {
            return i_P1.Column != i_P2.Column || i_P1.Row != i_P2.Row;
        }

    }
}
