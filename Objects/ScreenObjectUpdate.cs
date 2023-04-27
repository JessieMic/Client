using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace Objects
{
    public struct ScreenObjectUpdate
    {
        public List<Point> m_NewPositions = new List<Point>();
        public List<string> m_ImageSources = new List<string>();
        public int m_ObjectNumber;
        public eScreenObjectType m_ScreenObjectType;

        public ScreenObjectUpdate(
            eScreenObjectType i_Type,
            int i_ObjectNumber,
            List<Point> i_Points,
            List<string> i_ImageSources)
        {
            m_ScreenObjectType = i_Type;
            m_ObjectNumber = i_ObjectNumber;
            m_NewPositions = i_Points;
            m_ImageSources = i_ImageSources;
        }
    }
}
