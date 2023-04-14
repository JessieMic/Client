using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public struct ScreenDimension
    {
        private Size m_Size = new Size();
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

        public Size Size
        {
            get { return m_Size; }
            set
            {
                m_Size = value;
                //m_Size.m_Height-= 115;
            }
        }

    }
}
