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
        public void GetGameButtons(ref List<GameObject> i_GameObjectsToAdd)
        {
            m_Buttons.Add(eButton.Right);
            m_Buttons.Add(eButton.Down);
            m_Buttons.Add(eButton.Left);
            m_Buttons.Add(eButton.Up);


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

        //protected void addBasicGameButtons(ref List<GameObject> i_GameObjectsToAdd)
        //{
        //    m_Buttons.Add(eButton.Right);
        //    m_Buttons.Add(eButton.Down);
        //    m_Buttons.Add(eButton.Left);
        //    m_Buttons.Add(eButton.Up);

                           

        //    //returnButtons.Add(new ScreenObjectAdd(eScreenObjectType.Button, eButton.Right, getButtonPoint(eButton.Right), m_MovementButtonSize, string.Empty, null,0));
        //    //returnButtons.Add(new ScreenObjectAdd(eScreenObjectType.Button, eButton.Left, getButtonPoint(eButton.Left), m_MovementButtonSize, string.Empty, null,0));
        //    //returnButtons.Add(new ScreenObjectAdd(eScreenObjectType.Button, eButton.Down, getButtonPoint(eButton.Down), m_MovementButtonSize, string.Empty, null, 0));
        //    //returnButtons.Add(new ScreenObjectAdd(eScreenObjectType.Button, eButton.Up, getButtonPoint(eButton.Up), m_MovementButtonSize, string.Empty, null, 0));

           

        //    foreach(var button in m_Buttons)
        //    {
        //        GameObject newButton = new GameObject();

        //        i_GameObjectsToAdd.Add(newButton.Initialize(eScreenObjectType.Button,));

        //        i_GameObjectsToAdd.Add(newButton.Initialize(eScreenObjectType.Button, eButton.Right, generatePngString(eButton.Right)
        //            , getButtonPoint(eButton.Right), GameSettings.m_GameBoardGridSize, GameSettings.m_MovementButtonSize, getValuesToAdd(eButton.Right));
        //    }
        //    List<GameObject> gameObjects = new List<GameObject>();
            
            
        //}

        private Point getValuesToAdd(eButton i_Button)
        { 
            Point values = new Point(10,10);

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
            Point returnPoint = new Point();
            //int space = GameSettings.m_SpacingAroundButtons;///m_GameSettings.m_SpacingAroundButtons;
            //Size buttonSize = GameSettings.m_GameBoardGridSize;//gam_GameSettings.m_MovementButtonSize;

            if (m_ClientScreenDimension.Position.Row == eRowPosition.UpperRow)
            {
                if (i_Type == eButton.Down)
                {
                    returnPoint.SetAndGetPoint(1, 2);
                    //returnPoint.SetAndGetPoint(buttonSize.m_Width + space, space + 2 * buttonSize.m_Height);
                }
                else if (i_Type == eButton.Up)
                {
                    returnPoint.SetAndGetPoint(1, 0);
                    //returnPoint.SetAndGetPoint(buttonSize.m_Width + space, space);
                }
                else if (i_Type == eButton.Left)
                {
                    returnPoint.SetAndGetPoint(0,1);
                    //returnPoint.SetAndGetPoint(space, space + buttonSize.m_Height);
                }
                else
                {
                    returnPoint.SetAndGetPoint(2, 1);
                    //returnPoint.SetAndGetPoint(space + 2 * buttonSize.m_Width, space + buttonSize.m_Height);
                }
            }
            else
            {
                returnPoint.SetAndGetPoint(1, 2);
                //int screenWidth = m_ClientScreenDimension.Size.m_Width;
                //int screenHeight = m_ClientScreenDimension.Size.m_Height;
                //if (i_Type == eButton.Up)
                //{
                //    returnPoint.SetAndGetPoint(screenWidth - (space + 2 * buttonSize.m_Width), screenHeight - (space + 3 * buttonSize.m_Height));
                //}
                //else if (i_Type == eButton.Down)
                //{
                //    returnPoint.SetAndGetPoint(screenWidth - (space + 2 * buttonSize.m_Width), screenHeight - (space + buttonSize.m_Height));
                //}
                //else if (i_Type == eButton.Right)
                //{
                //    returnPoint.SetAndGetPoint(screenWidth - (space + buttonSize.m_Width), screenHeight - (space + 2 * buttonSize.m_Height));
                //}
                //else
                //{
                //    returnPoint.SetAndGetPoint(screenWidth - (space + 3 * buttonSize.m_Width), screenHeight - (space + 2 * buttonSize.m_Height));
                //}
            }
            return returnPoint;
        }
    }
}
