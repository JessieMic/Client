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
    }
}
