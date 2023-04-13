using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace Objects
{
    public class GameInformation
    {
        public bool isInitialized = false;
        private static GameInformation m_Instance = null;
        public eGames m_NameOfGame;
        private int m_AmountOfPlayers;
        private Player m_Player = Player.Instance;
        public ScreenDimension m_ClientScreenDimension = new ScreenDimension();
        private List<ScreenDimension> m_ScreenInfoOfAllPlayers =  new List<ScreenDimension>();
        public string[] m_NamesOfAllPlayers;

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

        public void SetScreenInfo(string[] i_NamesOfPlayers, Size[] i_ScreenSizes)
        {
            m_NamesOfAllPlayers = i_NamesOfPlayers;
        
            for(int i = 0; i < m_AmountOfPlayers; i++)
            {
                m_ScreenInfoOfAllPlayers.Add(new ScreenDimension(m_ClientScreenDimension.Size, new Position(m_AmountOfPlayers,i+1)));

                if (m_Player.ButtonThatPlayerPicked == i + 1)
                {
                    m_ClientScreenDimension = m_ScreenInfoOfAllPlayers[i];
                }
            }
        }

        public List<ScreenDimension> ScreenInfoOfAllPlayers
        {
            get {return m_ScreenInfoOfAllPlayers;}
        }


        public int AmountOfPlayers
        {
            get { return m_AmountOfPlayers; }
            set { m_AmountOfPlayers = value; }
        }
    }
}
