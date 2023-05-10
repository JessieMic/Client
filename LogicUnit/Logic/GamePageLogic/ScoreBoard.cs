using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic
{
    public class ScoreBoard
    {
        public List<string> m_LoseOrder = new List<string>();
        public int[] m_PlayersScore = new int[4];
        public bool m_ShowScoreBoardByOrder;//if false order will be by score

        public void ShowScoreBoard()
        {

        }
    }
}
