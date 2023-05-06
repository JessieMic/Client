using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;

namespace Objects
{
    public class Buttons
    {
        public Size m_MovementButtonSize = new Size(35,35);
        public ScreenDimension m_ClientScreenDimension = new ScreenDimension();
        private List<eButton> m_Buttons = new List<eButton>();
        public Size m_ClientScreenSize = new Size();
        public void GetGameButtons(ref List<GameObject> i_GameObjectsToAdd)
        {
            m_Buttons.Add(eButton.Right);
            m_Buttons.Add(eButton.Down);
            m_Buttons.Add(eButton.Left);
            m_Buttons.Add(eButton.Up);
            m_Buttons.Add(eButton.Menu);


            foreach (var button in m_Buttons)
            {
                GameObject newButton = new GameObject();
                newButton.InitializeButton(
                    button,
                    generatePngString(button),
                    getButtonPoint(button),
                    35,
                    m_MovementButtonSize,
                    getValuesToAdd(button));
                i_GameObjectsToAdd.Add(newButton);
            }
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
                values.m_Row = m_ClientScreenSize.m_Height*35+10;
            }
                return values;
        }

        protected string generatePngString(eButton i_Button)
        {
            string png;

            png = i_Button.ToString() + "button" + ".png";

            return png.ToLower();
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
                else
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenSize.m_Width - 1, 0);
                }
            }
            else
            {
                if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenSize.m_Width - 2, 0);
                }
                else if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenSize.m_Width - 2, 2);
                }
                else if (i_Type == eButton.Right)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenSize.m_Width - 1, 1);
                }
                else if(i_Type == eButton.Left)
                {
                    returnPoint.SetAndGetPoint(m_ClientScreenSize.m_Width - 3, 1);
                }
                else
                {
                    returnPoint.SetAndGetPoint(0, 2);
                }
            }
            return returnPoint;
        }
    }
}
