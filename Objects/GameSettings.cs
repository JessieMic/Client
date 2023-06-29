using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Objects
{
    public static class GameSettings
    {
        public static SizeDTO m_MovementButtonOurSize = new SizeDTO(35, 35);
        public const int m_GameBoardGridSize = 35;
        public const int m_SpacingAroundButtons = 10;
        public static int m_ID = 0;
        public static SizeDTO m_PauseMenuOurSize = new SizeDTO(210, 245);
        public static SizeDTO m_PauseMenuButtonOurSize = new SizeDTO(175,35);

        public static int getID()
        {
            int res = m_ID;
            m_ID++;
            return res;
        }
    }
}
