using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public class GameInformation
    {
        public bool isInitialized = false;
        private static GameInformation m_Instance = null;
        public eGames m_NameOfGame;
        private int m_AmountOfPlayers;
        public List<ScreenDimension> ScreenInfoOfAllPlayers;
        public List<string> NamesOfAllPlayers;

        private static readonly object s_InstanceLock = new object();

        public static GameInformation Instance
        {
            get
            {
                lock (s_InstanceLock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new GameInformation();
                    }
                }
                return m_Instance;
            }
        }

        public int AmountOfPlayers
        {
            get { return m_AmountOfPlayers; }
            set { m_AmountOfPlayers = value; }
        }
    }
}
