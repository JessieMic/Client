using Objects;
using Objects.Enums.BoardEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Pong
{
    internal class Bat: GameObject
    {
        public int AmountOfLives { get; set; } = 2;
        private bool m_IsDyingAnimationOn = false;
        public double m_DeathAnimationStart;
        private ClickReleaseMover m_ClickReleaseMover = new ClickReleaseMover();

        public Bat(int i_PlayerNumber, int i_X, int i_Y, int[,] i_Board)
        {
            Velocity = 300;
            DoWeCheckTheObjectForCollision = true;
            ObjectNumber = i_PlayerNumber;
            MonitorForCollision = true;
            Board = i_Board;
            this.Initialize(eScreenObjectType.Player, i_PlayerNumber, "uibackground.png", getPointOnGrid(i_X, i_Y), true,
                m_GameInformation.PointValuesToAddToScreen);
            m_ClickReleaseMover.Movable = this as IMovable;
            Size = new SizeDTO(45 * 5,20);
        }

        Point getPointOnGrid(int i_X, int i_Y)
        {
            Point point = new Point();

            if(m_GameInformation.AmountOfPlayers == 4)
            {
                if(ObjectNumber == 1 || ObjectNumber == 3)
                {
                    point.Column = m_GameInformation.GameBoardSizeByGrid.Width / 4;
                }
                else
                {
                    point.Column = m_GameInformation.GameBoardSizeByGrid.Width * 3/ 4;
                }

                if(ObjectNumber == 2 || ObjectNumber == 1)
                {
                    point.Row = 0.5;
                }
                else
                {
                    point.Row = m_GameInformation.GameBoardSizeByGrid.Height - 1;
                }
            }
            else
            {
                point.Column = m_GameInformation.GameBoardSizeByGrid.Width / 2;
                if(ObjectNumber == 1)
                {
                    point.Row = 0.5;
                }
                else
                {
                    point.Row = m_GameInformation.GameBoardSizeByGrid.Height - 1;
                }
            }

            return point;
        }

        public override void Update(double i_TimeElapsed)
        {
            if (AmountOfLives != 0)
            {
                if (m_IsDyingAnimationOn)
                {
                    double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_DeathAnimationStart;

                    if (timePassed < 1600)
                    {
                        IsVisable = !IsVisable;
                    }
                    else
                    {
                        IsVisable = true;
                        m_IsDyingAnimationOn = false;
                        IsObjectMoving = true;
                    }
                }

                base.Update(i_TimeElapsed);
            }
        }
        protected override void  isPointOnBoard(ref Point i_Point)
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

        public override void Collided(ICollidable i_Collidable)
        {
            if(i_Collidable is Ball)
            {
                i_Collidable.Collided(this);
            }
            else
            {
                collidedWithSolid(i_Collidable);
                Direction = Direction.Stop;
            }
        }

        public void DeathAnimation(double i_DeathStartTime)
        {
            IsVisable = false;
            m_IsDyingAnimationOn = true;
            if (AmountOfLives == 0 && MonitorForCollision)
            {
                m_IsDyingAnimationOn = false;
                OnDisposed();
            }
            m_DeathAnimationStart = i_DeathStartTime;
            m_IsDyingAnimationOn = true;
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
    }
}
