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
    public class GameObject : ICollidable , IMovable
    {
        public event EventHandler<EventArgs> UpdateGameObject;
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<int> SpecialEvent;
        public event EventHandler<Point> UpdatePosition;
        //public bool Turn { get; set; }
        public Point PointOnScreen { get; set; }
        public bool IsVisable { get; set; } = true;
        public int Rotatation { get; set; } = 0;
        public int ScaleX { get; set; } = 1;
        private Point m_StartupPoint;
        public bool MonitorForCollision { get; set; }
        public int ScaleY { get; set; } = 1;
        public string ImageSource { get; set; }
        public int ObjectNumber { get; set; }
        public bool IsCollisionEnabled { get; set; } = true;
        public eScreenObjectType ScreenObjectType { get; set; }
        public int GameBoardGridSize { get; set; } = GameSettings.GameGridSize;
        protected GameInformation m_GameInformation = GameInformation.Instance;
        protected Point m_ValuesToAdd;
        public bool IsObjectMoving { get; set; } = true;
        public Direction Direction { get; set; } = Direction.Stop;
        public Direction RequestedDirection { get; set; } = Direction.Stop;
        public eButton ButtonType { get; set; }
        public string Text { get; set; }
        public SizeDTO Size { get; set; }= GameSettings.m_MovementButtonOurSize;
        public int ID { get; set; }
        public int Velocity { get; set; } = 90;
        public bool Fade { get; set; } = false;
        public bool DoWeCheckTheObjectForCollision { get; set; } = false;
        public int[,] Board { get; set; }
        public bool WantToTurn { get; set; } = false;
        protected bool m_CanRotateToAllDirections = true;
        protected bool m_FlipsWhenMoved = false;

        public int Updated { get; set; } = 0;
        public int ZIndex { get; set; } = -1;

        public void Initialize(eScreenObjectType i_ScreenObjectType, int i_ObjectNumber, string i_Png, Point i_Point, bool i_IsGrid, Point i_ValuesToAdd)
        {
            ObjectNumber = i_ObjectNumber;
            ScreenObjectType = i_ScreenObjectType;
            m_ValuesToAdd = i_ValuesToAdd;
            Point point = GetScreenPoint(i_Point, i_IsGrid);
            PointOnScreen = m_StartupPoint = point;
            ImageSource = i_Png;
            ID = GameSettings.getID();
        }

        public void Initialize()
        {
            ID = GameSettings.getID();
        }

        public void SetImageDirection(Direction i_Direction)
        {

            if (m_CanRotateToAllDirections)
            {
                if (i_Direction == Direction.Up)
                {
                    Rotatation = 270;
                    ScaleX = 1;
                }
                else if (i_Direction == Direction.Down)
                {
                    Rotatation = 90;
                    ScaleX = 1;
                }
            }

            if (i_Direction == Direction.Left)
            {
                if (m_FlipsWhenMoved)
                {
                    ScaleX = -1;
                    Rotatation = 0;
                }
                else
                {
                    Rotatation = 180;
                    ScaleX = 1;
                }
            }
            else if (i_Direction == Direction.Right)
            {
                ScaleX = 1;
                Rotatation = 0;
            }
        }

        public void InitializeButton(eButton i_ButtonType, string i_Png, Point i_Point, bool i_IsGrided, SizeDTO i_Size, Point i_ValuesToAdd)
        {
            ButtonType = i_ButtonType;
            ScreenObjectType = eScreenObjectType.Button;
            m_ValuesToAdd = i_ValuesToAdd;
            Point point = GetScreenPoint(i_Point, i_IsGrided);
            PointOnScreen = point;
            ImageSource = i_Png;
            ID = GameSettings.getID();
            Size = i_Size;
        }

        public void OnDraw()
        {
            UpdateGameObject.Invoke(this, null);
        }

        public virtual void RequestDirection(Direction i_Direction)
        { }

        protected void OnSpecialEvent(int eventNumber)
        {
            SpecialEvent.Invoke(this, eventNumber);
        }

        public void OnUpdatePosition(Point i_Point)
        {
            UpdatePosition.Invoke(this, i_Point);
        }

        protected void resetToStartupPoint()
        {
            IsObjectMoving = false;
            PointOnScreen = m_StartupPoint;
            Direction = RequestedDirection = Direction.Stop;
        }

        public virtual void OnDisposed()
        {
            MonitorForCollision = false;
            IsCollisionEnabled =false;
            Disposed?.Invoke(this, null);
            IsVisable = false;
            OnUpdate();
        }

        public virtual void OnUpdate()
        {
            UpdateGameObject?.Invoke(this, null);
        }

        public Point GetPointOnGrid()
        {
            double temp;
            Point point = GetCurrentPointOnScreen();

            temp = point.Row / GameSettings.GameGridSize;
            point.Row = (int)Math.Round(temp);
            temp = point.Column / GameSettings.GameGridSize;
            point.Column = (int)Math.Round(temp);

            return point;
        }

        protected virtual void isPointOnBoard(ref Point i_Point)
        {
            if (i_Point.Column < m_ValuesToAdd.Column)
            {
                i_Point.Column = m_ValuesToAdd.Column;
            }
            else if (i_Point.Row < m_ValuesToAdd.Row)
            {
                i_Point.Row = m_ValuesToAdd.Row;
            }
            else if (i_Point.Row > m_GameInformation.GameBoardSizeByPixel.Height + m_ValuesToAdd.Row - Size.Width)
            {
                i_Point.Row = m_GameInformation.GameBoardSizeByPixel.Height + m_ValuesToAdd.Row - Size.Width;
            }
            else if (i_Point.Column > m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - Size.Height)
            {
                i_Point.Column = m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - Size.Height;
            }
        }

        protected  virtual void collidedWithSolid(ICollidable i_Solid)//(Point i_PointOfSolid,SizeInPixelsDto i_SizeOfSolid)
        {
            Point newPoint = PointOnScreen;
            if (Direction == Direction.Left)//solid on the left
            {
                newPoint.Column = i_Solid.PointOnScreen.Column + i_Solid.Bounds.Width;
            }
            else if (Direction == Direction.Right) //solid on the right
            {
                newPoint.Column = i_Solid.PointOnScreen.Column - Size.Width;
            }
            else if (Direction == Direction.Up)
            {
                newPoint.Row = i_Solid.PointOnScreen.Row + i_Solid.Bounds.Height;
            }
            else //direction is down
            {
                newPoint.Row = i_Solid.PointOnScreen.Row - Size.Height;
            }
            PointOnScreen = newPoint;
        }

        public Point GetScreenPoint(Point i_Point, bool i_IsGrided)
        {
            Point point = new Point();

            if (!i_IsGrided)
            {
                GameBoardGridSize = 1;
            }

            point = i_Point;
            point.Column = point.Column * GameBoardGridSize + m_ValuesToAdd.Column;
            point.Row = point.Row * GameBoardGridSize + m_ValuesToAdd.Row;

            return point;
        }

        public virtual void Update(double i_TimeElapsed)
        {
            updatePosition(i_TimeElapsed);
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            if (i_Source != null)
            {
                collided = i_Source.Bounds.IntersectsWith(this.Bounds);
            }

            return collided;
        }

        public virtual void Collided(ICollidable i_Collidable)
        { }

        protected void centerObjectInGrid()
        {
            Point newPoint = PointOnScreen;
            double valueToAdd = (GameSettings.GameGridSize / 2) - Size.Height / 2;

            newPoint.Row += valueToAdd;
            newPoint.Column += valueToAdd;
            PointOnScreen = newPoint;
        }

        private void updatePosition(double i_TimeElapsed)
        {
            if (WantToTurn)
            {
                WantToTurn = false;
            }
            else
            {
                Point newPoint = PointOnScreen;
                newPoint.Column += ((Direction.ColumnOffset * Velocity) * i_TimeElapsed / 1000);
                //newPoint.Row += ((Direction.RowOffset * Velocity) * i_TimeElapsed / 1000);

                isPointOnBoard(ref newPoint);
                PointOnScreen = newPoint;
            }
        }

        public Point GetCurrentPointOnScreen()//without add values
        {
            Point value = PointOnScreen;

            value.Row -= m_ValuesToAdd.Row;
            value.Column -= m_ValuesToAdd.Column;

            return value;
        }

        public void UpdatePointOnScreen(Point i_Point)
        {
            Point p = GetScreenPoint(i_Point, true);
            Updated = 0;
            if (!m_GameInformation.IsPointIsOnBoardPixels(p))
            {
                PointOnScreen = p;
            }
            else if(!m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))//update is on screen, if actual point isnt 
            {
                PointOnScreen = p;
                System.Diagnostics.Debug.WriteLine("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
            }
        }

        public Rect Bounds
        {
            get
            {
                return new Rect(PointOnScreen.Column, PointOnScreen.Row,
                Size.Width, Size.Height);
            }
        }
        public void MoveToPointInGrided(Point i_Point)
        {
            Point point = GetScreenPoint(i_Point, true);
            PointOnScreen = point;
        }
    }
}

//public void FlipImage(Direction i_Direction)
//{
//    if (i_Direction == Direction.Left)
//    {
//        ScaleX = -1;
//    }
//    else if (i_Scale == eImageScale.FlipY)
//    {
//        ScaleY = -1;
//        Rotatation = 0;
//    }
//    if (i_Scale == eImageScale.OriginalX)
//    {
//        ScaleX = 1;
//    }
//    else
//    {
//        ScaleY = 1;
//        Rotatation = 0;
//    }
//}