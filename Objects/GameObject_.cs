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
    public class GameObject_
    {
        public Point PointOnGrid { get; set; }
        public Point PointOnScreen { get; set; }

        public int Rotatation { get; set; } = 0;

        public int ScaleX { get; set; } = 1;

        public int ScaleY { get; set; } = 1;
        public string ImageSource { get; set; }
        public int ObjectNumber { get; set; }
        public eScreenObjectType ScreenObjectType { get; set; }
        private int GameBoardGridSize { get; set; }

        private Point m_ValuesToAdd;
        //protected int m_Velocity { get; set; } = 1;
        public bool IsObjectMoving { get; set; } = false;
        public Direction Direction { get; set; } = Direction.Stop;
        public eButton ButtonType { get; set; }
        public string Text { get; set; }
        public SizeDTO m_OurSize = GameSettings.m_MovementButtonOurSize;
        public int ID { get; set; }
        public bool Fade { get; set; } = false;

        public void Initialize(eScreenObjectType i_ScreenObjectType, int i_ObjectNumber, string i_Png, Point i_Point, int i_GameBoardGridSize, Point i_ValuesToAdd)
        {
            ObjectNumber = i_ObjectNumber;
            ScreenObjectType = i_ScreenObjectType;
            GameBoardGridSize = i_GameBoardGridSize;
            m_ValuesToAdd = i_ValuesToAdd;
            PointOnGrid=i_Point;
            Point point = getScreenPoint(i_Point);
            PointOnGrid = point;
            ImageSource=i_Png;
            ID=GameSettings.getID();
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

        public void InitializeButton(eButton i_ButtonType, string i_Png, Point i_Point, int i_GameBoardGridSize, SizeDTO i_OurSize, Point i_ValuesToAdd)
        {
            ButtonType = i_ButtonType;
            ScreenObjectType = eScreenObjectType.Button;
            GameBoardGridSize = i_GameBoardGridSize;
            m_ValuesToAdd = i_ValuesToAdd;
            PointOnGrid=i_Point;
            Point point = getScreenPoint(i_Point);
            PointOnScreen=point;
            ImageSource=i_Png;
            ID=GameSettings.getID();
            m_OurSize = i_OurSize;
        }

        //public void set(GameObject i_GameObject)
        //{
        //    m_PointsOnGrid = i_GameObject.m_PointsOnGrid;
        //    m_PointsOnScreen = i_GameObject.m_PointsOnScreen;
        //    m_ImageSources = i_GameObject.m_ImageSources;
        //    m_ObjectNumber = i_GameObject.m_ObjectNumber;
        //    m_ScreenObjectType = i_GameObject.m_ScreenObjectType;
        //    m_GameBoardGridSize = i_GameObject.m_GameBoardGridSize;
        //    m_ValuesToAdd = i_GameObject.m_ValuesToAdd;
        //    m_OurSize = i_GameObject.m_OurSize;
        //    m_ID = i_GameObject.m_ID;
        //    m_Fade = i_GameObject.m_Fade;
        //    m_Rotatation = i_GameObject.m_Rotatation;
        //    m_ScaleX = i_GameObject.m_ScaleX;
        //    m_ScaleY = i_GameObject.m_ScaleY;
        //}

        public void FadeWhenObjectIsRemoved()
        {
            Fade = true;
        }

        private Point getScreenPoint(Point i_Point)
        {
            Point point = new Point();
            point = i_Point;

            point.m_Column = point.m_Column * GameBoardGridSize + m_ValuesToAdd.m_Column;
            point.m_Row = point.m_Row * GameBoardGridSize + m_ValuesToAdd.m_Row;
            return point;
        }

        public Point GetOneMoveAhead()
        {
            return PointOnGrid.Move(Direction);
        }

        public void MoveToPoint(Point i_Point)
        {
            PointOnGrid = i_Point;
            Point point = getScreenPoint(i_Point);
            PointOnScreen = point;
        }

        public void MoveSameDirection()
        {
            MoveToPoint(GetOneMoveAhead());
        }
    }
}
