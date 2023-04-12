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



        protected virtual void OnAddScreenObject(ScreenObject i_ScreenObject)
        {
            AddScreenObject.Invoke(this, i_ScreenObject);
        }

        public void SetGameScreen()
        {
            //m_GameInformation.m_ClientScreenDimension.Position.Row = eRowPosition.LowerRow;
            //m_GameInformation.m_ClientScreenDimension.Position.Column = eColumnPosition.LeftColumn;

            setGameButtons();
            setGameBackGround();
            setGameSpacing();
        }

        protected void setGameButtons()
        {
            Point point = new Point(2, 1);
            Size size = new Size(35, 35);
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, getTypeOfButton(point), point, size, string.Empty, null));
            point.SetAndGetPoint(3, 2);
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, getTypeOfButton(point), point, size, string.Empty, null));
            point.SetAndGetPoint(1, 2);
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, getTypeOfButton(point), point, size, string.Empty, null));
            point.SetAndGetPoint(2, 3);
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Button, getTypeOfButton(point), point, size, string.Empty, null));
        }

        protected void setGameSpacing()
        {

            OnAddScreenObject(new ScreenObject(eScreenObjectType.Space, null, getSpacingPoint(), new Size(10, 10), string.Empty, null));
        }

        protected void setGameBackGround()
        {
            OnAddScreenObject(new ScreenObject(eScreenObjectType.Image, null, getBackgroundPoint(), new Size(m_GameInformation.m_ClientScreenDimension.m_Size.m_Width-115,0), "aa.png", null));
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
