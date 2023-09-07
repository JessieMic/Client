using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public struct ScreenDimension
    {
        public SizeD ScreenSizeInPixels = new SizeD();
        public Position m_Position = new Position();
        public double Density { get; set; }

        public ScreenDimension(int i_ScreenWidth,int i_ScreenHeight, Position i_Position, double i_Density)
        {
            ScreenSizeInPixels.Width = i_ScreenWidth;
            ScreenSizeInPixels.Height = i_ScreenHeight;
            m_Position = i_Position;
            Density = i_Density;
        }

        public Position Position
        {
            get
            {
                return m_Position;
            }
        }

        public SizeD SizeInPixelsD
        {
            get { return ScreenSizeInPixels; }
            set
            {
                ScreenSizeInPixels = value;
            }
        }

    }
}
