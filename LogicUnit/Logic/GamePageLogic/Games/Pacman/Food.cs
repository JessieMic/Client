using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class Food : GameObject
    {
        bool m_IsFoodOnScreen = false;
        private int m_FoodCounter;

        public Food(Point i_Point, ref int i_ScreenFoodCounter)
        {
            IsCollisionDetectionEnabled = true;
            ObjectNumber = 1;
            this.Initialize(eScreenObjectType.Image, 1, "pacmanfood.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
            m_Size.Height = m_Size.Width = GameSettings.GameGridSize / 5;
            centerObjectInGrid();

            if (m_GameInformation.IsPointIsOnBoardPixels(PointOnScreen))
            {
                m_GameInformation.m_food++;
                //i_ScreenFoodCounter++;
                m_IsFoodOnScreen = true;
            }
            m_FoodCounter = i_ScreenFoodCounter;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (m_IsFoodOnScreen)
            {
                m_GameInformation.m_food--;
                if (m_GameInformation.m_food == 0)
                {
                    OnSpecialEvent((int)ePacmanSpecialEvents.ClearFoodOnPlayerScreen);
                }
            }

            OnDisposed();
        }
    }
}