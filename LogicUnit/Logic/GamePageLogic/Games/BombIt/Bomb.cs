using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUnit.Logic.GamePageLogic.Games.Pacman;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.BombIt
{
    internal class Bomb : GameObject
    {

        public Bomb(int i_X, int i_Y, int[,] i_Board, int i_PlayerNumber)
        {
            IsCollisionDetectionEnabled = true;
            Board = i_Board;
            this.Initialize(eScreenObjectType.Player, i_PlayerNumber, $"bomb.png", new Point(i_X, i_Y), true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            if (i_TimeElapsed > 3000)
            {
                IsVisable = false;
                OnUpdate();
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is PacmanObject)
            {

            }
            else if (i_Collidable is Boarder)
            {
                collidedWithSolid(i_Collidable);
            }
        }
    }
}
