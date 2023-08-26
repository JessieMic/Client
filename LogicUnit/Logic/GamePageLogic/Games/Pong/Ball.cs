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
        public Ball()
        {
            Point point = new Point(
                m_GameInformation.GameBoardSizeByPixel.Width,
                m_GameInformation.GameBoardSizeByPixel.Height);
            IsCollisionDetectionEnabled = true;
            this.Initialize(eScreenObjectType.Image, 0, $"bomb.png", point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            int xDirectionBounceFactor=0;
            int yDirectionBounceFactor=0;
            
            if (PointOnScreen.Column >= m_GameInformation.GameBoardSizeByPixel.Width - 45)
            {
                xDirectionBounceFactor = -50;
            }
            //if ball's 'x' cordinate reaches start of the screen width
            if (PointOnScreen.Column <= 10)
            {
                xDirectionBounceFactor = 50;
            }
            //if ball's 'y' cordinate reaches end of the screen height
            if (PointOnScreen.Row >= m_GameInformation.GameBoardSizeByPixel.Height - 90)
            {
                yDirectionBounceFactor = -50;
            }
            //if ball's 'x' cordinate reaches start of the screen height
            if (PointOnScreen.Row <= 10) //if (ballYPosition <= -(screenHeight / 2)+75)
            {
                yDirectionBounceFactor = 50;
            }
            Point newPoint = PointOnScreen;
            newPoint.Column += xDirectionBounceFactor * i_TimeElapsed / 1000;
            newPoint.Row += yDirectionBounceFactor * i_TimeElapsed / 1000;

            isPointOnBoard(ref newPoint);
            PointOnScreen = newPoint;
        }

    }
    
}
