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

        public List<Explosion> ExplosionsToAdd { get; private set; } = new List<Explosion>();

        private void expload()
        {
            Point point = base.GetPointOnGrid();
            int whatsInFront = -1;

            ExplosionsToAdd.Add(new Explosion(point));
            foreach (var direction in Direction.GetAllDirections())
            {
                Point nextPoint = point;

                for (int i = 0; i < 2; i++)
                {
                    if (checkDirectionForExplosion(nextPoint, direction, ref whatsInFront))
                    {
                        nextPoint = nextPoint.Move(direction);
                        ExplosionsToAdd.Add(new Explosion(nextPoint));
                        if (whatsInFront == 2)
                        {
                            Board[(int)point.Column, (int)point.Row] = 0;
                            break;
                        }
                    }
                }
            }
            OnSpecialEvent(ObjectNumber);
        }

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
                foreach(var explosion in ExplosionsToAdd)
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

        bool checkDirectionForExplosion(Point i_Point, Direction i_Direction, ref int i_WhatsInFront)
        {
            bool canChange = false;
            Point point = i_Point.Move(i_Direction);
 
            try
            {
                if (point.Row >= 0 && point.Column >= 0 && m_GameInformation.GameBoardSizeByGrid.Height > point.Row &&
                    m_GameInformation.GameBoardSizeByGrid.Width > point.Column)
                {
                    i_WhatsInFront = Board[(int)point.Column, (int)point.Row];
                    if (i_WhatsInFront != 1)
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
    }
}
