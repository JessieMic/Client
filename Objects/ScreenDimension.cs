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

        public ScreenDimension(SizeDTO i_OurSize, Position i_Position)
        {
            m_OurSize = i_OurSize;
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
                //m_Size.m_Height-= 115;
            }
        }

    }
}
