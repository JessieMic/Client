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
            this.Initialize(eScreenObjectType.Image, m_GameInformation.Player.PlayerNumber, "pacmanfood.png", point, true,
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
               // RefreshOtherClientsAboutPosition();
            }
            else if (m_IsCoolDownTimeStarted)
            {
                double timePassed = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_StartTimeOfCoolDownTime;
                if(timePassed > 2000)
                {
                    m_IsCoolDownTimeStarted = false;
                    IsObjectMoving = false;
                }
            }
            //else if (m_GameInformation.Player.PlayerNumber == 1)
            //{
            //    if (m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds - m_TimeFromLastUpdate > 3000
            //       && m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))
            //    {
            //        m_TimeFromLastUpdate = m_GameInformation.RealWorldStopwatch.Elapsed.TotalMilliseconds;
            //        OnSpecialEvent(-1);
                   
            //    }
            //}


            m_TimesGotHit++;
        }

        //private void RefreshOtherClientsAboutPosition()
        //{
        //    if(IsObjectMoving)
        //    {
        //        if(m_TimesGotHit > 4)
        //        {
        //            m_TimesGotHit = 0;
        //            if(m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))
        //            {
        //                OnSpecialEvent(-1);
        //            }
        //        }
        //    }
        //}

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
                    System.Diagnostics.Debug.WriteLine("1GGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
                    IsObjectMoving = false;
                }
                else
                {
                    yDirectionBounceFactor *= -1;
                }
            }

            if (PointOnScreen.Row >= m_GameInformation.GameBoardSizeByPixel.Height + m_ValuesToAdd.Row - 45)
            {
                if (isPointOnScreen)
                {
                    OnSpecialEvent(2);
                    System.Diagnostics.Debug.WriteLine("2GGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
                    IsObjectMoving = false;
                }
                else
                {
                    yDirectionBounceFactor *= -1;
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
