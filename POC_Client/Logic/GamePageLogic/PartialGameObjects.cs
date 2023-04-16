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
    public abstract partial class Game
    {
        public event EventHandler<ScreenObject> AddScreenObject;

        //private int m_MovementButtonSize = 35;
        //private int m_SpacingAroundButtons = 10;
        //private int m_ControllBoardTotalHeight = m_MovementButtonSize*3 + m_SpacingAroundButtons*2;

        protected virtual void OnAddScreenObject(ScreenObject i_ScreenObject)
        {
            AddScreenObject.Invoke(this, i_ScreenObject);
        }

        public void SetGameScreen()
        {
            //m_GameInformation.m_ClientScreenDimension.Position.Row = eRowPosition.LowerRow;
            //m_GameInformation.m_ClientScreenDimension.Position.Column = eColumnPosition.LeftColumn;
            //setGameButtons();
             setGameButtons();
            setGameBackGround();
            //setGameButtons();
            //setGameSpacing();
        }

        protected void setGameButtons()
        {
            Size size = m_ScreenMapping.m_MovementButtonSize;

            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, eButton.Right, getButtonPoint(eButton.Right), size, string.Empty, null));
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, eButton.Left, getButtonPoint(eButton.Left), size, string.Empty, null));
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, eButton.Down, getButtonPoint(eButton.Down), size, string.Empty, null));
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, eButton.Up, getButtonPoint(eButton.Up), size, string.Empty, null));
        }

        protected void setGameSpacing()
        {

            OnAddScreenObject(new ScreenObject(eScreenObjectType.Space, null, getSpacingPoint(), new Size(10, 10), string.Empty, null));
        }

        protected void setGameBackGround()
        {
            Size actualSize = new Size(m_ScreenMapping.m_TotalScreenSize.m_Width*m_ScreenMapping.m_GameBoardGridSize.m_Height,m_ScreenMapping.m_TotalScreenSize.m_Height*m_ScreenMapping.m_GameBoardGridSize.m_Height);
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Image, null, m_ScreenMapping.m_ValueToAdd, actualSize, "aa.png", null));
        }

        private Point getButtonPoint(eButton i_Type)
        {
            Point returnPoint = new Point();
            int space = m_ScreenMapping.m_SpacingAroundButtons;
            Size buttonSize = m_ScreenMapping.m_MovementButtonSize;

            if (m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if(i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(buttonSize.m_Width + space, space + 2 * buttonSize.m_Height);
                }
                else if(i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(buttonSize.m_Width + space, space);
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(space, space + buttonSize.m_Height);
                }
                else
                {
                    returnPoint.SetAndGetPoint(space + 2* buttonSize.m_Width, space + buttonSize.m_Height);
                }
            }
            else
            {
                int screenWidth = m_GameInformation.m_ClientScreenDimension.Size.m_Width;
                int screenHeight = m_GameInformation.m_ClientScreenDimension.Size.m_Height;
                if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space+2 * buttonSize.m_Width), screenHeight-(space + 3 * buttonSize.m_Height));
                }   
                else if(i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + 2 * buttonSize.m_Width), screenHeight - (space + buttonSize.m_Height));
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + buttonSize.m_Width), screenHeight - (space + 2* buttonSize.m_Height));
                }
                else
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + 3 * buttonSize.m_Width), screenHeight - (space + 2 * buttonSize.m_Height));
                }
            }
            return returnPoint;
        }

        private Point getSpacingPoint()
        {
            Point returnPoint = new Point();

            if (m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if (m_GameInformation.m_ClientScreenDimension.Position.Column == eColumnPosition.RightColumn)
                {
                    returnPoint.SetAndGetPoint(5, 0);
                }
                else
                {
                    returnPoint.SetAndGetPoint(0, 0);
                }
            }
            else
            {
                if (m_GameInformation.m_ClientScreenDimension.Position.Column == eColumnPosition.RightColumn)
                {
                    returnPoint.SetAndGetPoint(5, 5);
                }
                else
                {
                    returnPoint.SetAndGetPoint(0, 5);
                }
            }

            return returnPoint;
        }

        private Point getBackgroundPoint()
        {
            Point returnPoint = new Point();

            if(m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if(m_GameInformation.m_ClientScreenDimension.Position.Column == eColumnPosition.RightColumn)
                {
                    returnPoint.SetAndGetPoint(0,4);
                }
                else
                {
                    returnPoint.SetAndGetPoint(4,4);
                }
            }
            else
            {
                if (m_GameInformation.m_ClientScreenDimension.Position.Column == eColumnPosition.RightColumn)
                {
                    returnPoint.SetAndGetPoint(0,0);
                }
                else
                {
                    returnPoint.SetAndGetPoint(4,0);
                }
            }

            return returnPoint;
        }

        private eButton getTypeOfButton(Point i_Point)
        {
            eButton result;

            if(m_GameInformation.m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if(new Point(2, 1) == i_Point)
                {
                    result = eButton.Down;
                }
                else if(new Point(1, 2)==i_Point)
                {
                    result = eButton.Right;
                }
                else if(new Point(3, 2)== i_Point)
                {
                    result = eButton.Left;
                }
                else
                {
                    result = eButton.Up;
                }
            }
            else
            {
                if (new Point(2, 1) == i_Point)
                {
                    result = eButton.Up;
                }
                else if (new Point(1, 2) == i_Point)
                {
                    result = eButton.Left;
                }
                else if (new Point(3, 2) == i_Point)
                {
                    result = eButton.Right;
                }
                else
                {
                    result = eButton.Down;
                }
            }

            return result;
        }

    }
}
