using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects;
using POC_Client.Objects.Enums;
using Point = POC_Client.Objects.Point;
using Size = POC_Client.Objects.Size;

namespace POC_Client.Logic
{
    public class ScreenMapping
    {
        private GameInformation m_GameInformation = GameInformation.Instance;
        public Player m_Player = Player.Instance;
        public Size m_TotalScreenSize = new Size();
        public Point m_p1 = new Point(0, 115);
        public Point m_p2 = new Point(0, -276);
        public Point m_ValueToAdd; //Values that each Player adds to game objects so the would be in the right place
        public Size m_Boundaries;//Boundaries in case of an uneven amount of players
        private double m_MinimumLeftColumn;
        private double m_MinimumUpperRow;

        public ScreenMapping()
        {
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
            if (m_GameInformation.ScreenInfoOfAllPlayers[m_Player.ButtonThatPlayerPicked - 1].m_Position.Column == eColumnPosition.RightColumn)
            {
                m_ValueToAdd.m_Column = (int)-m_MinimumLeftColumn;
            }
        }

        private void calculateHeightScreenValuesToAdd()
        {
            if (m_GameInformation.ScreenInfoOfAllPlayers[m_Player.ButtonThatPlayerPicked - 1].m_Position.Row == eRowPosition.LowerRow)
            {
                m_ValueToAdd.m_Row = (int)-m_MinimumUpperRow;
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
            double minScreenValue1 = 0;
            double minScreenValue2 = 0;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                if (m_GameInformation.ScreenInfoOfAllPlayers[i].m_Position.Column == i_ColumnPosition)
                {
                    if (minScreenValue1 == 0)
                    {
                        minScreenValue1 = m_GameInformation.ScreenInfoOfAllPlayers[i].Size.m_Width;
                    }
                    else
                    {
                        minScreenValue2 = m_GameInformation.ScreenInfoOfAllPlayers[i].Size.m_Width;
                    }
                }
            }

            if (minScreenValue2 == 0)
            {
                m_Boundaries.m_Width = m_TotalScreenSize.m_Width;
                m_TotalScreenSize.m_Width += minScreenValue1;
            }
            else
            {
                m_TotalScreenSize.m_Width += double.Min(minScreenValue1, minScreenValue2);
            }
        }

        private void getMinHeightByRowPosition(eRowPosition i_RowPosition)
        {
            double minScreenValue1 = 0;
            double minScreenValue2 = 0;

            for (int i = 0; i < m_GameInformation.AmountOfPlayers; i++)
            {
                if (m_GameInformation.ScreenInfoOfAllPlayers[i].m_Position.Row == i_RowPosition)
                {
                    if (minScreenValue1 == 0)
                    {
                        minScreenValue1 = m_GameInformation.ScreenInfoOfAllPlayers[i].Size.m_Height;
                    }
                    else if (minScreenValue2 == 0)
                    {
                        minScreenValue2 = m_GameInformation.ScreenInfoOfAllPlayers[i].Size.m_Height;
                    }
                }
            }

            if (minScreenValue2 == 0)
            {
                if (m_GameInformation.AmountOfPlayers == 3 && m_TotalScreenSize.m_Height != 0)
                {
                    m_Boundaries.m_Height = m_TotalScreenSize.m_Height;
                }
                m_TotalScreenSize.m_Height += minScreenValue1;
            }
            else
            {
                double minTotalRow;
                minTotalRow = double.Min(minScreenValue1, minScreenValue2);
                m_TotalScreenSize.m_Height += minTotalRow;
            }
        }
    }
}




























//private void calculateTotalScreenSize()
//{
//    if (m_GameInformation.AmountOfPlayers == 2)
//    {
//        getMinWidthSizeBetweenTwoScreens(0, 1);
//        getMinHeightSizeBetweenTwoScreens(0, 1);
//    }
//    else if (m_GameInformation.AmountOfPlayers == 3)
//    {
//        getMinWidthSizeBetweenTwoScreens(0, 2);
//        m_Boundaries.m_Width = m_TotalScreenSize.m_Width;
//        m_TotalScreenSize.m_Width += m_GameInformation.ScreenInfoOfAllPlayers[1].Width;
//        getMinHeightSizeBetweenTwoScreens(0, 1);
//        m_Boundaries.m_Height = m_TotalScreenSize.m_Height;
//        m_TotalScreenSize.m_Height += m_GameInformation.ScreenInfoOfAllPlayers[2].Height;
//    }
//    else if (m_GameInformation.AmountOfPlayers == 5)
//    {
//        getMinWidthSizeBetweenTwoScreens(0, 3);
//        getMinWidthSizeBetweenTwoScreens(1, 4);
//        m_Boundaries.m_Width = m_TotalScreenSize.m_Width;
//        m_TotalScreenSize.m_Width += m_GameInformation.ScreenInfoOfAllPlayers[2].Width;
//        getMinHeightSizeBetweenTwoScreens(0, 1);
//        m_Boundaries.m_Height = m_TotalScreenSize.m_Height;
//        getMinHeightSizeBetweenTwoScreens(3, 4);
//    }
//    else
//    {

//    }

//}

//private void getMinWidthSizeBetweenTwoScreens(int i_ScreenA, int i_ScreenB)
//{
//    //double minimumWidth = double.Min(
//    //    m_GameInformation.ScreenInfoOfAllPlayers[i_ScreenA].Width,
//    //    m_GameInformation.ScreenInfoOfAllPlayers[i_ScreenB].Width);

//    //m_TotalScreenSize.m_Width += minimumWidth;
//}


//private void getMinHeightSizeBetweenTwoScreens(int i_ScreenA, int i_ScreenB)
//{
//    double minimumHeight = double.Min(
//        m_GameInformation.ScreenInfoOfAllPlayers[i_ScreenA].Height,
//        m_GameInformation.ScreenInfoOfAllPlayers[i_ScreenB].Height);

//    if ((m_GameInformation.AmountOfPlayers == 5 || m_GameInformation.AmountOfPlayers == 6) && i_ScreenA == 0)
//    {
//        minimumHeight = double.Min(minimumHeight, m_GameInformation.ScreenInfoOfAllPlayers[2].Height);
//    }

//    m_TotalScreenSize.m_Height += minimumHeight;
//}