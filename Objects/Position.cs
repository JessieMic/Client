using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace Objects
{
    public class Position
    {
        private eRowPosition m_Row;
        private eColumnPosition m_Column;
        public Position() { }
        public Position(int i_AmountOfPlayers, int i_PlacementNumber)
        {
            m_Column = getColumnPosition(i_PlacementNumber, i_AmountOfPlayers);
            m_Row = getRowPosition(i_PlacementNumber, i_AmountOfPlayers);
        }

        public void SetPosition(int i_AmountOfPlayers, int i_PlacementNumber)
        {
            m_Column = getColumnPosition(i_PlacementNumber, i_AmountOfPlayers);
            m_Row = getRowPosition(i_PlacementNumber, i_AmountOfPlayers);
        }

        public eRowPosition Row
        {
            get
            {
                return m_Row;
            }
            set
            {
                m_Row = value;
            }
        }

        public eColumnPosition Column
        {
            get
            {
                return m_Column;
            }
            set
            {
                m_Column = value;
            }
        }

        private eColumnPosition getColumnPosition(int i_PlacementNumber, int i_AmountOfPlayers)
        {
            eColumnPosition returnValue;

            if (i_PlacementNumber == 1 || i_PlacementNumber == 3 || (i_PlacementNumber == 2 && i_AmountOfPlayers == 2))
            {
                returnValue = eColumnPosition.LeftColumn;
            }
            else
            {
                returnValue = eColumnPosition.RightColumn;
            }

            return returnValue;
        }

        private eRowPosition getRowPosition(int i_PlacementNumber, int i_AmountOfPlayers)
        {
            eRowPosition returnValue;

            if (i_PlacementNumber == 1 || (i_PlacementNumber == 2 && i_AmountOfPlayers > 2))
            {
                returnValue = eRowPosition.UpperRow;
            }
            else
            {
                returnValue = eRowPosition.LowerRow;
            }

            return returnValue;
        }
    }
}
