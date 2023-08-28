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

        public Ball()
        {              
                Point point = new Point(
                m_GameInformation.GameBoardSizeByGrid.Width/2,
                m_GameInformation.GameBoardSizeByGrid.Height/2);
            MonitorForCollision = true;
            IsObjectMoving = true;
            this.Initialize(eScreenObjectType.Image, 0, "pacmanfood.png", point, true,
                m_GameInformation.PointValuesToAddToScreen);
            xDirectionBounceFactor= yDirectionBounceFactor=Velocity = 120;
        }

        public override void Update(double i_TimeElapsed)
        {
            Point newPoint = PointOnScreen;
            bool isPointOnScreen;

            if(IsObjectMoving)
            {
                newPoint.Column += xDirectionBounceFactor * i_TimeElapsed / 1000;
                newPoint.Row += yDirectionBounceFactor * i_TimeElapsed / 1000;
                PointOnScreen = newPoint;
                isPointOnScreen = m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen);

                if (PointOnScreen.Column <= m_ValuesToAdd.Column)
                {
                    xDirectionBounceFactor = Velocity;
                }

                if (PointOnScreen.Column >= m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - 45)
                {
                    xDirectionBounceFactor = -Velocity;
                }


                if (PointOnScreen.Row <= m_ValuesToAdd.Row)
                {
                    if (isPointOnScreen)
                    {
                       // OnSpecialEvent(1);
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
                        //OnSpecialEvent(2);
                        IsObjectMoving = false;
                    }
                    else
                    {
                        yDirectionBounceFactor = -Velocity;
                    }
                }
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            yDirectionBounceFactor *= -1;
            //if(i_Collidable.ObjectNumber == 2)
            //{
            //    yDirectionBounceFactor = -Velocity;
            //}
            //else
            //{
            //    yDirectionBounceFactor = Velocity;
            //}
        }

        public void Reset()
        {
            m_StartUpDirection *= -1;
            xDirectionBounceFactor = Velocity * m_StartUpDirection;
            yDirectionBounceFactor = Velocity * m_StartUpDirection;
            resetToStartupPoint();
            IsObjectMoving = true;
        }

    }
    
}
