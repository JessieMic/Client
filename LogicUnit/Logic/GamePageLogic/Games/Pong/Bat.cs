using Objects;
using Objects.Enums.BoardEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Objects.Enums;
using Point = Objects.Point;


namespace LogicUnit.Logic.GamePageLogic.Games.Pong
{
    internal class Bat : GameObject
    {
        public int AmountOfLives { get; set; } = 2;
        private bool m_IsDyingAnimationOn = false;
        public double m_DeathAnimationStart;
        private bool m_IncreaseVelocity = false;
        private ClickReleaseMover m_ClickReleaseMover = new ClickReleaseMover();

        public Bat(int i_PlayerNumber, int i_X, int i_Y, int[,] i_Board)
        {
            Velocity = 650;
            DoWeCheckTheObjectForCollision = true;
            ObjectNumber = i_PlayerNumber;
            MonitorForCollision = true;
            Board = i_Board;
            this.Initialize(eScreenObjectType.Player, i_PlayerNumber, "bat.png", getPointOnGrid(i_X, i_Y), true,
                m_GameInformation.PointValuesToAddToScreen);
            m_ClickReleaseMover.Movable = this as IMovable;
            Size = new SizeD(45 * 4, 20);
        }

        Point getPointOnGrid(int i_X, int i_Y)
        {
            Point point = new Point();

            if (m_GameInformation.AmountOfPlayers == 4)
            {
                if (ObjectNumber == 1 || ObjectNumber == 3)
                {
                    point.Column = m_GameInformation.GameBoardSizeByGrid.Width / 4;
                }
                else
                {
                    point.Column = m_GameInformation.GameBoardSizeByGrid.Width * 3 / 4;
                }

                if (ObjectNumber == 2 || ObjectNumber == 1)
                {
                    point.Row = 0;
                }
                else
                {
                    point.Row = m_GameInformation.GameBoardSizeByGrid.Height - 0.5;
                }
            }
            else
            {
                point.Column = m_GameInformation.GameBoardSizeByGrid.Width / 2;
                if (ObjectNumber == 1)
                {
                    point.Row = 0;
                }
                else
                {
                    point.Row = m_GameInformation.GameBoardSizeByGrid.Height-0.5;// - 1;
                }
            }

            return point;
        }

        public override void Update(double i_TimeElapsed)
        {
            if (AmountOfLives != 0)
            {
                changeVelocity(); 
                updatePosition(i_TimeElapsed);
            }
        }


        protected override void isPointOnBoard(ref Point i_Point)
        {
            if (i_Point.Column < m_ValuesToAdd.Column)
            {
                i_Point.Column = m_ValuesToAdd.Column;
            }
            else if (i_Point.Column > m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - Size.Width)
            {
                i_Point.Column = m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - Size.Width;
            }
        }

        private void changeVelocity()
        {
            if (m_IncreaseVelocity)
            {
                m_IncreaseVelocity = false;
                if (Velocity < 1200)
                {
                    Velocity += 10;
                }
                if ( Size.Width > 45 )
                {
                    SizeD a = Size;
                    a.Width -= 7;
                    Size = a;
                }

            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Ball)
            {
                i_Collidable.Collided(this);
            }
            else
            {
                collidedWithSolid(i_Collidable);
                Direction = Direction.Stop;
            }
        }

        public void Reset()
        {
            Size = new SizeD(45 * 4, 20);
        }

        public void IncreaseVelocity()
        {
            m_IncreaseVelocity = true;
        }

        public override void RequestDirection(Direction i_Direction)
        {
            m_ClickReleaseMover.RequestDirection(i_Direction);
        }

        protected override void updatePosition(double i_TimeElapsed)
        {
            Point newPoint = PointOnScreen;

            newPoint.Column += ((Direction.ColumnOffset * Velocity) * i_TimeElapsed / 1000);
            isPointOnBoard(ref newPoint);
            PointOnScreen = newPoint;
        }

        protected override void collidedWithSolid(ICollidable i_Solid)//(Point i_PointOfSolid,SizeInPixelsD i_SizeOfSolid)
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
            
            PointOnScreen = newPoint;
        }

        public override void UpdatePointOnScreenByPixel(Point i_Point)
        {
            Updated = 0;
            if (!m_GameInformation.IsPointIsOnBoardPixels(i_Point))
            {
                Point p = PointOnScreen;
                p.Column = i_Point.Column;
                PointOnScreen = p;
            }
            else if (!m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))//update is on screen, if actual point isnt 
            {
                Point p = PointOnScreen;
                p.Column = i_Point.Column;
                PointOnScreen = p;
            }
        }
    }
}