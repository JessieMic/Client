using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic;
using Objects;
using Objects.Enums;
using Objects.Enums.Snake;
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

        protected override void setGameBoardAndGameObjects()
        {

        }

        protected override void AddGameObjects()
        {
            addPlayerObjects();
            addFood();
        }

        private void addPlayerObjects()
        {
            m_PlayerGameObjects.Add(new GameObject(eScreenObjectType.PlayerObject,1,m_ScreenMapping.m_GameBoardGridSize,m_ScreenMapping.m_ValueToAdd));
            for (int col = 3; col < 7; col++)
            {
                ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.PlayerObject, null, new Point(col,1), m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1);
                m_PlayerGameObjects[0].SetObject(ref obj);
                m_ScreenObjectList.Add(obj);
                m_Board[col, 1] = (int)eBoardObject.Snake1;
            }
        }

        private void addFood()
        {
            List<Point> emptyPositions = getEmptyPositions();
            Point randomPoint = emptyPositions[m_randomPosition.Next(emptyPositions.Count)];

            m_gameObjects.Add(new GameObject(eScreenObjectType.GameObject, 1, m_ScreenMapping.m_GameBoardGridSize, m_ScreenMapping.m_ValueToAdd));
            ScreenObjectAdd obj = new ScreenObjectAdd(eScreenObjectType.GameObject, null, randomPoint, m_ScreenMapping.m_MovementButtonSize, "player.png", string.Empty, 1);
            m_gameObjects[0].SetObject(ref obj);
            m_ScreenObjectList.Add(obj);
            m_Board[randomPoint.m_Column, randomPoint.m_Row] = (int)eBoardObject.Food;
        }

        private void initializeGame()
        {
            m_TypeOfGameButtons = eTypeOfGameButtons.MovementButtonsForAllDirections;
            m_AmountOfLivesTheClientHas = 3;
        }

        private List<Point> getEmptyPositions()
        {
            List<Point> res = new List<Point>();

            for(int col = 0; col < m_BoardSize.m_Width; col++)
            {
                for(int row = 0 ;row < m_BoardSize.m_Height; row++)
                {
                    if(m_Board[col, row] == 0)
                    {
                        res.Add(new Point(col,row));
                    }
                }
            }

            return res;
        }
    }
}
