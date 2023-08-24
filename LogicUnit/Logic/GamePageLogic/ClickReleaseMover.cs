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
        public void RequestDirection(Direction i_Direction)
        {
            if (Movable.IsObjectMoving)
            {
                if (i_Direction == Direction.Stop && Movable.Direction != Direction.Stop)
                {
                    Movable.RequestedDirection = Movable.Direction;
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
            }
        }

        void checkIfWantToTurn(Direction i_Direction)
        {
            int x = Movable.RequestedDirection.ColumnOffset + i_Direction.ColumnOffset;
            int y = Movable.RequestedDirection.RowOffset + i_Direction.RowOffset;

            if (x != 0 && y != 0)
            { 
                Movable.WantToTurn = true;
                Movable.RequestedDirection = i_Direction;
                Point PointUpdate = Movable.GetPointOnGrid();
                //check if we are on our side
                Movable.PointOnScreen = Movable.GetScreenPoint(PointUpdate, true);
                if (m_GameInformation.IsPointIsOnBoardPixels(Movable.PointOnScreen))
                {
                    Movable.OnUpdatePosition(PointUpdate);
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
