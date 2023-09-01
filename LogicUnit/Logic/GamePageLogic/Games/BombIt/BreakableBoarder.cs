using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    public class BreakableBoarder : GameObject
    {
        public BreakableBoarder(Point i_Point)
        {
            Random random = new Random();

            MonitorForCollision = true;
            DoWeCheckTheObjectForCollision = true;
            ObjectNumber = 2;
            this.Initialize(eScreenObjectType.Image, 2, $"mushroom{random.Next(1,3)}.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Explosion)
            {
                OnDisposed();
            }
        }
    }
}
