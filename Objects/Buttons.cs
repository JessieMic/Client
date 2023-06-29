using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Objects.Enums;

namespace Objects
{
    public class Buttons
    {
        public SizeDTO m_MovementButtonOurSize = new SizeDTO(35,35);
        public ScreenDimension m_ClientScreenDimension = new ScreenDimension();
        private List<eButton> m_Buttons = new List<eButton>();
        public SizeDTO m_ClientScreenOurSize = new SizeDTO();
        public eTypeOfGameMovementButtons m_TypeMovementButtons;
        public int m_AmountOfExtraButtons = 0;


        public void GetGameButtons(ref List<GameObject> i_GameObjectsToAdd)
        {
            setButtonList();
            
            foreach (var button in m_Buttons)
            {
                GameObject newButton = new GameObject();
                newButton.InitializeButton(
                    button,
                    generatePngString(button),
                    getButtonPoint(button),
                    35,
                    m_MovementButtonOurSize,
                    getValuesToAdd(button));
                if(button == eButton.Restart || button == eButton.Exit || button == eButton.Continue)
                {
                    newButton.m_OurSize = GameSettings.m_PauseMenuButtonOurSize;
                }
                i_GameObjectsToAdd.Add(newButton);
                
            }
        }

        public eGameStatus GetGameStatue(int i_Button)
        {
            eGameStatus status;

            if(i_Button == (int)eButton.PauseMenu)
            {
                status = eGameStatus.Paused;
            }
            else if(i_Button == (int)eButton.Exit)
            {
                status = eGameStatus.Exited;
            }
            else
            {
                status = eGameStatus.Running;
            }

            return status;
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {

        }

        private Point getValuesToAdd(eButton i_Button)
        { 
            Point values = new Point();

            if(m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                values = new Point(10, 10);
            }
            else
            {
                values.m_Column = 10;
                values.m_Row = m_ClientScreenOurSize.m_Height*35+10;
            }
                return values;
        }

        public void GetMenuButtons(ref List<GameObject> i_GameObjectsToAdd)
        {
            
            List<eButton> buttonList = setPauseButtonList();
            foreach (var button in buttonList)
            {
                GameObject newButton = new GameObject();
                newButton.InitializeButton(
                    button,
                    generatePngString(button),
                    getButtonPoint(button),
                    35,
                    GameSettings.m_PauseMenuButtonOurSize,
                    getValuesToAdd(button));
                
                i_GameObjectsToAdd.Add(newButton);
            }
        }

        public void hideMenuButtons()
        {

        }

        protected string generatePngString(eButton i_Button)
        {
            string png;

            png = "upbutton.png";// i_Button.ToString() + "button" + ".png";

            return png.ToLower();
        }

        public eButton StringToButton(string i_Button)
        {
            if (i_Button == eButton.Up.ToString())
            {
                return eButton.Up;
            }
            else if (i_Button == eButton.Down.ToString())
            {
                return eButton.Down;
            }
            else if (i_Button == eButton.PauseMenu.ToString())
            {
                return eButton.PauseMenu;
            }
            else if (i_Button == eButton.Exit.ToString())
            {
                return eButton.Exit;
            }
            else if (i_Button == eButton.Right.ToString())
            {
                return eButton.Right;
            }
            else if (i_Button == eButton.Continue.ToString())
            {
                return eButton.Continue;
            }
            else
            {
                return eButton.Left;
            }
        }

        private Point getButtonPoint(eButton i_Type)
        {
            Point returnPoint = new Point(1,1);

            if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(1, 2);
                }
                else if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(1, 0);
                }
                else if (i_Type == eButton.Left)
                {
                    returnPoint.SetAndGetPoint(0, 1);
                }
                else if(i_Type == eButton.Right) 
                {
                    returnPoint.SetAndGetPoint(2, 1);
                }
                else if(i_Type == eButton.ButtonA)
                {
                    returnPoint.SetAndGetPoint(5, 1);
                }
                else if(i_Type == eButton.ButtonB)
                {
                    returnPoint.SetAndGetPoint(7, 1);
                }
                else if (i_Type == eButton.PauseMenu)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 2, 0);
                }
                else
                {
                    returnPoint = getPauseMenuButtonPoint(i_Type);
                }
            }
            else
            {
                if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 2, 0);
                }
                else if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 2, 2);
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 1, 1);
                }
                else if(i_Type == eButton.Left)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 3, 1);
                }
                else if (i_Type == eButton.ButtonA)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 6, 1);
                }
                else if (i_Type == eButton.ButtonB)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.m_Width - 8, 1);
                }
                else if (i_Type == eButton.PauseMenu)
                {
                    returnPoint.SetAndGetPoint(1, 2);
                }
                else
                {
                    returnPoint = getPauseMenuButtonPoint(i_Type);
                }
            }
            return returnPoint;
        }

        private Point getPauseMenuButtonPoint(eButton i_Type)
        {
            Point returnPoint = new Point(1, 1);

            if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if (i_Type == eButton.Continue)
                {
                    returnPoint.SetAndGetPoint((m_ClientScreenOurSize.m_Width/2)-2, (m_ClientScreenOurSize.m_Height/2)+3);
                }
                else if (i_Type == eButton.Restart)
                {
                    returnPoint.SetAndGetPoint((m_ClientScreenOurSize.m_Width / 2)-2, (m_ClientScreenOurSize.m_Height / 2)+1);
                }
                else if (i_Type == eButton.Exit)
                {
                    returnPoint.SetAndGetPoint((m_ClientScreenOurSize.m_Width / 2)-2, (m_ClientScreenOurSize.m_Height / 2)-1);
                }
            }
            else
            {
                if (i_Type == eButton.Continue)
                {
                    returnPoint.SetAndGetPoint((m_ClientScreenOurSize.m_Width / 2), (m_ClientScreenOurSize.m_Height / 2)-2);
                }
                else if (i_Type == eButton.Restart)
                {
                    returnPoint.SetAndGetPoint((m_ClientScreenOurSize.m_Width / 2), (m_ClientScreenOurSize.m_Height / 2));
                }
                else if (i_Type == eButton.Exit)
                {
                    returnPoint.SetAndGetPoint((m_ClientScreenOurSize.m_Width / 2), (m_ClientScreenOurSize.m_Height / 2)+2);
                }
            }
            return returnPoint;
        }

        private List<eButton> setPauseButtonList()
        {
            List<eButton> buttonList = new List<eButton>();
            buttonList.Add(eButton.Continue);
            buttonList.Add(eButton.Exit);
            buttonList.Add(eButton.Restart);

            return buttonList;
        }

        private void setButtonList()
        {
            m_Buttons.Add(eButton.Right);
            m_Buttons.Add(eButton.Left);
            m_Buttons.Add(eButton.PauseMenu);

            if(m_TypeMovementButtons == eTypeOfGameMovementButtons.AllDirections)
            {
                m_Buttons.Add(eButton.Down);
                m_Buttons.Add(eButton.Up);
            }

            if(m_AmountOfExtraButtons != 0)
            {
                m_Buttons.Add(eButton.ButtonA);
                if(m_AmountOfExtraButtons == 2)
                {
                    m_Buttons.Add(eButton.ButtonB);
                }
            }
        }
    }
}
