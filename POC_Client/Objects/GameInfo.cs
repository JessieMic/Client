using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Client.Objects
{
    public class GameInfo
    {
        private List<ScreenInfo> m_ScreenInfoOfAllPlayers;
        private List<string> m_NamesOfAllPlayers;
        private double m_TotalScreenWidth;
        private double m_TotalScreenHeight;
        private double m_ValueToAddToWidth; // Values that each client adds to game objects so the would be in the right place 
        private double m_ValueToAddToHeight;
        private double m_WidthBoundaries; //Boundaries in case of an uneven amount of players
        private double m_HeightBoundaries;
        private int m_AmountOfPlayers;
        private int m_ClientSpot;

        private double m_MinimumLeftColumn;
        private double m_MinimumMiddleAndLeft;
        private double m_MinimumUpperRow;

        public GameInfo(List<ScreenInfo> i_ScreenInfoOfAllPlayers, List<string> i_NamesOfAllPlayers, int i_AmountOfPlayers,int i_ClientSpot)
        {
            m_ScreenInfoOfAllPlayers = i_ScreenInfoOfAllPlayers;
            m_NamesOfAllPlayers = i_NamesOfAllPlayers;
            m_AmountOfPlayers = i_AmountOfPlayers;
            m_ClientSpot = i_ClientSpot;

            calculateTotalScreenSize();
            calculateScreenValuesToAdd();
        }

        private void calculateScreenValuesToAdd()
        {
            calculateWidthScreenValuesToAdd();
            calculateHeightScreenValuesToAdd();
        }

        private void calculateWidthScreenValuesToAdd()
        {
            if(m_ScreenInfoOfAllPlayers[m_ClientSpot - 1].m_ColumnPosition == eColumnPosition.MiddleColumn)
            {
                m_ValueToAddToWidth = -m_MinimumLeftColumn;
            }
            else if(m_ScreenInfoOfAllPlayers[m_ClientSpot - 1].m_ColumnPosition == eColumnPosition.RightColumn)
            {
                m_ValueToAddToWidth = -m_MinimumMiddleAndLeft;
            }
        }

        private void calculateHeightScreenValuesToAdd()
        {
            if(m_ScreenInfoOfAllPlayers[m_ClientSpot - 1].m_RowPosition == eRowPosition.LowerRow)
            {
                m_ValueToAddToHeight = -m_MinimumUpperRow;
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
            m_MinimumUpperRow = m_TotalScreenHeight;
            getMinHeightByRowPosition(eRowPosition.LowerRow);
        }

        private void calculateTotalWidthScreenSize()
        {
            getMinWidthByColumnPosition(eColumnPosition.LeftColumn);
            m_MinimumLeftColumn = m_TotalScreenWidth;

            if (m_AmountOfPlayers > 2)
            {
                getMinWidthByColumnPosition(eColumnPosition.MiddleColumn);
                m_MinimumMiddleAndLeft = m_TotalScreenWidth;
            }

            if (m_AmountOfPlayers > 4)
            {
                getMinWidthByColumnPosition(eColumnPosition.RightColumn);
            }
        }

        private void getMinWidthByColumnPosition(eColumnPosition i_ColumnPosition)
        {
            double minScreenValue1 = 0; 
            double minScreenValue2 = 0;

            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                if(m_ScreenInfoOfAllPlayers[i].m_ColumnPosition == i_ColumnPosition)
                {
                    if(minScreenValue1 == 0)
                    {
                        minScreenValue1 = m_ScreenInfoOfAllPlayers[i].Width;
                    }
                    else
                    {
                        minScreenValue2 = m_ScreenInfoOfAllPlayers[i].Width;
                    }
                }
            }

            if(minScreenValue2 == 0)
            {
                m_WidthBoundaries = m_TotalScreenWidth;
                m_TotalScreenWidth += minScreenValue1;
            }
            else
            {
                m_TotalScreenWidth += double.Min(minScreenValue1, minScreenValue2);
            }
        }

        private void getMinHeightByRowPosition(eRowPosition i_RowPosition)
        {
            double minScreenValue1 = 0;
            double minScreenValue2 = 0;
            double minScreenValue3 = 0;
            double minTotalRow;

            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                if (m_ScreenInfoOfAllPlayers[i].m_RowPosition == i_RowPosition)
                {
                    if (minScreenValue1 == 0)
                    {
                        minScreenValue1 = m_ScreenInfoOfAllPlayers[i].Height;
                    }
                    else if(minScreenValue2 == 0)
                    {
                        minScreenValue2 = m_ScreenInfoOfAllPlayers[i].Height;
                    }
                    else
                    {
                        minScreenValue3 = m_ScreenInfoOfAllPlayers[i].Height;
                    }
                }
            }

            if(minScreenValue2 == 0)
            {
                if(m_AmountOfPlayers == 3 && m_TotalScreenHeight != 0)
                {
                    m_HeightBoundaries = m_TotalScreenHeight;
                }
                m_TotalScreenHeight += minScreenValue1;
            }
            else
            {
                if(m_AmountOfPlayers == 5 && m_TotalScreenHeight != 0)
                {
                    m_HeightBoundaries = m_TotalScreenHeight;
                }
                    minTotalRow = double.Min(minScreenValue1, minScreenValue2);
                if(minScreenValue3 != 0)
                {
                    minTotalRow = double.Min(minScreenValue3, minTotalRow);
                }

                m_TotalScreenWidth += minTotalRow;
            }
        }
    }
}




























