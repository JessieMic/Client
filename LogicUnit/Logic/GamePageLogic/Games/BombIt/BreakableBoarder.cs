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
        private bool m_IsBroken = false;

        public BreakableBoarder(Point i_Point)
        {
            Random random = new Random();

            MonitorForCollision = true;
            DoWeCheckTheObjectForCollision = true;
            this.Initialize(eScreenObjectType.Image,0, $"mushroom{random.Next(1,3)}.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Explosion)
            {
                if(!m_IsBroken)
                {
                    m_IsBroken = true;
                    ObjectNumber = i_Collidable.ObjectNumber;
                    ImageSource = $"mushroom_buff_{ObjectNumber}.png";
                    OnUpdate();
                }
                //if(ObjectNumber == -(int)eBombItBuffs.NoBuff)
                //{
                  // OnDisposed();
                //}
                //else
                //{
                //    OnSpecialEvent(i_Collidable.ObjectNumber);
                //}
            }
        }
    }
}
