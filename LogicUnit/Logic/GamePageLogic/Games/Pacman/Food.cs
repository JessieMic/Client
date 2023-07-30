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
        public Food()
        {
            ObjectNumber = 2;
            this.Initialize(eScreenObjectType.Player, 2, "boarder.png", new Point(3, 5), true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            base.Update(i_TimeElapsed);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is PacmanObject)
            {
                Rotatation += 20;
            }
        }
    }
}
