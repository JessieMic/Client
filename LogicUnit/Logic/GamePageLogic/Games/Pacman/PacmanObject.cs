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
        public PacmanObject()
        {
            ObjectNumber = 1;
            this.Initialize(eScreenObjectType.Player,1,"heart.png",new Point(1,1),true,
                m_GameInformation.PointValuesToAddToScreen);
        }

    }
}
