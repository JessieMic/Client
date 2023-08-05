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
        public SizeDTO ScreenSizeInPixels = new SizeDTO();
        public Position m_Position = new Position();

        public ScreenDimension(int i_ScreenWidth,int i_ScreenHeight, Position i_Position)
        {
            ScreenSizeInPixels.Width = i_ScreenWidth;
            ScreenSizeInPixels.Height = i_ScreenHeight;
            m_Position = i_Position;
        }

        public Position Position
        {
            get
            {
                return m_Position;
            }
        }

        public SizeDTO SizeInPixelsDto
        {
            get { return ScreenSizeInPixels; }
            set
            {
                ScreenSizeInPixels = value;
            }
        }

    }
}
