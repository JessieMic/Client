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
        public const int GameGridSize = 50;
        public static SizeDTO m_MovementButtonOurSize = new SizeDTO(GameGridSize, GameGridSize);
        public const int m_SpacingAroundButtons = 10;
        public const int m_UIBackgroundHeight = GameGridSize * 3;
        public static int m_ID = 0;
        public static SizeDTO m_HeartSize = new SizeDTO((int)(GameGridSize * 1.2), (int)(GameGridSize*1.2));
        public static SizeDTO m_PauseMenuOurSize = new SizeDTO(GameGridSize*6, GameGridSize*5);
        public static SizeDTO m_PauseMenuButtonOurSize = new SizeDTO(GameGridSize*5, GameGridSize);
        public const int ControllBoardTotalHeight = GameGridSize * 3 + 2 * m_SpacingAroundButtons;
        public static SizeDTO UIBackgroundSize = new SizeDTO(3000, ControllBoardTotalHeight);
        

        public static int getID()
        {
          
                int res = m_ID;
                m_ID++;
                return res;
            
        }
    }
}
