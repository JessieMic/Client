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

        public Bat(int i_playerNumber, int i_X, int i_Y, int[,] i_Board)
        {
            Velocity = 300;
            DoWeCheckTheObjectForCollision = true;
            ObjectNumber = i_playerNumber;
            MonitorForCollision = true;
            Board = i_Board;
            this.Initialize(eScreenObjectType.Player, i_playerNumber, "pacmanfood.png", getPointOnGrid(i_X, i_Y), true,
                m_GameInformation.PointValuesToAddToScreen);
            m_ClickReleaseMover.Movable = this as IMovable;
            Size = new SizeDTO(45 * 5,20);
        }

        Point getPointOnGrid(int i_X, int i_Y)
        {
            Point point;
            if (ObjectNumber == 2)
            {
               point = new Point(
                    m_GameInformation.GameBoardSizeByGrid.Width / 2,
                    m_GameInformation.GameBoardSizeByGrid.Height-1);
            }
            else if (ObjectNumber == 3)
            {
                point = new Point(0, i_Y - 1);
            }
            else if (ObjectNumber == 4)
            {
                point = new Point(i_X - 1, i_Y - 1);
            }
            else
            {
                point = new Point(
                    m_GameInformation.GameBoardSizeByGrid.Width / 2,
                    0);
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

        public override void Collided(ICollidable i_Collidable)
        {
            //if (i_Collidable is Explosion)
            //{
            //    if (!m_IsDyingAnimationOn && m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen) && IsObjectMoving)
            //    {
            //        IsObjectMoving = false;
            //        OnSpecialEvent(-1);
            //    }
            //}
            //else if (i_Collidable is Boarder)
            //{
            //    collidedWithSolid(i_Collidable);
            //}
            //else if (i_Collidable is BreakableBoarder)
            //{
            //    collidedWithSolid(i_Collidable);
            //}
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
    }
}
