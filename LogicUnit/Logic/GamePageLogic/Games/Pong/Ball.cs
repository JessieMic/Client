using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Pong
{
    internal class Ball : GameObject
    {
        int xDirectionBounceFactor;
        int yDirectionBounceFactor;
        private int m_StartUpDirection = 1;
        private double m_StartTimeOfCoolDownTime = 0;
        private bool m_IsCoolDownTimeStarted = false;
        private int m_TimesGotHit = 0;
        private double m_TimeFromLastUpdate = 0;

        public Ball()
        {              
                Point point = new Point(
                m_GameInformation.GameBoardSizeByGrid.Width/2,
                m_GameInformation.GameBoardSizeByGrid.Height/2);
            MonitorForCollision = true;
            IsObjectMoving = true;
            this.Initialize(eScreenObjectType.Image, m_GameInformation.Player.PlayerNumber, "pong_ball.png", point, true,
                m_GameInformation.PointValuesToAddToScreen);
            xDirectionBounceFactor= yDirectionBounceFactor=Velocity = 120;
        }

        public override void Update(double i_TimeElapsed)
        {
            if(IsObjectMoving)
            {
                updatePoint(i_TimeElapsed);
                bounceFromHittingSidesOfGameBoard();
                checkIfLost();
            }
            else if (m_IsCoolDownTimeStarted)
            {
                double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_StartTimeOfCoolDownTime;
                if(timePassed > 2000)
                {
                    m_IsCoolDownTimeStarted = false;
                    IsObjectMoving = true;
                }
            }

            if (m_GameInformation.Player.PlayerNumber == 1)
            {
                if (m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_TimeFromLastUpdate > 3000
                   && m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))
                {
                    System.Diagnostics.Debug.WriteLine("WOWOW");
                    m_TimeFromLastUpdate = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
                    OnSpecialEvent(-1);

                }
            }
        }

        private void bounceFromHittingSidesOfGameBoard()
        {
            if (PointOnScreen.Column <= m_ValuesToAdd.Column)
            {
                xDirectionBounceFactor = Velocity;
            }

            if (PointOnScreen.Column >= m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - 45)
            {
                xDirectionBounceFactor = -Velocity;
            }
        }

        private void updatePoint(double i_TimeElapsed)
        {
            Point newPoint = PointOnScreen;

            newPoint.Column += xDirectionBounceFactor * i_TimeElapsed / 1000;
            newPoint.Row += yDirectionBounceFactor * i_TimeElapsed / 1000;
            PointOnScreen = newPoint;
        }

        private void checkIfLost()
        {
            bool isPointOnScreen = m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen);
            
            if (PointOnScreen.Row <= m_ValuesToAdd.Row)
            {
                if (isPointOnScreen)
                {
                    OnSpecialEvent(2);
                    IsObjectMoving = false;
                }
                else
                {
                    yDirectionBounceFactor = Velocity;
                }
            }

            if (PointOnScreen.Row >= m_GameInformation.GameBoardSizeByPixel.Height + m_ValuesToAdd.Row - 45)
            {
                if (isPointOnScreen)
                {
                    OnSpecialEvent(2);
                    IsObjectMoving = false;
                }
                else
                {
                    yDirectionBounceFactor = -Velocity;
                }
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (m_GameInformation.ScreenInfoOfAllPlayers[i_Collidable.ObjectNumber - 1].Position.Row == eRowPosition.UpperRow)
            {
                yDirectionBounceFactor = 120;
            }
            else
            {
                yDirectionBounceFactor = -120;
            }
        }

        public void Reset()
        {
            m_TimeFromLastUpdate = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            m_TimesGotHit = 0;
            IsObjectMoving = false;
            m_StartTimeOfCoolDownTime = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            m_IsCoolDownTimeStarted = true;
            m_StartUpDirection *= -1;
            xDirectionBounceFactor = Velocity * m_StartUpDirection;
            yDirectionBounceFactor = Velocity * m_StartUpDirection;
            resetToStartupPoint();
        }
    }
}
