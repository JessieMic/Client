using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Objects;
using Objects.Enums;
using Point = Objects.Point;


namespace LogicUnit
{
    public abstract partial class Game
    {
        public void SetGameScreen()
        {
            setUIBackground();
            setGameButtons();
            setGameBackground();
            setHearts();
            setPauseMenu();
            AddGameObjects();
            OnAddScreenObjects();
        }

        protected abstract void AddGameObjects();

        protected void setPauseMenu()
        {
           m_PauseMenu.GetPauseMenu(m_Buttons,ref m_GameObjectsToAdd);
        }


        protected void setHearts()
        {
            m_Hearts.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
            m_Hearts.m_ClientScreenOurSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.PlayerNumber - 1];
            m_Hearts.setHearts(m_GameInformation.AmountOfPlayers, ref m_GameStatus, ref m_LoseOrder,m_Player.PlayerNumber);
            m_Hearts.getHearts(ref m_GameObjectsToAdd);
        }

        protected void setGameButtons()
        {
            m_Buttons.m_MovementButtonOurSize = m_ScreenMapping.m_MovementButtonOurSize;
            m_Buttons.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
            m_Buttons.m_ClientScreenOurSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.PlayerNumber-1];

            m_Buttons.GetGameButtons(ref m_GameObjectsToAdd);
        }

        protected void setGameBackground()
        {
            setBoarder();
            SizeDTO actualOurSize = new SizeDTO(m_ScreenMapping.m_TotalScreenGridSize.Width * m_ScreenMapping.m_GameBoardGridSize,
                m_ScreenMapping.m_TotalScreenGridSize.Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            background.GameBoardGridSize = actualOurSize.Height;
            background.Initialize(eScreenObjectType.Image, 0, m_GameName +"background.png", new Point(0, 0),true, m_ScreenMapping.m_ValueToAdd);
            background.m_Size = actualOurSize;
            m_GameObjectsToAdd.Add(background);
            m_GameInformation.BackgroundRect = new Rect(
                background.PointOnScreen.Column,
                background.PointOnScreen.Row,
                actualOurSize.Width,
                actualOurSize.Height);
        }

        void setBoarder()
        {
            SizeDTO actualOurSize = new SizeDTO(m_ScreenMapping.m_TotalScreenGridSize.Width * m_ScreenMapping.m_GameBoardGridSize,
                m_ScreenMapping.m_TotalScreenGridSize.Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            Point p = m_ScreenMapping.m_ValueToAdd;
            p.Column -= 5;
            p.Row -= 5;
            actualOurSize.Height += 10;
            actualOurSize.Width += 10;
            background.GameBoardGridSize = actualOurSize.Height;
            background.Initialize(eScreenObjectType.Image, 0, "boarder.png", new Point(0, 0), true,p);
            background.m_Size = actualOurSize;
            m_GameObjectsToAdd.Add(background);
            setb();
        }

        void setb()
        {
            SizeDTO actualOurSize = new SizeDTO(m_ScreenMapping.m_TotalScreenGridSize.Width * m_ScreenMapping.m_GameBoardGridSize,
                m_ScreenMapping.m_TotalScreenGridSize.Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            Point p = m_ScreenMapping.m_ValueToAdd;
            p.Column -= 2;
            p.Row -= 2;
            actualOurSize.Height += 5;
            actualOurSize.Width += 5;
            background.GameBoardGridSize = actualOurSize.Height;
            background.Initialize(eScreenObjectType.Image, 0, "boarder2.png", new Point(0, 0), true, p);
            background.m_Size = actualOurSize;
            m_GameObjectsToAdd.Add(background);
        }
        private void setUIBackground()
        {
            Point point = new Point(
                0,
                m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.PlayerNumber - 1].Height + 1);
            GameObject UIBackground = new GameObject();
            if (m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                point.SetAndGetPoint(0, 0);
                UIBackground.SetImageDirection(Direction.Left);
            }
            SizeDTO actualOurSize = new SizeDTO(m_GameInformation.m_ClientScreenDimension.SizeInPixelsDto.Width+5,
                GameSettings.m_UIBackgroundHeight);
            UIBackground.Initialize(eScreenObjectType.Image, 0, "uibackground.png", point, true, new Point(0,0));
            UIBackground.m_Size = actualOurSize;
            m_GameObjectsToAdd.Add(UIBackground);
        }
    }
}
