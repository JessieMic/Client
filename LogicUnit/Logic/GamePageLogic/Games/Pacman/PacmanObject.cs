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

        private int i = 0;
        public PacmanObject(int ob, int[,] i_Board)
        {
            m_Board = i_Board;
            ObjectNumber = ob;
            this.Initialize(eScreenObjectType.Player,ob, "pacman.gif", new Point(0,0),true,
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
                collidedWithSolid(i_Collidable);
                Direction = RequestedDirection;
            }
        }

    }
}
