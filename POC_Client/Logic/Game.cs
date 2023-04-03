using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects;

namespace POC_Client.Logic
{
    public abstract class Game
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        private Player m_Player = Player.Instance;
        protected ScreenMapping m_ScreenMapping = new ScreenMapping();
        public void RunGame()
        {

        }

    }
}
