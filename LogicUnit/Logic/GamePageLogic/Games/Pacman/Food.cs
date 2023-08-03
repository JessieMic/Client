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
        public override bool IsCollisionDetectionEnabled => true;
        public Food(Point i_Point)
        {
            ObjectNumber = 1;
            this.Initialize(eScreenObjectType.Image, 1, "pacmanfood.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
            m_Size.Height = m_Size.Width = GameSettings.GameGridSize/5;
            centerObjectInGrid();
        }

        public override void Update(double i_TimeElapsed)
        {
            base.Update(i_TimeElapsed);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            OnDisposed();
        }
    }
}
