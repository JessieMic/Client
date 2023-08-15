using Objects;
using Objects.Enums.BoardEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class PacmanObject : GameObject, IPacmanGamePlayer
    {
        public double m_CherryTimeStart;
        public int AmountOfLives { get; set; } = 2;
        public bool IsHunting { get; set; } = false;
        public double m_DeathAnimationStart;
        private bool m_IsDyingAnimationOn = false;
        private bool m_IsCherryTime = false;
        private short m_Pic = 0;
        public PacmanObject(int[,] i_Board)
        {
            m_Board = i_Board;
            IsCollisionDetectionEnabled = true;
            this.Initialize(eScreenObjectType.Player, 1, "pacman1.png", new Point(0, 0), true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            if (m_Pic == 0)
            {
                ImageSource = "pacman1.png";
            }
            else if (m_Pic == 3 || m_Pic == 9)
            {
                ImageSource = "pacman2.png";
            }
            else if (m_Pic == 6)
            {
                ImageSource = "pacman3.png";
            }
            else if (m_Pic > 11)
            {
                m_Pic = -1;
            }

            m_Pic++;
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

            if (m_IsCherryTime)
            {
                double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_CherryTimeStart;

                if (timePassed > 7000)
                {
                    m_IsCherryTime = false;
                    IsHunting = false;
                }
            }
            base.Update(i_TimeElapsed);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is GhostObject)
            {
                if (!IsHunting && IsObjectMoving)//Got eaten
                {
                    IsObjectMoving = false;
                    if (m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))
                    {
                        Thread t = Thread.CurrentThread;
                        System.Diagnostics.Debug.WriteLine(m_GameInformation.m_Player.PlayerNumber + " AAAAAA " + t.ManagedThreadId + " " + Thread.GetCurrentProcessorId());
                        OnSpecialEvent((int)ePacmanSpecialEvents.GotHit);
                    }
                    else
                    {
                        //IsObjectMoving = true;
                    }
                }
            }
            else if (i_Collidable is Food)
            {
                i_Collidable.Collided(this);
                //points up
            }
            else if (i_Collidable is Boarder)
            {
                collidedWithSolid(i_Collidable);
            }
            else if (i_Collidable is Cherry)
            {
                i_Collidable.Collided(this);
                OnSpecialEvent((int)ePacmanSpecialEvents.AteCherry);
            }
        }

        public void ResetPosition(double i_DeathStartTime)
        {
            IsObjectMoving = false;
            m_IsDyingAnimationOn = true;
            m_DeathAnimationStart = i_DeathStartTime;
            resetToStartupPoint();
        }

        public void InitiateCherryTime(double i_BerryStartTime)
        {
            m_IsCherryTime = true;
            IsHunting = true;
            m_CherryTimeStart = i_BerryStartTime;
        }
    }
}