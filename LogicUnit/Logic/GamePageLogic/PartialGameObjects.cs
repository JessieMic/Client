﻿using System;
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
            setGameButtons();
            setGameBackground();
            setHearts();
            setPauseMenu();
            AddGameObjects();
            OnAddScreenObjects();
            OnHideGameObjects(m_PauseMenu.m_PauseMenuIDList);
        }

        protected abstract void AddGameObjects();

        protected void setPauseMenu()
        {
           m_PauseMenu.GetPauseMenu(m_ScreenMapping,m_Buttons,ref m_GameObjectsToAdd);
        }


        protected void setHearts()
        {
            m_Hearts.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
            m_Hearts.m_ClientScreenOurSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.ButtonThatPlayerPicked - 1];
            m_Hearts.setHearts(m_GameInformation.AmountOfPlayers, ref m_GameStatus, ref m_LoseOrder,m_Player.ButtonThatPlayerPicked);
            m_Hearts.getHearts(ref m_GameObjectsToAdd);
        }

        protected void setGameButtons()
        {
            m_Buttons.m_MovementButtonOurSize = m_ScreenMapping.m_MovementButtonOurSize;
            m_Buttons.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
            m_Buttons.m_ClientScreenOurSize = m_ScreenMapping.m_PlayerGameBoardScreenSize[m_Player.ButtonThatPlayerPicked-1];

            m_Buttons.GetGameButtons(ref m_GameObjectsToAdd);
        }

        protected void setGameBackground()
        {
            SizeDTO actualOurSize = new SizeDTO(m_ScreenMapping.m_TotalScreenOurSize.m_Width * m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_TotalScreenOurSize.m_Height * m_ScreenMapping.m_GameBoardGridSize);
            GameObject background = new GameObject();
            background.Initialize(eScreenObjectType.Image, 0, "aa.png", new Point(0, 0), actualOurSize.m_Height, m_ScreenMapping.m_ValueToAdd);
            background.m_OurSize = actualOurSize;
            m_GameObjectsToAdd.Add(background);
        }
    }
}
