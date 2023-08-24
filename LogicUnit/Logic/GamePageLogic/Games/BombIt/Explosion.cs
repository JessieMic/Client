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
    }
}
