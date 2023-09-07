using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DTOs;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit
{
    public class ScreenMapping
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player;
        public SizeD m_TotalScreenGridSize = new SizeD();
        public SizeD m_TotalScreenPixelSize = new SizeD();
        public Point m_ValueToAdd; //Values that each Player adds to game objects so the would be in the right place
        public SizeD m_Boundaries;//Boundaries in case of an uneven amount of players
        private int m_MinimumLeftColumn;
        private int m_MinimumUpperRow;
        public List<SizeD> m_PlayerGameBoardScreenSize = new List<SizeD>();
        public SizeD m_MovementButtonOurSize = GameSettings.m_MovementButtonOurSize;
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
                SizeD maxScreenOurSize = new SizeD();
                maxScreenOurSize.Height = ((m_GameInformation.ScreenInfoOfAllPlayers[i].SizeInPixelsD.Height - GameSettings.ControllBoardTotalHeight) / m_GameBoardGridSize);
                maxScreenOurSize.Width = ((m_GameInformation.ScreenInfoOfAllPlayers[i].SizeInPixelsD.Width) / m_GameBoardGridSize);
                m_PlayerGameBoardScreenSize.Add(maxScreenOurSize);
                System.Diagnostics.Debug.WriteLine($"player {m_GameInformation.Player.PlayerNumber} -{i+1} - {m_GameInformation.ScreenInfoOfAllPlayers[i].SizeInPixelsD.Width}");
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
                m_ValueToAdd.Column += (m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].SizeInPixelsD.Width - (m_PlayerGameBoardScreenSize[playerIndex].Width * m_GameBoardGridSize));
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
                    m_GameInformation.ScreenInfoOfAllPlayers[playerIndex].SizeInPixelsD.Height - (GameSettings.ControllBoardTotalHeight + m_PlayerGameBoardScreenSize[playerIndex].Height * m_GameBoardGridSize);
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
            double[] density1 = new double[2]; ;
            double[] density2= new double[2];
            

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                if (m_GameInformation.ScreenInfoOfAllPlayers[i].m_Position.Column == i_ColumnPosition)
                {
                    if (minScreenValue1[0] == 0)
                    {
                        minScreenValue1[0] = m_PlayerGameBoardScreenSize[i].Width;
                        density1[1] = minScreenValue1[1] = i;
                        density1[0] = m_GameInformation.ScreenInfoOfAllPlayers[i].Density;
                    }
                    else
                    {
                        minScreenValue2[0]= m_PlayerGameBoardScreenSize[i].Width;
                        density2[1] = minScreenValue2[1] = i;
                        density2[0] = m_GameInformation.ScreenInfoOfAllPlayers[i].Density;
                    }
                }
            }
            //m_Boundaries.Width = m_TotalScreenGridSize.Width;
            if (minScreenValue2[0] == 0)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"&&&{m_GameInformation.Player.PlayerNumber}|| {m_GameInformation.ScreenInfoOfAllPlayers[1].m_Position.Column.ToString()}");
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

                if(density1[0] < density2[0])
                {
                    setDensityAndValuesToAdd(density2[0], density1[0], density2[1], i_ColumnPosition);
                }
                else 
                {
                    setDensityAndValuesToAdd(density1[0], density2[0], density1[1], i_ColumnPosition);
                }
            }
        }

        void setDensityAndValuesToAdd(double i_Density1, double i_Density2,double i_Player,eColumnPosition i_ColumnPosition)
        {
            if((int)i_Player == m_Player.PlayerNumber - 1)
            {
                int boardLeftSizeInPixel;
                m_GameInformation.ImageDensity = i_Density1/i_Density2;

                if (i_ColumnPosition == eColumnPosition.LeftColumn)
                {
                    boardLeftSizeInPixel = m_TotalScreenGridSize.Width * m_GameBoardGridSize;
                }
                else
                {
                    boardLeftSizeInPixel = m_Boundaries.Width * m_GameBoardGridSize;
                }

                try
                {
                    //m_ValueToAdd.Column += boardLeftSizeInPixel -(boardLeftSizeInPixel / m_GameInformation.ImageDensity);
                    m_GameInformation.ImageXValues = boardLeftSizeInPixel -(boardLeftSizeInPixel / m_GameInformation.ImageDensity);
                }
                catch (Exception ex)
                {

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