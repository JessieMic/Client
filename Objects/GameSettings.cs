using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public static class GameSettings
    {
        public static Size m_MovementButtonSize = new Size(35, 35);
        public static int m_GameBoardGridSize = 35;
        public static int m_SpacingAroundButtons = 10;
        public static int m_ID = 0;
        public static Size m_PauseMenuSize = new Size(210, 245);
        public static Size m_PauseMenuButtonSize = new Size(175,35);

        public static int getID()
        {
            int res = m_ID;
            m_ID++;
            return res;
        }
    }
}
