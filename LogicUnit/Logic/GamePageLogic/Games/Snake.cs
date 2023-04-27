using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic;
using Objects;
using Objects.Enums;
using Point = Objects.Point;
namespace LogicUnit
{
    public class Snake : Game
    {
        public Snake()
        {
            
        }

        public override void RunGame()
        {

        }

        protected override void AddGameObjects()
        {
            Point p = m_ScreenMapping.m_ValueToAdd;
            ScreenObjectAdd obj;
            obj= new ScreenObjectAdd(eScreenObjectType.PlayerObject,null,p, m_ScreenMapping.m_MovementButtonSize, "player.png",string.Empty,1);
            m_ScreenObjectList.Add(obj);
            m_PlayerGameObjects.Add(new GameObject(eScreenObjectType.PlayerObject,1));
            m_PlayerGameObjects[0].m_Positions.Add(m_ScreenMapping.m_ValueToAdd);
            
            p.m_Column += 35;
            m_ScreenObjectList.Add(new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, p, m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1));
            m_PlayerGameObjects[0].m_Positions.Add(p);
            p.m_Column += 35;
            m_PlayerGameObjects[0].m_Positions.Add(p);


            m_PlayerGameObjects[0].m_ImageSources.Add("player.png");
            m_PlayerGameObjects[0].m_ImageSources.Add("player.png");
            m_PlayerGameObjects[0].m_ImageSources.Add("player.png");
            
            m_ScreenObjectList.Add(new ScreenObjectAdd(eScreenObjectType.PlayerObject, null,p, m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1));
        }

        private void initializeGame()
        {
            m_TypeOfGameButtons = eTypeOfGameButtons.MovementButtonsForAllDirections;
            m_AmountOfLivesTheClientHas = 3;
        }
    }
}
