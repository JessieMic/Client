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
        int xDirectionBounceFactor=220;
        int yDirectionBounceFactor=220;
        public Ball()
        {              
                Point point = new Point(
                m_GameInformation.GameBoardSizeByGrid.Width/2,
                m_GameInformation.GameBoardSizeByGrid.Height/2);
            MonitorForCollision = true;
            this.Initialize(eScreenObjectType.Image, 0, "pacmanfood.png", point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            Point newPoint = PointOnScreen;
            newPoint.Column += xDirectionBounceFactor * i_TimeElapsed / 1000;
            newPoint.Row += yDirectionBounceFactor * i_TimeElapsed / 1000;

            //isPointOnBoard(ref newPoint);
            PointOnScreen = newPoint;

            //if (PointOnScreen.Column  <= m_ValuesToAdd.Column || PointOnScreen.Column >= m_GameInformation.GameBoardSizeByPixel.Height+ m_ValuesToAdd.Column - 45)
            //{
            //    xDirectionBounceFactor = -xDirectionBounceFactor;
            //}

            //if (PointOnScreen.Row <= m_ValuesToAdd.Row  || PointOnScreen.Column >= m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Row - 45)
            //{
            //    xDirectionBounceFactor = -xDirectionBounceFactor;
            //}

            if (PointOnScreen.Column <= m_ValuesToAdd.Column )
            {
                xDirectionBounceFactor = 220;
            }

            if (PointOnScreen.Column >= m_GameInformation.GameBoardSizeByPixel.Width + m_ValuesToAdd.Column - 45)
            {
                xDirectionBounceFactor = -220;
            }

            if (PointOnScreen.Row <= m_ValuesToAdd.Row)
            {
                yDirectionBounceFactor = 220;
            }

            if (PointOnScreen.Row>= m_GameInformation.GameBoardSizeByPixel.Height + m_ValuesToAdd.Row - 45)
            {
                yDirectionBounceFactor = -220;
            }

            ////if ball's 'x' cordinate reaches start of the screen width
            //if (PointOnScreen.Column <= 10)
            //{
            //    xDirectionBounceFactor = 50;
            //}
            ////if ball's 'y' cordinate reaches end of the screen height
            //if (PointOnScreen.Row >= m_GameInformation.GameBoardSizeByPixel.Height - 45)
            //{
            //    yDirectionBounceFactor = -50;
            //}
            ////if ball's 'x' cordinate reaches start of the screen height
            //if (PointOnScreen.Row <= 45) //if (ballYPosition <= -(screenHeight / 2)+75)
            //{
            //    yDirectionBounceFactor = 50;
            //}



        }

    }
    
}
