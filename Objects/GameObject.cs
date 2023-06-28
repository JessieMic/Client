using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

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
        protected Point m_ValuesToAdd = new Point();
        protected int m_Velocity = 1;
        public Direction m_Direction = Direction.Stop;
        public eButton m_ButtonType;
        public string m_text;
        public SizeDTO m_OurSize = new SizeDTO(35,35);
        public List<int> m_ID = new List<int>();
        public bool m_Fade = false;
        
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
            m_ID.Add(GameSettings.getID());
        }

        public void InitializeButton(eButton i_ButtonType, string i_Png, Point i_Point, int i_GameBoardGridSize, SizeDTO i_OurSize, Point i_ValuesToAdd)
        {
            m_ButtonType = i_ButtonType;
            m_ScreenObjectType = eScreenObjectType.Button;
            m_GameBoardGridSize = i_GameBoardGridSize;
            m_ValuesToAdd = i_ValuesToAdd;
            m_PointsOnGrid.Add(i_Point);
            Point point = getScreenPoint(i_Point);
            m_PointsOnScreen.Add(point);
            m_ImageSources.Add(i_Png);
            m_ID.Add(GameSettings.getID());
            m_OurSize = i_OurSize;
        }

        public void set(GameObject i_GameObject)
        {
            m_PointsOnGrid = i_GameObject.m_PointsOnGrid;
            m_PointsOnScreen = i_GameObject.m_PointsOnScreen;
            m_ImageSources = i_GameObject.m_ImageSources;
            m_ObjectNumber = i_GameObject.m_ObjectNumber;
            m_ScreenObjectType = i_GameObject.m_ScreenObjectType;
            m_GameBoardGridSize = i_GameObject.m_GameBoardGridSize;
            m_ValuesToAdd = i_GameObject.m_ValuesToAdd;
            m_OurSize = i_GameObject.m_OurSize;
            m_ID = i_GameObject.m_ID;
            m_Fade = i_GameObject.m_Fade;
        }

        //public void SetObject(string i_Image, Point i_Point)
        //{
        //    m_PointsOnGrid.Add(i_Point);
        //    Point point = getScreenPoint(i_Point);
        //    m_PointsOnScreen.Add(point);
        //    m_ImageSources.Add(i_Image);
        //    m_ID.Add();
        //}

        public void CombineGameObjects(GameObject i_GameObject)
        {
            m_PointsOnGrid.Add(i_GameObject.m_PointsOnGrid[0]);
            m_PointsOnScreen.Add(i_GameObject.m_PointsOnScreen[0]);
            m_ImageSources.Add(i_GameObject.m_ImageSources[0]);
            m_ID.Add(i_GameObject.m_ID[0]);
        }

        public void FadeWhenObjectIsRemoved()
        {
            m_Fade = true;
        }

        private Point getScreenPoint(Point i_Point)
        {
            Point point = new Point();
            point = i_Point;

            point.m_Column = point.m_Column * m_GameBoardGridSize + m_ValuesToAdd.m_Column;
            point.m_Row = point.m_Row * m_GameBoardGridSize + m_ValuesToAdd.m_Row;
            return point;
        }

        //public void update()
        //{
        //    for(int i=0; i< m_Images.Count; i++)
        //    {
        //        m_Images[i].TranslationX = m_PointsOnScreen[i].m_Column;
        //        m_Images[i].TranslationY = m_PointsOnScreen[i].m_Row;
        //    }
        //}

        public void AddPointTop(Point i_Point)
        {
            m_PointsOnGrid.Insert(0, i_Point);
            m_PointsOnScreen.Insert(0, getScreenPoint(i_Point));
        }

        public Point GetOneMoveAhead()
        {
            return m_PointsOnGrid.First().Move(m_Direction);
        }

        public void MoveToPoint(Point i_Point)
        {
            m_PointsOnGrid[0]=i_Point;
            Point point = getScreenPoint(i_Point);
            m_PointsOnScreen[0] = point;
            //update();
        }

        public void MoveSameDirection()
        {
            MoveToPoint(GetOneMoveAhead());
        }

        public void PopPoint()
        {
            m_PointsOnGrid.RemoveAt(m_PointsOnGrid.Count - 1);
            m_PointsOnScreen.RemoveAt(m_PointsOnScreen.Count - 1);
        }
    }
}
