using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    internal class Cherry : GameObject
    {
        public Cherry(Point i_Point)
        {
            MonitorForCollision = true;
            this.Initialize(eScreenObjectType.Image, 1, "pacman_cherry.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
            //Size.Height = Size.Width = GameSettings.GameGridSize / 5;
            //centerObjectInGrid();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            OnDisposed();
        }
    }
}
