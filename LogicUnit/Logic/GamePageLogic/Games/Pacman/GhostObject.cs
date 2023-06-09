using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class GhostObject : GameObject
    {
        private int[,] m_Board;

        public GhostObject(ref int[,] i_Board)
        {
            m_Board = i_Board;
        }
    }
}
