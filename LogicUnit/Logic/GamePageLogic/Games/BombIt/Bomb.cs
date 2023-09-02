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
        public Explosion[] Explosions{ get; private set; } = new Explosion[9];

        private void SetExplosions()
        {
            Point point = base.GetPointOnGrid();
            int whatsInFront = -1;
            int index = 1;
            Explosions[0].Ignite(point);
            foreach (var direction in Direction.GetAllDirections())
            {
                Point nextPoint = point;

                for (int i = 0; i < 2; i++)
                {
                    if (checkDirectionForExplosion(nextPoint, direction, ref whatsInFront))
                    {
                        nextPoint = nextPoint.Move(direction);
                        Explosions[index].Ignite(nextPoint);
                        index++;
                        if (whatsInFront == 2)
                        {
                            Board[(int)point.Column, (int)point.Row] = 0;
                            break;
                        }
                    }
                }
            }
        }

        public void Drop(Point i_Point)
        {
            MoveToPointInGrided(i_Point);
            ChangeState(true);
        }

        public Bomb(ref int[,] i_Board, int i_PlayerNumber)
        {
            MonitorForCollision = true;
            ChangeState(false);
            Board = i_Board;
            this.Initialize(eScreenObjectType.Image, i_PlayerNumber, $"bomb.png",new Point(0,0), true,
                m_GameInformation.PointValuesToAddToScreen);
            addExplosions();
        }

        public void ChangeState(bool i_State)
        {
            IsVisable = i_State;
            IsCollisionEnabled = i_State;
            OnUpdate();
        }

        private void addExplosions()
        {
            for(int i = 0; i < 9; i++)
            {
                Explosions[i]= new Explosion();
            }
        }

        public override void Update(double i_TimeElapsed)
        {
            if(IsVisable = false && !m_HasBombExploded)
            {
                ChangeState(true);
            }

            if (i_TimeElapsed > 3500)
            {
                StopExplosion();
            }
            else if (!m_HasBombExploded && i_TimeElapsed > 2500)
            {
                m_HasBombExploded = true;
                ChangeState(false);
                SetExplosions();
            }
        }

        public void StopExplosion()
        {
            foreach (var explosion in Explosions)
            {
                explosion.ChangeState(false);
            }
            ChangeState(false);
            m_HasBombExploded = false;
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
