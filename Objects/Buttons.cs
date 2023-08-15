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
        public SizeDTO m_MovementButtonOurSize = GameSettings.m_MovementButtonOurSize;
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
                    true,
                    m_MovementButtonOurSize,
                    getValuesToAdd());

                newButton.SetImageDirection(Direction.getDirection(button.ToString()));
                if(button == eButton.PauseMenu && m_ClientScreenDimension.Position.Row == eRowPosition.LowerRow)
                {
                    newButton.Rotatation = 0;
                }
                
                i_GameObjectsToAdd.Add(newButton);
                
            }
        }

        public eGameStatus GetGameStatue(int i_Button,eGameStatus i_GameStatus)
        {
            eGameStatus status = eGameStatus.Running;

            if(i_GameStatus == eGameStatus.Running && i_Button == (int)eButton.PauseMenu)
            {
                status = eGameStatus.Paused;
            }
            else
            {
                if (i_Button == (int)eButton.Exit)
                {
                    status = eGameStatus.Exited;
                }
                else if(i_Button == (int)eButton.Restart)
                {
                    status = eGameStatus.Restarted;
                }
                else if(i_Button == (int)eButton.Resume)
                {
                    status = eGameStatus.Running;
                }
                else if (i_Button != (int)eButton.Resume)
                {
                    status = i_GameStatus;
                }
            }

            return status;
        }

        private Point getValuesToAdd()
        { 
            Point values = new Point();

            if(m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                values = new Point(GameSettings.m_SpacingAroundButtons, GameSettings.m_SpacingAroundButtons);
            }
            else
            {
                values.Column = GameSettings.m_SpacingAroundButtons;
                values.Row = m_ClientScreenOurSize.Height*GameSettings.GameGridSize+GameSettings.m_SpacingAroundButtons;
            }

            return values;
        }

        public List<GameObject> GetMenuButtons(Point i_MenuPoint)// List<GameObject> o_GameObjectsToAdd)
        {
            List<eButton> buttonList = setPauseButtonList();
            List<GameObject> menuButtons = new List<GameObject>();
            foreach (var button in buttonList)
            {
                GameObject newButton = new GameObject();
                newButton.InitializeButton(
                    button,
                    "pause_menu_option_button.png",
                    getPauseMenuButtonPoint(button,i_MenuPoint),
                    false,
                    GameSettings.m_PauseMenuButtonOurSize, new Point(0, 0));
                newButton.Text = button.ToString();
                newButton.IsVisable = false;
                newButton.ZIndex = 1;
                if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
                {
                    newButton.SetImageDirection(Direction.getDirection(button.ToString()));
                }

                menuButtons.Add(newButton);
            }
            return menuButtons;
        }

        protected string generatePngString(eButton i_Button)
        {
            string png;

            png = "movebutton.png";// i_Button.ToString() + "button" + ".png";
            if(i_Button == eButton.PauseMenu)
            {
                png = "pausemenubutton.png";
            }
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
            else if (i_Button == eButton.Resume.ToString())
            {
                return eButton.Resume;
            }
            else if(i_Button == eButton.Restart.ToString())
            {
                return eButton.Restart;
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
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 2, 0);
                }
            }
            else
            {
                if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 2, 0);
                }
                else if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 2, 2);
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 1, 1);
                }
                else if(i_Type == eButton.Left)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 3, 1);
                }
                else if (i_Type == eButton.ButtonA)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 6, 1);
                }
                else if (i_Type == eButton.ButtonB)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenOurSize.Width - 8, 1);
                }
                else if (i_Type == eButton.PauseMenu)
                {
                    returnPoint.SetAndGetPoint(1, 2);
                }
            }
            return returnPoint;
        }

        private Point getPauseMenuButtonPoint(eButton i_Type, Point i_MenuPoint)
        {
            Point returnPoint = new Point(1, 1);
            int colum = (int)(i_MenuPoint.Column + GameSettings.GameGridSize * 0.5);

            if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if (i_Type == eButton.Resume)
                {
                    returnPoint.SetAndGetPoint(colum, (int)(i_MenuPoint.Row + GameSettings.GameGridSize * 3.5));
                }
                else if (i_Type == eButton.Restart)
                {
                    returnPoint.SetAndGetPoint(colum, i_MenuPoint.Row + GameSettings.GameGridSize * 2);
                }
                else if (i_Type == eButton.Exit)
                {
                    returnPoint.SetAndGetPoint(colum, (int)(i_MenuPoint.Row + GameSettings.GameGridSize * 0.5));
                }
            }
            else
            {
                if (i_Type == eButton.Resume)
                {
                    returnPoint.SetAndGetPoint(colum, (int)(i_MenuPoint.Row + GameSettings.GameGridSize*0.5));
                }
                else if (i_Type == eButton.Restart)
                {
                    returnPoint.SetAndGetPoint(colum, i_MenuPoint.Row + GameSettings.GameGridSize * 2);
                }
                else if (i_Type == eButton.Exit)
                {
                    returnPoint.SetAndGetPoint(colum, (int)(i_MenuPoint.Row + GameSettings.GameGridSize * 3.5));
                }
            }
            return returnPoint;
        }

        private List<eButton> setPauseButtonList()
        {
            List<eButton> buttonList = new List<eButton>();
            buttonList.Add(eButton.Resume);
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
