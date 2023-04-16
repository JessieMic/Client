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
        public event EventHandler<List<ScreenObject>> AddScreenObject;
        public event EventHandler<Point> GameObjectUpdate;

        protected virtual void OnAddScreenObjects(List<ScreenObject> i_ScreenObject)
        {
            AddScreenObject.Invoke(this, i_ScreenObject);
        }

        public void SetGameScreen()
        {
            List<ScreenObject> screenObjectList;

            screenObjectList = setGameButtons();
            screenObjectList.Add(setGameBackGround());
            GameObjectUpdate.Invoke(this,m_ScreenMapping.m_ValueToAdd);
            PlayerObject = m_ScreenMapping.m_ValueToAdd;
            OnAddScreenObjects(screenObjectList);
        }

        protected List<ScreenObject> setGameButtons()
        {
            
            m_Buttons.m_MovementButtonSize = m_ScreenMapping.m_MovementButtonSize;
            m_Buttons.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
            m_Buttons.m_ScreenMapping = m_ScreenMapping;

            return m_Buttons.GetGameButtons();
        }

        protected ScreenObject setGameBackGround()
        {
            ScreenObject returnObject = new ScreenObject();
            Size actualSize = new Size(m_ScreenMapping.m_TotalScreenSize.m_Width*m_ScreenMapping.m_GameBoardGridSize,m_ScreenMapping.m_TotalScreenSize.m_Height*m_ScreenMapping.m_GameBoardGridSize);
            returnObject = new ScreenObject(eScreenObjectType.Image, null, m_ScreenMapping.m_ValueToAdd, actualSize, "aa.png", null);

            return returnObject;
        }
    }
}
