using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public struct Dimentions
    {
        public double m_Width;
        public double m_Height;

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
