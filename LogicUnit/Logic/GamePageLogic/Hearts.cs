﻿using System;
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
    public class Hearts
    {
        public int[] m_AmountOfLivesPlayerHas = new int[4];
        public int m_AmountOfLivesPlayersGetAtStart = 3;
        public int m_AmountOfPlayersThatAreAlive;
        public int m_AmountOfPlayers;
        public int m_ClientNumber;
        public ScreenDimension m_ClientScreenDimension = new ScreenDimension();
        public eGameStatus m_GameStatus;
        public List<string> m_LoseOrder;
        public SizeDTO m_ClientScreenOurSize = new SizeDTO();
        public List<GameObject> m_HeartsOnScreen = new List<GameObject>();
        public GameObject m_HeartToRemove = null;

        public void setHearts(int i_AmountOfPlayers,ref eGameStatus i_Status, ref List<string> i_LoseList, int i_ClientNumber)
        {
            m_GameStatus = i_Status;
            m_LoseOrder = i_LoseList;
            m_AmountOfPlayers = m_AmountOfPlayersThatAreAlive = i_AmountOfPlayers;
            m_ClientNumber = i_ClientNumber;
            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                m_AmountOfLivesPlayerHas[i] = m_AmountOfLivesPlayersGetAtStart;
            }
        }

        public void getHearts(ref List<GameObject> i_GameObjectsToAdd)
        {
            for (int i = 0; i < m_AmountOfLivesPlayersGetAtStart; i++)
            {
                GameObject newHeart = new GameObject();
                newHeart.Initialize(eScreenObjectType.Image, 0, "heart.png",
                    getHeartPoint(i), GameSettings.m_GameBoardGridSize, getValuesToAdd());

                if(m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
                {
                    newHeart.m_rotate = 180;
                }
                i_GameObjectsToAdd.Add(newHeart);
                m_HeartsOnScreen.Add(newHeart);
            }
        }

        private Point getHeartPoint(int heartNumber)
        {
            Point heartPoint = new Point(1 + heartNumber,0);

            if(m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                heartPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 2 - heartNumber, 2);
            }

            return heartPoint;
        }

        private Point getValuesToAdd()
        {
            Point values = new Point();

            if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                values = new Point(10, 10);
            }
            else
            {
                values.m_Column = 10;
                values.m_Row = m_ClientScreenOurSize.m_Height * 35 + 10;
            }
            return values;
        }

        public eGameStatus setPlayerLifeAndGetGameStatus(int i_Player)
        {
            eGameStatus returnStatus = eGameStatus.Running;
            bool isGameRunning = false;

            m_AmountOfLivesPlayerHas[i_Player - 1]--;

            if (i_Player == m_ClientNumber)
            {
                removeAHeart();
            }

            if (m_AmountOfLivesPlayerHas[i_Player - 1] == 0)
            {
                m_AmountOfPlayersThatAreAlive--;

                if (m_AmountOfPlayersThatAreAlive > 1)//Player lost but game is still running
                {
                    //returnStatus = eGameStatus.Lost;
                }
                else//only one player is alive so the game has ended 
                {
                    //returnStatus = eGameStatus.Ended;
                }
            }

            return returnStatus;
        }

        private void removeAHeart()
        {
            int lastHeartIndex = m_HeartsOnScreen.Count - 1;
            m_HeartToRemove = m_HeartsOnScreen[lastHeartIndex];
            m_HeartsOnScreen.RemoveAt(lastHeartIndex);
        }
    }
}
