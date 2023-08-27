using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic
{
    public class ScoreBoard
    {
        public List<string> m_LoseOrder = new List<string>();
        public int[] m_PlayersScore = new int[4];
        public bool m_ShowScoreBoardByOrder;//if false order will be by score
        protected GameInformation m_GameInformation = GameInformation.Instance;
        private GameObject ScoreLabel { get; set; }
        public GameObject Label { get; set; }

        public ScoreBoard(PauseMenu i_Menu)
        {
            setLabel(i_Menu.getPauseMenuBackgroundPoint());
        }

        public void ShowScoreBoard(string i_Text, PauseMenu i_Menu)
        {
            i_Menu.ShowEndGameMenu();
            Label.Text = i_Text;
            Label.OnUpdate();
        }

        void setLabel(Point i_Point)
        {
            Label = new GameObject();
            int colum = (int)(i_Point.Column + GameSettings.GameGridSize * 0.5);

            Label.Size = GameSettings.m_PauseMenuButtonOurSize;
            Label.ScreenObjectType = eScreenObjectType.Label;
            Label.Initialize();
            if (m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                Label.SetImageDirection(Direction.getDirection("up"));
                Label.PointOnScreen= (new Point(colum, (int)(i_Point.Row + GameSettings.GameGridSize * 3.5)));
            }
            else
            {
                Label.PointOnScreen = (new Point(colum, (int)(i_Point.Row + GameSettings.GameGridSize * 0.5)));
            }
        }
    }
}
