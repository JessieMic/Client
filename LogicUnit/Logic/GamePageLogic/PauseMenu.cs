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
        private Player m_Player = Player.Instance;
        public void GetPauseMenu(ScreenMapping i_ScreenMapping, Buttons i_Buttons, ref List<GameObject> o_GameObejectsToAdd)
        {
            SizeDTO screenOurSize = i_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.ButtonThatPlayerPicked - 1];
            GameObject pauseMenu = new GameObject();
            pauseMenu.Initialize(eScreenObjectType.Image, 0, "pausemenu.png", new Point((screenOurSize.m_Width / 2) - 2, (screenOurSize.m_Height / 2) - 2), GameSettings.m_GameBoardGridSize, new Point(0, 0));
            pauseMenu.m_OurSize = GameSettings.m_PauseMenuOurSize;
            m_PauseMenuIDList.Add(pauseMenu.m_ID[0]);
            o_GameObejectsToAdd.Add(pauseMenu);
            o_GameObejectsToAdd.AddRange(GetButtons(i_Buttons));
        }

        private List<GameObject> GetButtons(Buttons i_Buttons)
        {
            List<GameObject> menuButtons = i_Buttons.GetMenuButtons();

            foreach(var menuButton in menuButtons)
            {
                m_PauseMenuIDList.Add(menuButton.m_ID[0]);
            }

            return menuButtons;
        }
    }
}
