using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;

namespace LogicUnit
{
    public class Snake : Game
    {
        public Snake()
        {
            
        }

        public override void RunGame()
        {

        }

        private void initializeGame()
        {
            m_TypeOfGameButtons = eTypeOfGameButtons.MovementButtonsForAllDirections;
            m_AmountOfLivesTheClientHas = 3;
        }
    }
}
