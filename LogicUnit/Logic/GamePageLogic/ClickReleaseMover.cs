using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic
{
    internal class ClickReleaseMover
    {
        public IMovable Movable { get; set; }
        protected GameInformation m_GameInformation = GameInformation.Instance;
        private short update = 0;
        private  bool m_IsOnScreen = false;

        public void RequestDirection(Direction i_Direction)
        {
            if (Movable.IsObjectMoving)
            {
                m_IsOnScreen = false;
                if (m_GameInformation.IsPointIsOnBoardPixels(Movable.PointOnScreen))
                {
                    m_IsOnScreen = true;
                    update = 0;
                }

                if (i_Direction == Direction.Stop && Movable.Direction != Direction.Stop)
                {
                    Movable.RequestedDirection = Movable.Direction;
                    if (m_IsOnScreen)
                    {
                        Point PointUpdate = Movable.GetPointOnGrid();
                       // Movable.OnUpdatePosition(PointUpdate);
                    }
                }

                Movable.Direction = i_Direction;

                if (Movable.Direction != Direction.Stop)
                {
                    if (checkIfCanChangeDirection(i_Direction))
                    {
                        checkIfWantToTurn(i_Direction);
                        Movable.Direction = i_Direction;
                    }

                    Movable.SetImageDirection(Movable.Direction);
                }

                if(update > 70 && m_GameInformation.Player.PlayerNumber == 1)
                {
                    Point PointUpdate = Movable.GetPointOnGrid();
                    //Movable.OnUpdatePosition(PointUpdate);
                    update = 0;
                }
                update++;
            }
        }

        void checkIfWantToTurn(Direction i_Direction)
        {
            int x = Movable.RequestedDirection.ColumnOffset + i_Direction.ColumnOffset;
            int y = Movable.RequestedDirection.RowOffset + i_Direction.RowOffset;

            if (x != 0 && y != 0)
            {
                Point PointUpdate = Movable.GetPointOnGrid();
                Movable.WantToTurn = true;
                Movable.RequestedDirection = i_Direction;
                Movable.PointOnScreen = Movable.GetScreenPoint(PointUpdate, true);
                if (m_IsOnScreen)
                {
                   // Movable.OnUpdatePosition(PointUpdate);
                }
            }
        }

        bool checkIfCanChangeDirection(Direction i_Direction)
        {
            bool canChange = false;
            Point point = Movable.GetPointOnGrid().Move(i_Direction);

            if (point.Row >= 0 && point.Column  >= 0 && m_GameInformation.GameBoardSizeByGrid.Height > point.Row  &&
                                                       m_GameInformation.GameBoardSizeByGrid.Width > point.Column)
            {
                canChange = true;
            }

            return canChange;
        }
    }
}
