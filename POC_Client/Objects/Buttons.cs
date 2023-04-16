﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Logic;
using POC_Client.Objects.Enums;

namespace POC_Client.Objects
{
    public class Buttons
    {
        public Size m_MovementButtonSize = new Size();
        public ScreenDimension m_ClientScreenDimension= new ScreenDimension();
        public ScreenMapping m_ScreenMapping;

        public List<ScreenObject> GetGameButtons()
        {
            return addBasicGameButtons();//up down left right
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {

        }

        protected List<ScreenObject> addBasicGameButtons()
        {
            List<ScreenObject> returnButtons = new List<ScreenObject>(); ;
           
            returnButtons.Add(new ScreenObject(eScreenObjectType.Button, eButton.Right, getButtonPoint(eButton.Right), m_MovementButtonSize, string.Empty, null));
            returnButtons.Add(new ScreenObject(eScreenObjectType.Button, eButton.Left, getButtonPoint(eButton.Left), m_MovementButtonSize, string.Empty, null));
            returnButtons.Add(new ScreenObject(eScreenObjectType.Button, eButton.Down, getButtonPoint(eButton.Down), m_MovementButtonSize, string.Empty, null));
            returnButtons.Add(new ScreenObject(eScreenObjectType.Button, eButton.Up, getButtonPoint(eButton.Up), m_MovementButtonSize, string.Empty, null));

            return returnButtons;
        }

        private Point getButtonPoint(eButton i_Type)
        {
            Point returnPoint = new Point();
            int space = m_ScreenMapping.m_SpacingAroundButtons;
            Size buttonSize = m_ScreenMapping.m_MovementButtonSize;

            if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(buttonSize.m_Width + space, space + 2 * buttonSize.m_Height);
                }
                else if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(buttonSize.m_Width + space, space);
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(space, space + buttonSize.m_Height);
                }
                else
                {
                    returnPoint.SetAndGetPoint(space + 2 * buttonSize.m_Width, space + buttonSize.m_Height);
                }
            }
            else
            {
                int screenWidth = m_ClientScreenDimension.Size.m_Width;
                int screenHeight = m_ClientScreenDimension.Size.m_Height;
                if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + 2 * buttonSize.m_Width), screenHeight - (space + 3 * buttonSize.m_Height));
                }
                else if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + 2 * buttonSize.m_Width), screenHeight - (space + buttonSize.m_Height));
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + buttonSize.m_Width), screenHeight - (space + 2 * buttonSize.m_Height));
                }
                else
                {
                    returnPoint.SetAndGetPoint(screenWidth - (space + 3 * buttonSize.m_Width), screenHeight - (space + 2 * buttonSize.m_Height));
                }
            }
            return returnPoint;
        }
    }
}
