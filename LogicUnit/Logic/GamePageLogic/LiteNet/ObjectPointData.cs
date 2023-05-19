using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic.LiteNet
{
    public class ObjectPointData
    {
        public void set(int i_Column, int i_Row, int i_Object)
        {
            m_Column = i_Column;
            m_Row = i_Row;
            m_Object = i_Object;
        }

        private int m_Column;
        private int m_Row;
        private int m_Object;
    }
}
