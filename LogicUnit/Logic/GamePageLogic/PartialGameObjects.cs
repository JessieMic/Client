using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using LogicUnit.Logic.GamePageLogic;
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
            setScoreBoard();
            AddGameObjects();
            OnAddScreenObjects();
        }

        protected abstract void AddGameObjects();

        private void setScoreBoard()
        {
            m_ScoreBoard = new ScoreBoard(m_PauseMenu);
        }

        private void setPauseMenu()
        {
           m_PauseMenu.GetPauseMenu(m_Buttons,ref m_GameObjectsToAdd);
        }

        private void setHearts()
        {
            m_Hearts.m_ClientScreenDimension = r_GameInformation.m_ClientScreenDimension;
            m_Hearts.m_ClientScreenOurSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.PlayerNumber - 1];
            m_Hearts.setHearts(r_GameInformation.AmountOfPlayers, ref m_GameStatus,m_Player.PlayerNumber);
            m_Hearts.getHearts(ref m_GameObjectsToAdd);
        }

        private void setGameButtons()
        {
            m_Buttons.m_MovementButtonOurSize = m_ScreenMapping.m_MovementButtonOurSize;
            m_Buttons.m_ClientScreenDimension = r_GameInformation.m_ClientScreenDimension;
            m_Buttons.m_ClientScreenOurSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.PlayerNumber-1];

            m_Buttons.GetGameButtons(ref m_GameObjectsToAdd);
        }

        private void setGameBackground()
        {
            setBoarder();
            SizeDTO actualOurSize = new SizeDTO((int)r_GameInformation.ImageXValues-3 + m_ScreenMapping.m_TotalScreenGridSize.Width * m_ScreenMapping.m_GameBoardGridSize,
                m_ScreenMapping.m_TotalScreenGridSize.Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            background.GameBoardGridSize = actualOurSize.Height;
            background.Initialize(eScreenObjectType.Image, 0, m_GameName +"background.png", new Point(0, 0),true, m_ScreenMapping.m_ValueToAdd);
            background.Size = actualOurSize;
            m_GameObjectsToAdd.Add(background);
            r_GameInformation.BackgroundRect = new Rect(
                background.PointOnScreen.Column,
                background.PointOnScreen.Row,
                actualOurSize.Width,
                actualOurSize.Height);
        }

        private void setBoarder()
        {
            SizeDTO actualOurSize = new SizeDTO((int)r_GameInformation.ImageXValues + m_ScreenMapping.m_TotalScreenGridSize.Width * m_ScreenMapping.m_GameBoardGridSize,
                m_ScreenMapping.m_TotalScreenGridSize.Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            Point p = m_ScreenMapping.m_ValueToAdd;
            p.Column -= 5;
            p.Row -= 5;
            actualOurSize.Height += 10;
            actualOurSize.Width += 10;
            background.GameBoardGridSize = actualOurSize.Height;
            background.Initialize(eScreenObjectType.Image, 0, "boarder.png", new Point(0, 0), true,p);
            background.Size = actualOurSize;
            m_GameObjectsToAdd.Add(background);
            setb();
        }

        private void setb()
        {
            SizeDTO actualOurSize = new SizeDTO((int)r_GameInformation.ImageXValues + m_ScreenMapping.m_TotalScreenGridSize.Width * m_ScreenMapping.m_GameBoardGridSize,
                m_ScreenMapping.m_TotalScreenGridSize.Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            Point p = m_ScreenMapping.m_ValueToAdd;
            p.Column -= 2;
            p.Row -= 2;
            actualOurSize.Height += 5;
            actualOurSize.Width += 5;
            background.GameBoardGridSize = actualOurSize.Height;
            background.Initialize(eScreenObjectType.Image, 0, "boarder2.png", new Point(0, 0), true, p);
            background.Size = actualOurSize;
            m_GameObjectsToAdd.Add(background);
        }
        private void setUIBackground()
        {
            Point point = new Point(
                0,
                m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.PlayerNumber - 1].Height + 1);
            GameObject UIBackground = new GameObject();
            if (r_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                point.SetAndGetPoint(0, 0);
                UIBackground.SetImageDirection(Direction.Left);
            }
            SizeDTO actualOurSize = new SizeDTO((r_GameInformation.m_ClientScreenDimension.SizeInPixelsDto.Width+5)*2,
                GameSettings.m_UIBackgroundHeight);
            UIBackground.Initialize(eScreenObjectType.Space, 0, "uibackground.png", point, true, new Point(-r_GameInformation.ImageXValues-5, 0));
            UIBackground.Size = actualOurSize;
            m_GameObjectsToAdd.Add(UIBackground);
        }
    }
}
