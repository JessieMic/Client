using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public struct ScreenDimension
    {
        public Size m_Size = new Size();
        public Position m_Position = new Position();

        public ScreenDimension(Size i_Size,Position i_Position)
        {
            m_Size = i_Size;
            m_Position = i_Position;
        }

        public Position Position
        {
            get
            {
                return m_Position;
            }
        }
    }
}
