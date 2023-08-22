using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    internal class Explosion : GameObject
    {
        public Explosion(Point i_Point)
        {
            IsCollisionDetectionEnabled = true;
            this.Initialize(eScreenObjectType.Image, 0, "explosion.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        //public override void Update(double i_TimeElapsed)
        //{
        //    if (i_TimeElapsed > 4500)
        //    {
        //        IsVisable = false;
        //        OnDisposed();
        //    }
        //}

        //public override void Collided(ICollidable i_Collidable)
        //{
        //    //if (i_Collidable is PacmanObject)
        //    //{

        //    //}
        //    //else if (i_Collidable is Boarder)
        //    //{
        //    //    collidedWithSolid(i_Collidable);
        //    //}
        //}
    }
}
