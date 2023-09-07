using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public static class GameSettings
    {
        public const int GameGridSize = 50;
        public static SizeD m_MovementButtonOurSize = new SizeD(GameGridSize, GameGridSize);
        public const int m_SpacingAroundButtons = 10;
        public const int m_UIBackgroundHeight = GameGridSize * 3;
        public static int m_ID = 0;
        public static SizeD m_HeartSize = new SizeD((int)(GameGridSize * 1.2), (int)(GameGridSize*1.2));
        public static SizeD m_PauseMenuOurSize = new SizeD(GameGridSize*6, GameGridSize*5);
        public static SizeD m_PauseMenuButtonOurSize = new SizeD(GameGridSize*5, GameGridSize);
        public const int ControllBoardTotalHeight = GameGridSize * 3 + 2 * m_SpacingAroundButtons;
        public static SizeD UIBackgroundSize = new SizeD(3000, ControllBoardTotalHeight);
        

        public static int getID()
        {
          
                int res = m_ID;
                m_ID++;
                return res;
            
        }
    }
}
