using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Objects
{
    public struct ScreenDimension
    {
        public SizeDTO m_OurSize = new SizeDTO();
        public Position m_Position = new Position();

        public ScreenDimension(int i_ScreenWidth,int i_ScreenHeight, Position i_Position)
        {
            m_OurSize.Width = i_ScreenWidth;
            m_OurSize.Height = i_ScreenHeight;
            m_Position = i_Position;
        }

        public Position Position
        {
            get
            {
                return m_Position;
            }
        }

        public SizeDTO SizeDTO
        {
            get { return m_OurSize; }
            set
            {
                m_OurSize = value;
            }
        }

    }
}
