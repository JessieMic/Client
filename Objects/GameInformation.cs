using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
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
        private List<ScreenDimension> m_ScreenInfoOfAllPlayers = new List<ScreenDimension>();
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

        public void SetScreenInfo(string[] i_NamesOfPlayers, SizeDTO[] i_ScreenSizes)
        {
            m_NamesOfAllPlayers = i_NamesOfPlayers;

            ////Size s = new Size(m_ClientScreenDimension.Size.m_Width+120, m_ClientScreenDimension.Size.m_Height);

            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                //if(true)//(i == 1)
                //{
                m_ScreenInfoOfAllPlayers.Add(new ScreenDimension(m_ClientScreenDimension.SizeDTO, new Position(m_AmountOfPlayers, i + 1)));
                //}
                //else
                //{
                //    m_ScreenInfoOfAllPlayers.Add(new ScreenDimension(s, new Position(m_AmountOfPlayers, i + 1)));
                //}

                if (m_Player.ButtonThatPlayerPicked == i + 1)
                {
                    m_ClientScreenDimension = m_ScreenInfoOfAllPlayers[i];
                }
            }
        }

        public List<ScreenDimension> ScreenInfoOfAllPlayers
        {
            get { return m_ScreenInfoOfAllPlayers; }
        }


        public int AmountOfPlayers
        {
            get { return m_AmountOfPlayers; }
            set { m_AmountOfPlayers = value; }
        }

        public eGames NameOfGame
        {
            get
            {
                return m_NameOfGame;
            }
            set
            {
                m_NameOfGame = value;
            }
        }
    }
}
