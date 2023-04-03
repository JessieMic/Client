using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects;

namespace POC_Client.Logic
{
    public class ScreenMapping
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        private Player m_Player = Player.Instance;
        private List<ScreenDimension> m_ScreenInfoOfAllPlayers;
        private double m_TotalScreenWidth;
        private double m_TotalScreenHeight;
        private double m_ValueToAddToWidth; // Values that each Player adds to game objects so the would be in the right place 
        private double m_ValueToAddToHeight;
        private double m_WidthBoundaries; //Boundaries in case of an uneven amount of players
        private double m_HeightBoundaries;
        private int m_AmountOfPlayers;
        private int m_PlayerSpot;

        private double m_MinimumLeftColumn;
        private double m_MinimumUpperRow;

        public ScreenMapping()
        {
            m_ScreenInfoOfAllPlayers = m_GameInformation.ScreenInfoOfAllPlayers;
            m_AmountOfPlayers = m_GameInformation.AmountOfPlayers;
            m_PlayerSpot = m_Player.ButtonThatPlayerPicked;

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
            if (m_ScreenInfoOfAllPlayers[m_PlayerSpot - 1].Position.Column == eColumnPosition.RightColumn)
            {
                m_ValueToAddToWidth = -m_MinimumLeftColumn;
            }
        }

        private void calculateHeightScreenValuesToAdd()
        {
            if (m_ScreenInfoOfAllPlayers[m_PlayerSpot - 1].Position.Row == eRowPosition.LowerRow)
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
                getMinWidthByColumnPosition(eColumnPosition.RightColumn);
            }
        }

        private void getMinWidthByColumnPosition(eColumnPosition i_ColumnPosition)
        {
            double minScreenValue1 = 0;
            double minScreenValue2 = 0;

            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                if (m_ScreenInfoOfAllPlayers[i].Position.Column == i_ColumnPosition)
                {
                    if (minScreenValue1 == 0)
                    {
                        minScreenValue1 = m_ScreenInfoOfAllPlayers[i].Width;
                    }
                    else
                    {
                        minScreenValue2 = m_ScreenInfoOfAllPlayers[i].Width;
                    }
                }
            }

            if (minScreenValue2 == 0)
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

            for (int i = 0; i < m_AmountOfPlayers; i++)
            {
                if (m_ScreenInfoOfAllPlayers[i].Position.Row == i_RowPosition)
                {
                    if (minScreenValue1 == 0)
                    {
                        minScreenValue1 = m_ScreenInfoOfAllPlayers[i].Height;
                    }
                    else if (minScreenValue2 == 0)
                    {
                        minScreenValue2 = m_ScreenInfoOfAllPlayers[i].Height;
                    }
                }
            }

            if (minScreenValue2 == 0)
            {
                if (m_AmountOfPlayers == 3 && m_TotalScreenHeight != 0)
                {
                    m_HeightBoundaries = m_TotalScreenHeight;
                }
                m_TotalScreenHeight += minScreenValue1;
            }
            else
            {
                double minTotalRow;
                minTotalRow = double.Min(minScreenValue1, minScreenValue2);
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