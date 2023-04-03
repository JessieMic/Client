using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public struct ScreenDimension
    {
        private double m_Width;
        private double m_Height;
        public Position Position;

        public double Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public double Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
    }
}
