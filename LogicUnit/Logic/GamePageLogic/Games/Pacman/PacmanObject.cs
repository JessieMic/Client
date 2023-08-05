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
        public Notify AteBerry;
        public double m_CherryTimeStart;
        public int AmountOfLives { get; set; } = 2;
        public bool IsHunting { get; set; } = false;
        public double m_DeathAnimationStart;
        private bool m_IsDyingAnimationOn = false;
        private bool m_IsCherryTime = false;
        public PacmanObject(int[,] i_Board)
        {
            m_Board = i_Board;
            IsCollisionDetectionEnabled = true;
            this.Initialize(eScreenObjectType.Player,1, "pacman.gif", new Point(0,0),true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
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
            if(i_Collidable is GhostObject)
            {
                if(!IsHunting)//Got eaten
                {
                    IsObjectMoving = false;
                    m_IsDyingAnimationOn = true;
                    OnGotHit();
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
            else if(i_Collidable is Cherry)
            {
                i_Collidable.Collided(this);
                AteBerry.Invoke();
            }
        }

        public void ResetPosition(double i_DeathStartTime)
        {
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
