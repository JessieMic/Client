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
    public class PacmanObject : GameObject
    {
        public override bool IsCollisionDetectionEnabled => true;

        public PacmanObject()
        {
            ObjectNumber = 1;
            this.Initialize(eScreenObjectType.Player,1, "boarder.png", new Point(1,1),true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            base.Update(i_TimeElapsed);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if(i_Collidable is GhostObject)
            {
                //reset
            }
            else if (i_Collidable is Food)
            {
                i_Collidable.Collided(this);
                //points up
            }
            else if (i_Collidable is Boarder)
            {
                collidedWithSolid(i_Collidable.Bounds);
            }
        }

    }
}
