using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Enums;
using Point = Objects.Point;
using Size = Objects.Size;
using Objects;

using Objects.Enums.BoardEnum;

namespace LogicUnit.Logic.GamePageLogic.Games.Pacman
{
    public class Pacman : Game
    {
        Ghost m_Ghost; 
        public Pacman()
        {
            m_GameName = "Snake";
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 3;
            m_TypeOfGameButtons = eTypeOfGameButtons.MovementButtonsForAllDirections;
            m_Hearts.m_AmountOfLivesPlayersGetAtStart = 1;
        }

        public override void RunGame()
        {
            gameLoop();
        }

        protected override async Task gameLoop()
        {
            while (m_GameStatus == eGameStatus.Running)
            {
                await Task.Delay(400);

                m_Ghost.m_Direction = Direction.Right;
                m_Ghost.MoveSameDirection();
                await Task.Delay(400);
                m_Ghost.m_Direction = Direction.Down;
                m_Ghost.MoveSameDirection();
                await Task.Delay(400);
                m_Ghost.MoveToPoint(new Point(1, 1));
                
            }
        }

        protected override void AddGameObjects()
        {
            m_Ghost = new Ghost();

            GameObject obj = addGameBoardObject(eScreenObjectType.Player, new Point(1, 1), 1, 1, "body");

            m_Ghost.set(obj);
        }
    }
}
