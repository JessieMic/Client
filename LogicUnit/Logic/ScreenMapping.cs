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
    public class ScreenMapping
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player;
        public SizeDTO m_TotalScreenGridSize = new SizeDTO();
        public SizeDTO m_TotalScreenPixelSize = new SizeDTO();
        public Point m_ValueToAdd; //Values that each Player adds to game objects so the would be in the right place
        public SizeDTO m_Boundaries;//Boundaries in case of an uneven amount of players
        private int m_MinimumLeftColumn;
        private int m_MinimumUpperRow;
        public List<SizeDTO> m_PlayerGameBoardScreenSize = new List<SizeDTO>();
        public SizeDTO m_MovementButtonOurSize = GameSettings.m_MovementButtonOurSize;
        public int m_GameBoardGridSize = GameSettings.GameGridSize;
        public int m_SpacingAroundButtons = GameSettings.m_SpacingAroundButtons;

        public ScreenMapping()
        {
            m_Player = m_GameInformation.Player;
            calculateMaxBoardSizeByGrid();
            calculateTotalScreenSize();
            calculateScreenValuesToAdd();
        }

        private void calculateMaxBoardSizeByGrid()
        {
            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                SizeDTO maxScreenOurSize = new SizeDTO();
                maxScreenOurSize.Height = ((m_GameInformation.ScreenInfoOfAllPlayers[i].SizeInPixelsDto.Height - GameSettings.ControllBoardTotalHeight) / m_GameBoardGridSize);
                maxScreenOurSize.Width = ((m_GameInformation.ScreenInfoOfAllPlayers[i].SizeInPixelsDto.Width) / m_GameBoardGridSize);
                m_PlayerGameBoardScreenSize.Add(maxScreenOurSize);
            }
        }

        private void calculateScreenValuesToAdd()
        {
            calculateWidthScreenValuesToAdd();
            calculateHeightScreenValuesToAdd();
            m_GameInformation.PointValuesToAddToScreen = m_ValueToAdd;
        }

        private void calculateWidthScreenValuesToAdd()
        {
            int playerIndex = m_Player.PlayerNumber - 1;

            if (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].m_Position.Column == eColumnPosition.RightColumn)
            {
                m_ValueToAdd.Column = -m_MinimumLeftColumn * m_GameBoardGridSize; 
            }
            else
            {
                m_ValueToAdd.Column += (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].SizeInPixelsDto.Width - (m_PlayerGameBoardScreenSize[playerIndex].Width * m_GameBoardGridSize));
            }
        }

        private void calculateHeightScreenValuesToAdd()
        {
            int playerIndex = m_Player.PlayerNumber - 1;

            if (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].m_Position.Row == eRowPosition.LowerRow)
            {
                m_ValueToAdd.Row = -m_MinimumUpperRow * m_GameBoardGridSize; ;
            }
            else
            {
                m_ValueToAdd.Row += GameSettings.ControllBoardTotalHeight;
                m_ValueToAdd.Row +=
                    m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].SizeInPixelsDto.Height - (GameSettings.ControllBoardTotalHeight + m_PlayerGameBoardScreenSize[playerIndex].Height * m_GameBoardGridSize);
            }
        }

        private void calculateTotalScreenSize()
        {
            calculateTotalWidthScreenSize();
            calculateTotalHeightScreenSize();
            m_TotalScreenPixelSize.Width = m_TotalScreenGridSize.Width * m_GameBoardGridSize;
            m_TotalScreenPixelSize.Height = m_TotalScreenGridSize.Height * m_GameBoardGridSize;
            m_GameInformation.GameBoardSizeByPixel = m_TotalScreenPixelSize;
            m_GameInformation.GameBoardSizeByGrid = m_TotalScreenGridSize;
        }

        private void calculateTotalHeightScreenSize()
        {
            getMinHeightByRowPosition(eRowPosition.UpperRow);
            m_MinimumUpperRow = m_TotalScreenGridSize.Height;
            getMinHeightByRowPosition(eRowPosition.LowerRow);
        }

        private void calculateTotalWidthScreenSize()
        {
            getMinWidthByColumnPosition(eColumnPosition.LeftColumn);
            m_MinimumLeftColumn = m_TotalScreenGridSize.Width;

            if (m_GameInformation.AmountOfPlayers > 2)
            {
                getMinWidthByColumnPosition(eColumnPosition.RightColumn);
            }
        }

        private void getMinWidthByColumnPosition(eColumnPosition i_ColumnPosition)
        {
            int[] minScreenValue1 = new int[2];
            int[] minScreenValue2 = new int[2];
            

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                if (m_GameInformation.ScreenInfoOfAllPlayers[i].m_Position.Column == i_ColumnPosition)
                {
                    if (minScreenValue1[0] == 0)
                    {
                        minScreenValue1[0] = m_PlayerGameBoardScreenSize[i].Width;
                        minScreenValue1[1] = i;
                    }
                    else
                    {
                        minScreenValue2[0]= m_PlayerGameBoardScreenSize[i].Width;
                        minScreenValue2[1] = i;
                    }
                }
            }

            if (minScreenValue2[0] == 0)
            {
                m_Boundaries.Width = m_TotalScreenGridSize.Width;
                m_TotalScreenGridSize.Width += minScreenValue1[0];
            }
            else
            {
                if(minScreenValue1[0] < minScreenValue2[0])
                {
                    m_TotalScreenGridSize.Width += minScreenValue1[0];
                    if(m_Player.PlayerNumber-1 == minScreenValue2[1])
                    {
                        m_ValueToAdd.Column += ((minScreenValue2[0] - minScreenValue1[0]) * m_GameBoardGridSize);
                    }
                }
                else
                {
                    m_TotalScreenGridSize.Width += minScreenValue2[0];
                    if (m_Player.PlayerNumber - 1 == minScreenValue1[1])
                    {
                        m_ValueToAdd.Column += (minScreenValue1[0] - minScreenValue2[0]) *m_GameBoardGridSize;
                    }
                }
            }
        }

        private void getMinHeightByRowPosition(eRowPosition i_RowPosition)
        {
            int[] minScreenValue1 = new int[2];
            int[] minScreenValue2 = new int[2];

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                if (m_GameInformation.ScreenInfoOfAllPlayers[i].m_Position.Row == i_RowPosition)
                {
                    if (minScreenValue1[0] == 0)
                    {
                        minScreenValue1[0] = m_PlayerGameBoardScreenSize[i].Height;
                        minScreenValue1[1] = i;
                    }
                    else if (minScreenValue2[0] == 0)
                    {
                        minScreenValue2[0] = m_PlayerGameBoardScreenSize[i].Height;
                        minScreenValue2[1] = i;
                    }
                }
            }

            if (minScreenValue2[0] == 0)
            {
                if (m_GameInformation.AmountOfPlayers == 3 && m_TotalScreenGridSize.Height != 0)
                {
                    m_Boundaries.Height = m_TotalScreenGridSize.Height;
                }
                m_TotalScreenGridSize.Height += minScreenValue1[0];
            }
            else
            {
                if (minScreenValue1[0] < minScreenValue2[0])
                {
                    m_TotalScreenGridSize.Height += minScreenValue1[0];
                    if (m_Player.PlayerNumber - 1 == minScreenValue2[1])
                    {
                        m_ValueToAdd.Row +=
                            (minScreenValue2[0] - minScreenValue1[0]) * m_GameBoardGridSize;
                    }
                }
                else
                {
                    m_TotalScreenGridSize.Height += minScreenValue2[0];
                    if (m_Player.PlayerNumber - 1 == minScreenValue1[1])
                    {
                        m_ValueToAdd.Row +=
                            (minScreenValue1[0] - minScreenValue2[0]) * m_GameBoardGridSize;
                    }
                }
            }
        }
    }
}       