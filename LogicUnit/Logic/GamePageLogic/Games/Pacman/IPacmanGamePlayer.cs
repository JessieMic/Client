using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    internal interface IPacmanGamePlayer
    {
        public bool IsHunting { get; set; }
        public int AmountOfLives { get; set; }
        public void InitiateCherryTime(double i_BerryStartTime);
        public void ResetPosition(double i_DeathStartTime);
    }
}
