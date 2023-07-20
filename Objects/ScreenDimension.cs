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
        private SizeDTO m_OurSize = new SizeDTO();
        public Position m_Position = new Position();

        public ScreenDimension(int i_ScreenWidth,int i_ScreenHeight, Position i_Position)
        {
            m_OurSize.m_Width = i_ScreenWidth;
            m_OurSize.m_Height = i_ScreenHeight;
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
