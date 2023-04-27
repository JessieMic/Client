using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
using Size = Objects.Size;


namespace LogicUnit
{
    public abstract partial class Game
    {
        public event EventHandler<List<ScreenObjectAdd>> AddScreenObject;
        public event EventHandler<List<ScreenObjectUpdate>> GameObjectUpdate;
        protected List<ScreenObjectAdd> m_ScreenObjectList;
        protected virtual void OnAddScreenObjects(List<ScreenObjectAdd> i_ScreenObject)
        {
            AddScreenObject.Invoke(this, i_ScreenObject);
        }

        public void SetGameScreen()
        {
            m_ScreenObjectList = setGameButtons();
            m_ScreenObjectList.Add(setGameBackGround());
            PlayerObject = m_ScreenMapping.m_ValueToAdd;
            AddGameObjects();
            OnAddScreenObjects(m_ScreenObjectList);
        }

        protected abstract void AddGameObjects();

        protected List<ScreenObjectAdd> setGameButtons()
        {
            m_Buttons.m_MovementButtonSize = m_ScreenMapping.m_MovementButtonSize;
            m_Buttons.m_ClientScreenDimension = m_GameInformation.m_ClientScreenDimension;
           
            return m_Buttons.GetGameButtons();
        }

        protected ScreenObjectAdd setGameBackGround()
        {
            ScreenObjectAdd returnObject = new ScreenObjectAdd();
            Size actualSize = new Size(m_ScreenMapping.m_TotalScreenSize.m_Width*m_ScreenMapping.m_GameBoardGridSize,m_ScreenMapping.m_TotalScreenSize.m_Height*m_ScreenMapping.m_GameBoardGridSize);
            returnObject = new ScreenObjectAdd(eScreenObjectType.Image, null, m_ScreenMapping.m_ValueToAdd, actualSize, "aa.png", null,0);

            return returnObject;
        }
    }
}
