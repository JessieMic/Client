using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTOs;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace Objects
{
    [Serializable]
    public class GameObject 
    {
        public List<Point> m_PointsOnGrid = new List<Point>();
        public List<Point> m_PointsOnScreen = new List<Point>();
        public List<int> m_Rotatation = new List<int>();
        public List<int> m_ScaleX = new List<int>();
        public List<int> m_ScaleY = new List<int>();
        public List<string> m_ImageSources = new List<string>();
        public int m_ObjectNumber;
        public eScreenObjectType m_ScreenObjectType;
        private int m_GameBoardGridSize;
        protected Point m_ValuesToAdd = new Point();
        protected int m_Velocity = 1;
        public bool m_IsObjectMoving = false;
        public Direction m_Direction = Direction.Stop;
        public eButton m_ButtonType;
        public string m_text;
        public SizeDTO m_OurSize = GameSettings.m_MovementButtonOurSize;
        public List<int> m_ID = new List<int>();
        public bool m_Fade = false;

        public GameObject()
        {
            m_Rotatation.Add(0);
            m_ScaleX.Add(1);
            m_ScaleY.Add(1);
        }

        //public void Clone()
        //{

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
            m_ID.Add(GameSettings.getID());
        }

        public void SetImageDirection(int i_Index, Direction i_Direction)
        {
            if(i_Direction == Direction.Up)
            {
                m_Rotatation[i_Index] = 270;
            }
            else if(i_Direction == Direction.Down)
            {
                m_Rotatation[i_Index] = 90;
            }
            else if(i_Direction == Direction.Left)
            {
                m_Rotatation[i_Index] = 180;
            }
            else
            {
                m_Rotatation[i_Index] = 0;
            }
        }

        public void FlipImage(int i_Index,eImageScale i_Scale)
        {
            if(i_Scale == eImageScale.FlipX)
            {
                m_ScaleX[i_Index] = -1;
            }
            else if(i_Scale == eImageScale.FlipY)
            {
                m_ScaleY[i_Index] = -1;
            }
            if (i_Scale == eImageScale.OriginalX)
            {
                m_ScaleX[i_Index] = 1;
            }
            else
            {
                m_ScaleY[i_Index] = 1;
            }
        }

        protected int getAmountOfCombinedObjects()
        {
            return m_ID.Count;
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
            m_Rotatation.Add(0);
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
            m_Rotatation = i_GameObject.m_Rotatation;
            m_ScaleX = i_GameObject.m_ScaleX;
            m_ScaleY = i_GameObject.m_ScaleY;

        }
        public void Copyy(GameObject i_GameObject)
        {
            m_ObjectNumber = i_GameObject.m_ObjectNumber;
            m_ScreenObjectType = i_GameObject.m_ScreenObjectType;
            m_GameBoardGridSize = i_GameObject.m_GameBoardGridSize;
            m_ValuesToAdd = i_GameObject.m_ValuesToAdd;
            m_OurSize = i_GameObject.m_OurSize;
            m_Fade = i_GameObject.m_Fade;
            m_ID = i_GameObject.m_ID;
            m_Rotatation = i_GameObject.m_Rotatation;

            m_ScaleX = i_GameObject.m_ScaleX;
            m_ScaleY = i_GameObject.m_ScaleY;

            for (int i = 0; i < i_GameObject.m_ID.Count; i++)
            {
                m_PointsOnGrid.Add(new Point(i_GameObject.m_PointsOnGrid[i].m_Column, i_GameObject.m_PointsOnGrid[i].m_Row));
                m_PointsOnScreen.Add(new Point(i_GameObject.m_PointsOnScreen[i].m_Column, i_GameObject.m_PointsOnScreen[i].m_Row));
                m_ImageSources.Add(new string(i_GameObject.m_ImageSources[i]));
                //m_ID.Add(i_GameObject.m_ID[i]);
                //m_Rotatation.Add(new double(i_GameObject.m_Rotatation));
            }
        }

        public void CombineGameObjects(GameObject i_GameObject)
        {
            m_PointsOnGrid.Add(i_GameObject.m_PointsOnGrid[0]);
            m_PointsOnScreen.Add(i_GameObject.m_PointsOnScreen[0]);
            m_ImageSources.Add(i_GameObject.m_ImageSources[0]);
            m_ID.Add(i_GameObject.m_ID[0]);
            m_ScaleX.Add(i_GameObject.m_ScaleX[0]);
            m_ScaleY.Add(i_GameObject.m_ScaleY[0]);
            m_Rotatation.Add(i_GameObject.m_Rotatation[0]);
        }
        public void CombineGameObjectsTop(GameObject i_GameObject)
        {
            m_PointsOnGrid.Insert(0, i_GameObject.m_PointsOnGrid[0]);
            m_PointsOnScreen.Insert(0, i_GameObject.m_PointsOnScreen[0]);
            m_ImageSources.Insert(0, i_GameObject.m_ImageSources[0]);
            m_ID.Insert(0, i_GameObject.m_ID[0]);
            m_ScaleX.Insert(0, i_GameObject.m_ScaleX[0]);
            m_ScaleY.Insert(0, i_GameObject.m_ScaleY[0]);
            m_Rotatation.Insert(0, i_GameObject.m_Rotatation[0]);
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
