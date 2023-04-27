using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic
{
    public class GameObject
    {
        public List<Point> m_Positions = new List<Point>();
        public List<string> m_ImageSources = new List<string>();
        public int m_ObjectNumber;
        public eScreenObjectType m_ScreenObjectType;
        private int m_Velocity;

        public GameObject(eScreenObjectType i_ScreenObjectType,int i_ObjectNumber)
        {
            m_ObjectNumber = i_ObjectNumber;
            m_ScreenObjectType = i_ScreenObjectType;
        }

        public void SetObject(ScreenObjectAdd i_ScreenObject)
        {
            m_Positions.Add(i_ScreenObject.m_Point);
            m_ImageSources.Add(i_ScreenObject.m_ImageSource);
        }

        public ScreenObjectUpdate GetObjectUpdate()
        {
            ScreenObjectUpdate ret = new ScreenObjectUpdate();
            ret.m_ObjectNumber = m_ObjectNumber;
            ret.m_ImageSources = m_ImageSources;

            for(int i = 0; i < m_Positions.Count; i++)
            {
                Point a = m_Positions[i];
                a.m_Column += 35;
                m_Positions[i]= a;
            }
            ret.m_NewPositions = m_Positions;
            ret
                .m_ScreenObjectType = m_ScreenObjectType;

            return ret;
        }
    }
}
