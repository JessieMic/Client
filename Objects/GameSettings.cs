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
        public const int m_GameBoardGridSize = 45;
        public static SizeDTO m_MovementButtonOurSize = new SizeDTO(m_GameBoardGridSize, m_GameBoardGridSize);
        public const int m_SpacingAroundButtons = 10;
        public const int m_UIBackgroundHeight = m_GameBoardGridSize * 3;
        public static int m_ID = 0;
        public static SizeDTO m_HeartSize = new SizeDTO((int)(m_GameBoardGridSize * 1.2), (int)(m_GameBoardGridSize*1.2));
        public static SizeDTO m_PauseMenuOurSize = new SizeDTO(210, 245);
        public static SizeDTO m_PauseMenuButtonOurSize = new SizeDTO(175,35);
        public const int ControllBoardTotalHeight = m_GameBoardGridSize * 3 + 2 * m_SpacingAroundButtons;
        public static SizeDTO UIBackgroundSize = new SizeDTO(3000, ControllBoardTotalHeight);

        public static int getID()
        {
            int res = m_ID;
            m_ID++;
            return res;
        }
    }
}
