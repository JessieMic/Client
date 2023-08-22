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
        private bool m_HasBombExploded = false;
        public List<Explosion> m_Explosions = new List<Explosion>();

        public Bomb(Point i_Point, int[,] i_Board, int i_PlayerNumber)
        {
            IsCollisionDetectionEnabled = true;
            Board = i_Board;
            this.Initialize(eScreenObjectType.Image, i_PlayerNumber, $"bomb.png", i_Point, true,
                m_GameInformation.PointValuesToAddToScreen);
        }

        public override void Update(double i_TimeElapsed)
        {
            
            if (i_TimeElapsed > 3500)
            {
                foreach(var explosion in m_Explosions)
                {
                    explosion.OnDisposed();
                }
                OnDisposed();
            }
            else if (!m_HasBombExploded && i_TimeElapsed > 2500)
            {
                m_HasBombExploded = true;
                IsVisable = false;
                expload();
                OnUpdate();
            }
        }

        private void expload()
        {
            Point point = base.GetPointOnGrid();

            m_Explosions.Add(new Explosion(point));
            foreach(var direction in Direction.GetAllDirections())
            {
                Point p = point;
                if(checkDirectionForExplosion(p, direction))
                {
                    p = p.Move(direction);
                    m_Explosions.Add(new Explosion(p));
                    if (checkDirectionForExplosion(p, direction))
                    {
                        p = p.Move(direction);
                        m_Explosions.Add(new Explosion(p));
                    }
                }
            }
            OnSpecialEvent(ObjectNumber);
        }

        bool checkDirectionForExplosion(Point i_Point, Direction i_Direction)
        {
            bool canChange = false;
            try
            {
                if (i_Point.Row + i_Direction.RowOffset >= 0 && i_Point.Column + i_Direction.ColumnOffset >= 0)
                {
                    if (Board[(int)i_Point.Column + i_Direction.ColumnOffset, (int)i_Point.Row + i_Direction.RowOffset] != 1)
                    {
                        canChange = true;
                    }
                }
            }
            catch (Exception e)
            {
                canChange = false;
            }

            return canChange;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Boarder)
            {
                collidedWithSolid(i_Collidable);
            }
        }
    }
}
