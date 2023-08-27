using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;

namespace LogicUnit.Logic.GamePageLogic
{
    public class ScoreBoard
    {
        public List<string> m_LoseOrder = new List<string>();
        public int[] m_PlayersScore = new int[4];
        public bool m_ShowScoreBoardByOrder;//if false order will be by score
        public GameObject Label { get; set; }

        public void ShowScoreBoard(string i_Text,PauseMenu i_Menu, GameObject i_Label)
        {
            i_Menu.ShowEndGameMenu();
            i_Label.Text = i_Text;
            i_Label.ScreenObjectType = eScreenObjectType.Label;
            i_Label.OnUpdate();
        }
    }
}
