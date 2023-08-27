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
    public class Boarder : GameObject
    {
        public Boarder(Point i_Point,string i_Png)
        {
            MonitorForCollision = true;
            ObjectNumber = 2;
            this.Initialize(eScreenObjectType.Image, 2, "pacman_boarder.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }
    }
}