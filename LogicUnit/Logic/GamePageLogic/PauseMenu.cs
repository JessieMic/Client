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
        private GameInformation m_gameInformation = GameInformation.Instance;
        public void GetPauseMenu(Buttons i_Buttons, ref List<GameObject> o_GameObejectsToAdd)
        {
            GameObject pauseMenu = new GameObject();
            Point menuPoint = getPauseMenuBackgroundPoint();
            pauseMenu.IsVisable = false;
            pauseMenu.Initialize(eScreenObjectType.Image, 0, "pausemenu.png", menuPoint, false, new Point(0, 0));
            pauseMenu.m_Size = GameSettings.m_PauseMenuOurSize;
            m_PauseMenuIDList.Add(pauseMenu.ID);
            o_GameObejectsToAdd.Add(pauseMenu);
            o_GameObejectsToAdd.AddRange(GetButtons(i_Buttons, menuPoint));
        }

        private List<GameObject> GetButtons(Buttons i_Buttons, Point i_MenuPoint)
        {
            List<GameObject> menuButtons = i_Buttons.GetMenuButtons(i_MenuPoint);

            foreach(var menuButton in menuButtons)
            {
                m_PauseMenuIDList.Add(menuButton.ID);
            }

            return menuButtons;
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
