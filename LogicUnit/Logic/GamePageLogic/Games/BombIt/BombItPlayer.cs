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
        private bool m_IsSteppingOnAbomb = false;
        private Point m_BombPointWeAreSteppingOn =new Point();
        private ClickReleaseMover m_ClickReleaseMover = new ClickReleaseMover();

        public BombItPlayer(int i_playerNumber, int i_X, int i_Y, int[,] i_Board)
        {
            ObjectNumber = i_playerNumber;
            m_CanRotateToAllDirections = false;
            m_FlipsWhenMoved = true;
            IsCollisionDetectionEnabled = true;
            Board = i_Board;
            this.Initialize(eScreenObjectType.Player, i_playerNumber, $"pacman_ghost_{ObjectNumber + 1}.png", getPointOnGrid(i_X, i_Y), true,
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
            if (AmountOfLives != 0)
            {
                //if (m_IsSteppingOnAbomb)
                //{
                //    Point p = base.GetPointOnGrid();
                //    if (m_BombPointWeAreSteppingOn != p)
                //    {
                //        m_IsSteppingOnAbomb = false;
                //    }
                //}

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
                Rotatation += 20;
            }
            else if (i_Collidable is Bomb)
            {
                if (i_Collidable.ObjectNumber != ObjectNumber)//m_Bomb == null || i_Collidable.PointOnScreen != m_Bomb.PointOnScreen)
                {
                    if(m_IsSteppingOnAbomb)
                    {
                        double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_TimeThatBombWasPlaced;
                        if (timePassed > 3700)
                        {
                            m_IsSteppingOnAbomb = false;
                        }
                    }
                    else
                    { 
                        collidedWithSolid(i_Collidable);
                    }
                }
            }
            else if (i_Collidable is Boarder)
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

        public void CheckIfOnBomb(int i_BombNumber,Point i_Point)
        {
            if (ObjectNumber != i_BombNumber && i_Point == base.GetPointOnGrid())
            {
                m_BombPointWeAreSteppingOn.Row = i_Point.Row;
                m_BombPointWeAreSteppingOn.Column = i_Point.Column;
                m_TimeThatBombWasPlaced = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
                m_IsSteppingOnAbomb = true;
            }
        }

        public List<Explosion> GetExplosions()
        {
            return m_Bomb.m_Explosions;
        }

        //protected override void collidedWithSolid(ICollidable i_Solid)//(Point i_PointOfSolid,SizeInPixelsDto i_SizeOfSolid)
        //{
        //    Point newPoint = PointOnScreen;
        //    if (Direction == Direction.Right)
        //    {
        //        newPoint.Column = i_Solid.PointOnScreen.Column + i_Solid.Bounds.Width;
        //    }
        //    else if (Direction == Direction.Left) 
        //    {
        //        newPoint.Column = i_Solid.PointOnScreen.Column - Size.Width;
        //    }
        //    else if (Direction == Direction.Down)
        //    {
        //        newPoint.Row = i_Solid.PointOnScreen.Row + i_Solid.Bounds.Height;
        //    }
        //    else 
        //    {
        //        newPoint.Row = i_Solid.PointOnScreen.Row - Size.Height;
        //    }
        //    PointOnScreen = newPoint;
        //}
    }
}