//private void calculateTotalScreenSize()
//{
//    if (m_AmountOfPlayers == 2)
//    {
//        getMinWidthSizeBetweenTwoScreens(0, 1);
//        getMinHeightSizeBetweenTwoScreens(0, 1);
//    }
//    else if (m_AmountOfPlayers == 3)
//    {
//        getMinWidthSizeBetweenTwoScreens(0, 2);
//        m_WidthBoundaries = m_TotalScreenWidth;
//        m_TotalScreenWidth += m_ScreenInfoOfAllPlayers[1].Width;
//        getMinHeightSizeBetweenTwoScreens(0, 1);
//        m_HeightBoundaries = m_TotalScreenHeight;
//        m_TotalScreenHeight += m_ScreenInfoOfAllPlayers[2].Height;
//    }
//    else if (m_AmountOfPlayers == 5)
//    {
//        getMinWidthSizeBetweenTwoScreens(0, 3);
//        getMinWidthSizeBetweenTwoScreens(1, 4);
//        m_WidthBoundaries = m_TotalScreenWidth;
//        m_TotalScreenWidth += m_ScreenInfoOfAllPlayers[2].Width;
//        getMinHeightSizeBetweenTwoScreens(0, 1);
//        m_HeightBoundaries = m_TotalScreenHeight;
//        getMinHeightSizeBetweenTwoScreens(3, 4);
//    }
//    else
//    {

//    }

//}

//private void getMinWidthSizeBetweenTwoScreens(int i_ScreenA, int i_ScreenB)
//{
//    //double minimumWidth = double.Min(
//    //    m_ScreenInfoOfAllPlayers[i_ScreenA].Width,
//    //    m_ScreenInfoOfAllPlayers[i_ScreenB].Width);

//    //m_TotalScreenWidth += minimumWidth;
//}


//private void getMinHeightSizeBetweenTwoScreens(int i_ScreenA, int i_ScreenB)
//{
//    double minimumHeight = double.Min(
//        m_ScreenInfoOfAllPlayers[i_ScreenA].Height,
//        m_ScreenInfoOfAllPlayers[i_ScreenB].Height);

//    if ((m_AmountOfPlayers == 5 || m_AmountOfPlayers == 6) && i_ScreenA == 0)
//    {
//        minimumHeight = double.Min(minimumHeight, m_ScreenInfoOfAllPlayers[2].Height);
//    }

//    m_TotalScreenHeight += minimumHeight;
//}