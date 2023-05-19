using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace LogicUnit.Logic.GamePageLogic
{
    public class Hearts
    {
        public int[] m_AmountOfLivesPlayerHas = new int[4];
        public int m_AmountOfLivesPlayersGetAtStart = 3;
        public int m_AmountOfPlayersThatAreAlive;
        public int m_AmountOfPlayers;
        public int m_ClientNumber; 

        public eGameStatus m_GameStatus;
        public List<string> m_LoseOrder;

        public void setHearts(int i_AmountOfPlayers,ref eGameStatus i_Status, ref List<string> i_LoseList)
        {
            m_GameStatus = i_Status;
            m_LoseOrder = i_LoseList;
            m_AmountOfPlayers = m_AmountOfPlayersThatAreAlive = i_AmountOfPlayers;
            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                m_AmountOfLivesPlayerHas[i] = m_AmountOfLivesPlayersGetAtStart;
            }
        }

        public eGameStatus setPlayerLifeAndGetGameStatus(int i_Player)
        {
            eGameStatus returnStatus = eGameStatus.Running;
            bool isGameRunning = false;

            m_AmountOfLivesPlayerHas[i_Player - 1]--;

            if (i_Player == m_ClientNumber)
            {
                removeAHeart();
            }

            if (m_AmountOfLivesPlayerHas[i_Player - 1] == 0)
            {
                m_AmountOfPlayersThatAreAlive--;

                if (m_AmountOfPlayersThatAreAlive > 1)//Player lost but game is still running
                {
                    returnStatus = eGameStatus.Lost;
                }
                else//only one player is alive so the game has ended 
                {
                    returnStatus = eGameStatus.Ended;
                }
            }

            return returnStatus;
        }

        private void removeAHeart()
        {

        }
    }
}
