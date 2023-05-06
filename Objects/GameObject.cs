using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Size = Objects.Size;

namespace Objects
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
        private int m_Velocity = 1;
        public Direction m_Direction = Direction.Stop;
        public eButton m_ButtonType;
        public string m_text;
        public Size m_Size = new Size(35,35);

        //public void Initialize(eScreenObjectType i_ScreenObjectType, int i_ObjectNumber, int i_GameBoardGridSize, Point i_ValuesToAdd)
        //{
        //    m_ObjectNumber = i_ObjectNumber;
        //    m_ScreenObjectType = i_ScreenObjectType;
        //    m_GameBoardGridSize = i_GameBoardGridSize;
        //    m_ValuesToAdd = i_ValuesToAdd;
        //}

        public void Initialize(eScreenObjectType i_ScreenObjectType, int i_ObjectNumber, string i_Png, Point i_Point, int i_GameBoardGridSize, Point i_ValuesToAdd)
        {
            m_ObjectNumber = i_ObjectNumber;
            m_ScreenObjectType = i_ScreenObjectType;
            m_GameBoardGridSize = i_GameBoardGridSize;
            m_ValuesToAdd = i_ValuesToAdd;
            m_PointsOnGrid.Add(i_Point);
            Point point = getScreenPoint(i_Point);
            m_PointsOnScreen.Add(point);
            m_ImageSources.Add(i_Png);
        }

        public void InitializeButton(eButton i_ButtonType, string i_Png, Point i_Point, int i_GameBoardGridSize, Size i_Size, Point i_ValuesToAdd)
        {
            m_ButtonType = i_ButtonType;
            m_ScreenObjectType = eScreenObjectType.Button;
            m_GameBoardGridSize = i_GameBoardGridSize;
            m_ValuesToAdd = i_ValuesToAdd;
            m_PointsOnGrid.Add(i_Point);
            Point point = getScreenPoint(i_Point);
            m_PointsOnScreen.Add(point);
            m_ImageSources.Add(i_Png);
            m_Size = i_Size;
        }

        public void SetObject(string i_Image, Point i_Point)
        {
            m_PointsOnGrid.Add(i_Point);
            Point point = getScreenPoint(i_Point);
            m_PointsOnScreen.Add(point);
            m_ImageSources.Add(i_Image);
            //i_ScreenObject.m_Point = point;
        }

        public void CombineGameObjects(GameObject i_GameObject)
        {
            m_PointsOnGrid.Add(i_GameObject.m_PointsOnGrid[0]);
            m_PointsOnScreen.Add(i_GameObject.m_PointsOnScreen[0]);
            m_ImageSources.Add(i_GameObject.m_ImageSources[0]);
        }

        public void SetObject(ref ScreenObjectAdd i_ScreenObject)
        {
            m_PointsOnGrid.Add(i_ScreenObject.m_Point);
            Point point = getScreenPoint(i_ScreenObject.m_Point);
            m_PointsOnScreen.Add(point);
            m_ImageSources.Add(i_ScreenObject.m_ImageSource);
            i_ScreenObject.m_Point = point;
        }

        private Point getScreenPoint(Point i_Point)
        {
            Point point = new Point();
            point = i_Point;

            point.m_Column = point.m_Column * m_GameBoardGridSize + m_ValuesToAdd.m_Column;
            point.m_Row = point.m_Row * m_GameBoardGridSize + m_ValuesToAdd.m_Row;
            return point;
        }

        public void AddPointTop(Point i_Point)
        {
            m_PointsOnGrid.Insert(0, i_Point);
            m_PointsOnScreen.Insert(0, getScreenPoint(i_Point));
        }

        public void PopPoint()
        {
            m_PointsOnGrid.RemoveAt(m_PointsOnGrid.Count - 1);
            m_PointsOnScreen.RemoveAt(m_PointsOnScreen.Count - 1);
        }

        public ScreenObjectUpdate GetObjectUpdate()
        {
            ScreenObjectUpdate ret = new ScreenObjectUpdate();

            ret.m_ObjectNumber = m_ObjectNumber;
            ret.m_ImageSources = m_ImageSources;
            ret.m_NewPositions = m_PointsOnScreen;
            ret
                .m_ScreenObjectType = m_ScreenObjectType;

            return ret;
        }
    }
}
