using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic
{
    internal class ClickMover
    {
        public IMovable Movable { get; set; }
        protected GameInformation m_GameInformation = GameInformation.Instance;
        public void RequestDirection(Direction i_Direction)
        {
            if (Movable.IsObjectMoving)
            {
                if (Movable.Direction == Direction.Stop)
                {
                    Movable.Direction = i_Direction;
                }
                else if (Movable.Direction != i_Direction)
                {
                    Movable.RequestedDirection = i_Direction;
                    if (checkIfCanChangeDirection(i_Direction))
                    {
                        checkIfWantToTurn(i_Direction);
                        Movable.Direction = i_Direction;
                    }
                }
                Movable.SetImageDirection(Movable.Direction);
            }
        }

        void checkIfWantToTurn(Direction i_Direction)
        {
            int x = Movable.Direction.ColumnOffset + i_Direction.ColumnOffset;
            int y = Movable.Direction.RowOffset + i_Direction.RowOffset;

            if (x != 0 && y != 0)
            {
                Movable.WantToTurn = true;
                Point PointUpdate = Movable.GetPointOnGrid();
                //check if we are on our side
                Movable.PointOnScreen = Movable.GetScreenPoint(PointUpdate, true);
                if (m_GameInformation.IsPointIsOnBoardPixels(Movable.PointOnScreen))
                {
                    Movable.OnUpdatePosition( PointUpdate);
                }
            }
        }

        bool checkIfCanChangeDirection(Direction i_Direction)
        {
            bool canChange = false;
            Point point = Movable.GetPointOnGrid();
            try
            {
                if (point.Row + i_Direction.RowOffset >= 0 && point.Column + i_Direction.ColumnOffset >= 0
                    && m_GameInformation.GameBoardSizeByGrid.Height > point.Row + i_Direction.RowOffset &&
                        m_GameInformation.GameBoardSizeByGrid.Width > point.Column + i_Direction.ColumnOffset)
                {
                    if (Movable.Board[(int)point.Column + i_Direction.ColumnOffset, (int)point.Row + i_Direction.RowOffset] != 1)
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
