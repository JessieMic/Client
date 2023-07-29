using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Microsoft.Maui.Controls.Shapes;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace Objects
{
    public class GameObject 
    {
        //public Point PointOnGrid { get; set; }
        public Point PointOnScreen { get; set; }

        public int Rotatation { get; set; } = 0;

        public int ScaleX { get; set; } = 1;

        public int ScaleY { get; set; } = 1;
        public string ImageSource { get; set; }
        public int ObjectNumber { get; set; }
        public eScreenObjectType ScreenObjectType { get; set; }
        public int GameBoardGridSize { get; set; } = GameSettings.m_GameBoardGridSize;
        protected GameInformation m_GameInformation = GameInformation.Instance;
        private Point m_ValuesToAdd;
        //protected int m_Velocity { get; set; } = 1;
        public bool IsObjectMoving { get; set; } = false;
        public Direction Direction { get; set; } = Direction.Stop;
        public eButton ButtonType { get; set; }
        public string Text { get; set; }
        public SizeDTO m_Size = GameSettings.m_MovementButtonOurSize;
        public int ID { get; set; } 

        public int Velocity { get; set; } = 120;

        private Rect m_Bounds = new Rect();

        public bool Fade { get; set; } = false;

        public void Initialize(eScreenObjectType i_ScreenObjectType, int i_ObjectNumber, string i_Png, Point i_Point, bool i_IsGrid, Point i_ValuesToAdd)
        {
            ObjectNumber = i_ObjectNumber;
            ScreenObjectType = i_ScreenObjectType;
            m_ValuesToAdd = i_ValuesToAdd;
            //PointOnGrid=i_Point;
            Point point = getScreenPoint(i_Point, i_IsGrid);
            PointOnScreen = point;
            ImageSource=i_Png;
            ID=GameSettings.getID();
            m_Bounds = new Rect(
                point.Column,
                point.Row,
                GameSettings.m_GameBoardGridSize,
                GameSettings.m_GameBoardGridSize);
        }

        public void SetImageDirection(Direction i_Direction)
        {
            if (i_Direction == Direction.Up)
            {
                Rotatation = 270;
            }
            else if (i_Direction == Direction.Down)
            {
                Rotatation = 90;
            }
            else if (i_Direction == Direction.Left)
            {
                Rotatation = 180;
            }
            else
            {
                Rotatation = 0;
            }
        }

        public void FlipImage(int i_Index, eImageScale i_Scale)
        {
            if (i_Scale == eImageScale.FlipX)
            {
                ScaleX = -1;
            }
            else if (i_Scale == eImageScale.FlipY)
            {
                ScaleY = -1;
            }
            if (i_Scale == eImageScale.OriginalX)
            {
                ScaleX = 1;
            }
            else
            {
                ScaleY= 1;
            }
        }

        public void InitializeButton(eButton i_ButtonType, string i_Png, Point i_Point, bool i_IsGrided, SizeDTO i_Size, Point i_ValuesToAdd)
        {
            ButtonType = i_ButtonType;
            ScreenObjectType = eScreenObjectType.Button;
            m_ValuesToAdd = i_ValuesToAdd;
            Point point = getScreenPoint(i_Point,i_IsGrided);
            PointOnScreen=point;
            ImageSource=i_Png;
            ID=GameSettings.getID();
            m_Size = i_Size;
        }

        //public void set(GameObject i_GameObject)
        //{
        //    m_PointsOnGrid = i_GameObject.m_PointsOnGrid;
        //    PointOnScreen = i_GameObject.PointOnScreen;
        //    m_ImageSources = i_GameObject.m_ImageSources;
        //    m_ObjectNumber = i_GameObject.m_ObjectNumber;
        //    ScreenObjectType = i_GameObject.ScreenObjectType;
        //    m_GameBoardGridSize = i_GameObject.m_GameBoardGridSize;
        //    m_ValuesToAdd = i_GameObject.m_ValuesToAdd;
        //    m_Size = i_GameObject.m_Size;
        //    ID = i_GameObject.ID;
        //    m_Fade = i_GameObject.m_Fade;
        //    m_Rotatation = i_GameObject.m_Rotatation;
        //    m_ScaleX = i_GameObject.m_ScaleX;
        //    m_ScaleY = i_GameObject.m_ScaleY;
        //}

        public void FadeWhenObjectIsRemoved()
        {
            Fade = true;
        }

        private Point getScreenPoint(Point i_Point,bool i_IsGrided)
        {
            Point point = new Point();
            
            if(!i_IsGrided)
            {
                GameBoardGridSize = 1;
            }

            point = i_Point;
            point.Column = point.Column * GameBoardGridSize + m_ValuesToAdd.Column;
            point.Row = point.Row * GameBoardGridSize + m_ValuesToAdd.Row;

            return point;
        }

        //public Point GetOneMoveAhead()
        //{
        //    return PointOnGrid.Move(Direction);
        //}

        public void Update(double i_TimeElapsed)
        {
            updatePosition(i_TimeElapsed);
        }

        private void updatePosition(double i_TimeElapsed)
        {
            Point newPoint = PointOnScreen;
            newPoint.Column += ((Direction.ColumnOffset * Velocity) * i_TimeElapsed/1000);
            newPoint.Row += ((Direction.RowOffset * Velocity) * i_TimeElapsed/1000);
            PointOnScreen = newPoint;
        }

        public Rect Rect
        {
            get
            {
                return new Rect(PointOnScreen.Row,PointOnScreen.Column,
                m_Size.Width,m_Size.Height);
            }
        }
    

        public void MoveToPointInGrided(Point i_Point)
        {
            //PointOnGrid = i_Point;
            Point point = getScreenPoint(i_Point,true);
            PointOnScreen = point;
        }
    }
}
