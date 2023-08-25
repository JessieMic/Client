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

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    internal class BombItPlayer : GameObject
    {
        public int AmountOfLives { get; set; } = 2;
        public bool CanPlaceABomb { get; set; } = true;
        private bool m_PlacedBomb =false;
        private bool m_IsDyingAnimationOn = false;
        public double m_DeathAnimationStart;
        private Bomb m_Bomb;
        private double m_BombStartTime;
        private double m_TimeThatBombWasPlaced;
        private ClickReleaseMover m_ClickReleaseMover = new ClickReleaseMover();
        private short m_Pic = 0;
        public BombItPlayer(int i_playerNumber, int i_X, int i_Y, int[,] i_Board)
        {
            DoWeCheckTheObjectForCollision = true;
            ObjectNumber = i_playerNumber;
            m_CanRotateToAllDirections = false;
            m_FlipsWhenMoved = true;
            IsCollisionDetectionEnabled = true;//$"pacman_ghost_{ObjectNumber + 1}.png"
            Board = i_Board;
            this.Initialize(eScreenObjectType.Player, i_playerNumber,"b1dino1.png" , getPointOnGrid(i_X, i_Y), true,
                m_GameInformation.PointValuesToAddToScreen);
            m_ClickReleaseMover.Movable = this as IMovable;
        }

        Point getPointOnGrid(int i_X, int i_Y)
        {
            Point point;
            if (ObjectNumber == 2)
            {
                if (m_GameInformation.AmountOfPlayers == 2)
                {
                    point = new Point(0, i_Y - 1);
                }
                else
                {
                    point = new Point(i_X - 1, 0);
                }
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
                point = new Point(0, 0);
            }

            return point;
        }

        public Point RequestPlaceBomb()
        {
            CanPlaceABomb = false;
            return base.GetPointOnGrid();
        }

        public Bomb PlaceBomb(Point i_Point)
        {
            m_BombStartTime = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            m_PlacedBomb = true;
            m_Bomb = new Bomb(i_Point, Board, ObjectNumber);

            return m_Bomb;
        }

        public override void Update(double i_TimeElapsed)
        {
            if (m_Pic == 0)
            {
                ImageSource = "b1dino1.png";
            }
            else if (m_Pic == 2 || m_Pic == 6)
            {
                ImageSource = "b2dino1.png";
            }
            else if (m_Pic == 4)
            {
                ImageSource = "b3dino1.png";
            }
            else if (m_Pic > 7)
            {
                m_Pic = -1;
            }

            m_Pic++;

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

                if (m_PlacedBomb)
                {
                    double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_BombStartTime;
                    m_Bomb.Update(timePassed);
                    if (timePassed > 3500)
                    {
                        m_PlacedBomb = false;
                        CanPlaceABomb = true;
                    }
                }

                base.Update(i_TimeElapsed);
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Explosion)
            {
                if (!m_IsDyingAnimationOn && m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen) && IsObjectMoving)
                {
                    IsObjectMoving = false;
                    OnSpecialEvent(-1);
                }
            }
            else if (i_Collidable is Boarder)
            {
                collidedWithSolid(i_Collidable);
            }
            else if(i_Collidable is BreakableBoarder)
            {
                collidedWithSolid(i_Collidable);
            }
        }

        public void DeathAnimation(double i_DeathStartTime)
        {
            IsVisable = false;
            m_IsDyingAnimationOn = true;
            if (AmountOfLives == 0 && IsCollisionDetectionEnabled)
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

        public List<Explosion> GetExplosions()
        {
            return m_Bomb.ExplosionsToAdd;
        }
    }
}
