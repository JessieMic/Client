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
    public class ScreenMapping
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player = Player.Instance;
        public Size m_TotalScreenSize = new Size();
        public Point m_ValueToAdd; //Values that each Player adds to game objects so the would be in the right place
        public Size m_Boundaries;//Boundaries in case of an uneven amount of players
        private int m_MinimumLeftColumn;
        private int m_MinimumUpperRow;
        List<Size> m_PlayerGameBoardScreenSize = new List<Size>();
        
        public Size m_MovementButtonSize = new Size(35, 35);
        public int m_GameBoardGridSize = 35;
        public int m_SpacingAroundButtons = 10;
        public int m_ControllBoardTotalHeight;

        public ScreenMapping()
        {
            calculateMaxBoardSizeByGrid();
            calculateTotalScreenSize();
            calculateScreenValuesToAdd();
        }

        private void calculateMaxBoardSizeByGrid()
        {
            m_ControllBoardTotalHeight = m_MovementButtonSize.m_Height * 3 + m_SpacingAroundButtons * 2;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                Size maxScreenSize = new Size();
                maxScreenSize.m_Height = ((m_GameInformation.ScreenInfoOfAllPlayers[i].Size.m_Height - m_ControllBoardTotalHeight) / m_GameBoardGridSize);
                maxScreenSize.m_Width = ((m_GameInformation.ScreenInfoOfAllPlayers[i].Size.m_Width) / m_GameBoardGridSize);
                m_PlayerGameBoardScreenSize.Add(maxScreenSize);
            }
        }

        private void calculateScreenValuesToAdd()
        {
            calculateWidthScreenValuesToAdd();
            calculateHeightScreenValuesToAdd();
        }

        private void calculateWidthScreenValuesToAdd()
        {
            int playerIndex = m_Player.ButtonThatPlayerPicked - 1;

            if (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].m_Position.Column == eColumnPosition.RightColumn)
            {
                m_ValueToAdd.m_Column = -m_MinimumLeftColumn * m_GameBoardGridSize; 
            }
            else
            {
                m_ValueToAdd.m_Column += (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].Size.m_Width - (m_PlayerGameBoardScreenSize[playerIndex].m_Width * m_GameBoardGridSize));
            }
        }

        private void calculateHeightScreenValuesToAdd()
        {
            int playerIndex = m_Player.ButtonThatPlayerPicked - 1;

            if (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].m_Position.Row == eRowPosition.LowerRow)
            {
                m_ValueToAdd.m_Row = -m_MinimumUpperRow * m_GameBoardGridSize; ;
            }
            else
            {
                m_ValueToAdd.m_Row += m_ControllBoardTotalHeight;
                m_ValueToAdd.m_Row +=
                    m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].Size.m_Height - (m_ControllBoardTotalHeight + m_PlayerGameBoardScreenSize[playerIndex].m_Height * m_GameBoardGridSize);
            }
        }

        private void calculateTotalScreenSize()
        {
            calculateTotalWidthScreenSize();
            calculateTotalHeightScreenSize();
        }

        private void calculateTotalHeightScreenSize()
        {
            getMinHeightByRowPosition(eRowPosition.UpperRow);
            m_MinimumUpperRow = m_TotalScreenSize.m_Height;
            getMinHeightByRowPosition(eRowPosition.LowerRow);
        }

        private void calculateTotalWidthScreenSize()
        {
            getMinWidthByColumnPosition(eColumnPosition.LeftColumn);
            m_MinimumLeftColumn = m_TotalScreenSize.m_Width;

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
                        minScreenValue1[0] = m_PlayerGameBoardScreenSize[i].m_Width;
                        minScreenValue1[1] = i;
                    }
                    else
                    {
                        minScreenValue2[0]= m_PlayerGameBoardScreenSize[i].m_Width;
                        minScreenValue2[1] = i;
                    }
                }
            }

            if (minScreenValue2[0] == 0)
            {
                m_Boundaries.m_Width = m_TotalScreenSize.m_Width;
                m_TotalScreenSize.m_Width += minScreenValue1[0];
            }
            else
            {
                if(minScreenValue1[0] < minScreenValue2[0])
                {
                    m_TotalScreenSize.m_Width += minScreenValue1[0];
                    if(m_Player.ButtonThatPlayerPicked-1 == minScreenValue2[1])
                    {
                        m_ValueToAdd.m_Column += ((minScreenValue2[0] - minScreenValue1[0]) * m_GameBoardGridSize);
                    }
                }
                else
                {
                    m_TotalScreenSize.m_Width += minScreenValue2[0];
                    if (m_Player.ButtonThatPlayerPicked - 1 == minScreenValue1[1])
                    {
                        m_ValueToAdd.m_Column += (minScreenValue1[0] - minScreenValue2[0]) *m_GameBoardGridSize;
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
                        minScreenValue1[0] = m_PlayerGameBoardScreenSize[i].m_Height;
                        minScreenValue1[1] = i;
                    }
                    else if (minScreenValue2[0] == 0)
                    {
                        minScreenValue2[0] = m_PlayerGameBoardScreenSize[i].m_Height;
                        minScreenValue2[1] = i;
                    }
                }
            }

            if (minScreenValue2[0] == 0)
            {
                if (m_GameInformation.AmountOfPlayers == 3 && m_TotalScreenSize.m_Height != 0)
                {
                    m_Boundaries.m_Height = m_TotalScreenSize.m_Height;
                }
                m_TotalScreenSize.m_Height += minScreenValue1[0];
            }
            else
            {
                if (minScreenValue1[0] < minScreenValue2[0])
                {
                    m_TotalScreenSize.m_Height += minScreenValue1[0];
                    if (m_Player.ButtonThatPlayerPicked - 1 == minScreenValue2[1])
                    {
                        m_ValueToAdd.m_Row +=
                            (minScreenValue2[0] - minScreenValue1[0]) * m_GameBoardGridSize;
                    }
                }
                else
                {
                    m_TotalScreenSize.m_Height += minScreenValue2[0];
                    if (m_Player.ButtonThatPlayerPicked - 1 == minScreenValue1[1])
                    {
                        m_ValueToAdd.m_Row +=
                            (minScreenValue1[0] - minScreenValue2[0]) * m_GameBoardGridSize;
                    }
                }
            }
        }
    }
}       