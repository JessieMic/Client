using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Size = Objects.Size;


namespace LogicUnit
{
    public abstract partial class Game
    {
        public void SetGameScreen()
        {
            setGameButtons();
            setGameBackground();
            AddGameObjects();
            OnAddScreenObjects();
        }

        protected abstract void AddGameObjects();

        protected void setGameButtons()
        {
            m_Buttons.m_MovementButtonSize = m_ScreenMapping.m_MovementButtonSize;
            m_Buttons.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
            m_Buttons.m_ClientScreenSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.ButtonThatPlayerPicked-1];

            m_Buttons.GetGameButtons(ref m_GameObjectsToAdd);
        }

        protected void setGameBackground()
        {
            Size actualSize = new Size(m_ScreenMapping.m_TotalScreenSize.m_Width * m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_TotalScreenSize.m_Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            background.Initialize(eScreenObjectType.Image, 0, "aa.png", new Point(0, 0), actualSize.m_Height, m_ScreenMapping.m_ValueToAdd);
            background.m_Size = actualSize;
            m_GameObjectsToAdd.Add(background);
        }
    }
}
