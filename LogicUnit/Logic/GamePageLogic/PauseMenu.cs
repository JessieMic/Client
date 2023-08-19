using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic
{
    public class PauseMenu
    {
        public List<int> m_PauseMenuIDList = new List<int>();
        private GameInformation m_gameInformation = GameInformation.Instance;
        private List<GameObject> m_MenuButtons = new List<GameObject>();
        private GameObject m_PauseMenuBackground;
        public GameObject MenuLabel { get; set; }
        public bool ShowResume { get; set; } = true;
        public void GetPauseMenu(Buttons i_Buttons, ref List<GameObject> o_GameObejectsToAdd)
        {
            GameObject pauseMenu = new GameObject();
            Point menuPoint = getPauseMenuBackgroundPoint();
            pauseMenu.IsVisable = false;
            pauseMenu.Initialize(eScreenObjectType.Image, 0, "pausemenu.png", menuPoint, false, new Point(0, 0));
            pauseMenu.Size = GameSettings.m_PauseMenuOurSize;
            m_PauseMenuIDList.Add(pauseMenu.ID);
            pauseMenu.ZIndex = 0;
            o_GameObejectsToAdd.Add(pauseMenu);
            getButtons(i_Buttons, menuPoint);
            o_GameObejectsToAdd.AddRange(m_MenuButtons);
            m_PauseMenuBackground = pauseMenu;
        }

        private void getButtons(Buttons i_Buttons, Point i_MenuPoint)
        {
            m_MenuButtons = i_Buttons.GetMenuButtons(i_MenuPoint);
        }

        public void ShowPauseMenu()
        {
            changeVisibility(true);
        }

        private void changeVisibility(bool i_Show)
        {
            foreach (GameObject button in m_MenuButtons)
            {
                button.IsVisable = i_Show;
                if (button.Text == eButton.Resume.ToString())
                {
                    button.IsVisable = ShowResume;
                    MenuLabel =button;
                    MenuLabel.ScreenObjectType = eScreenObjectType.Label;
                }
                button.OnUpdate();
            }

            m_PauseMenuBackground.IsVisable = i_Show;
            m_PauseMenuBackground.OnUpdate();
        }

        public GameObject ShowEndGameMenu()
        {
            ShowResume = false;
            changeVisibility(true);
            return MenuLabel;
        }

        public void HidePauseMenu()
        {
            changeVisibility(false);
        }

        private Point getPauseMenuBackgroundPoint()
        {
            SizeDTO screenSize = m_gameInformation.m_ClientScreenDimension.ScreenSizeInPixels;

            return new Point(
                (screenSize.Width / 2) - 3 * GameSettings.GameGridSize,
                (int)((screenSize.Height / 2) - 2.5 * GameSettings.GameGridSize));
        }
    }
}