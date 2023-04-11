using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects.Enums;

namespace POC_Client.Objects
{
    public class GameInformation
    {
        public bool isInitialized = false;
        private static GameInformation m_Instance = null;
        public eGames m_NameOfGame;
        private int m_AmountOfPlayers;
        private Player m_Player = Player.Instance;
        public ScreenDimension ClientScreenDimension = new ScreenDimension();
        private List<ScreenDimension> m_ScreenInfoOfAllPlayers;
        public List<string> m_NamesOfAllPlayers;

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

        public List<ScreenDimension> ScreenInfoOfAllPlayers
        {
            get {return m_ScreenInfoOfAllPlayers;}
            set
            {
                m_ScreenInfoOfAllPlayers = value;
                ClientScreenDimension = m_ScreenInfoOfAllPlayers[m_Player.ButtonThatPlayerPicked - 1];
            }
        }


        public int AmountOfPlayers
        {
            get { return m_AmountOfPlayers; }
            set { m_AmountOfPlayers = value; }
        }
    }
}
