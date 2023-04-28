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
        public List<Point> m_PointsOnGrid = new List<Point>();
        public List<Point> m_PointsOnScreen = new List<Point>();
        public List<string> m_ImageSources = new List<string>();
        public int m_ObjectNumber;
        public eScreenObjectType m_ScreenObjectType;
        private int m_GameBoardGridSize;
        private Point m_ValuesToAdd = new Point();
        private int m_Velocity;
        public Direction m_Direction;

        public GameObject(eScreenObjectType i_ScreenObjectType,int i_ObjectNumber, int i_GameBoardGridSize, Point i_ValuesToAdd)
        {
            m_ObjectNumber = i_ObjectNumber;
            m_ScreenObjectType = i_ScreenObjectType;
            m_GameBoardGridSize = i_GameBoardGridSize;
            m_ValuesToAdd = i_ValuesToAdd;
        }

        public void SetObject(ref ScreenObjectAdd i_ScreenObject)
        {
            m_PointsOnGrid.Add(i_ScreenObject.m_Point);
            Point p = new Point();
            p = i_ScreenObject.m_Point;
            p.m_Column = p.m_Column * m_GameBoardGridSize + m_ValuesToAdd.m_Column;
            p.m_Row = p.m_Row * m_GameBoardGridSize + m_ValuesToAdd.m_Row;
            m_PointsOnScreen.Add(p);
            m_ImageSources.Add(i_ScreenObject.m_ImageSource);
            i_ScreenObject.m_Point = p;
        }

        public ScreenObjectUpdate GetObjectUpdate()
        {
            ScreenObjectUpdate ret = new ScreenObjectUpdate();

            //if(m_CurrentDirection == eButton.Up)
            //{

            //}
            //el


        //if (m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.LowerRow)
        //{
        //    if (button.ClassId == eButton.Up.ToString())
        //    {
        //        PlayerObject.m_Row -= movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //    else if (button.ClassId == eButton.Down.ToString())
        //    {
        //        PlayerObject.m_Row += movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //    else if (button.ClassId == eButton.Right.ToString())
        //    {
        //        PlayerObject.m_Column += movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //    else
        //    {
        //        PlayerObject.m_Column -= movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //}
        //else
        //{
        //    if (button.ClassId == eButton.Up.ToString())
        //    {
        //        PlayerObject.m_Row += movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //    else if (button.ClassId == eButton.Down.ToString())
        //    {
        //        PlayerObject.m_Row -= movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //    else if (button.ClassId == eButton.Right.ToString())
        //    {
        //        PlayerObject.m_Column -= movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //    else
        //    {
        //        PlayerObject.m_Column += movementDistance;
        //        GameObjectUpdate.Invoke(this, PlayerObject);
        //    }
        //}




            ret.m_ObjectNumber = m_ObjectNumber;
            ret.m_ImageSources = m_ImageSources;

            for(int i = 0; i < m_PointsOnGrid.Count; i++)
            {
                Point a = m_PointsOnGrid[i];
                a.m_Column += 35;
                m_PointsOnGrid[i]= a;
            }
            ret.m_NewPositions = m_PointsOnGrid;
            ret
                .m_ScreenObjectType = m_ScreenObjectType;

            return ret;
        }
    }
}
