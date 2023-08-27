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
        private short update = 0;
        private short saftyUpdate = 0;
        bool isOnScreen=false;
        public void RequestDirection(Direction i_Direction)
        {
            if (Movable.IsObjectMoving)
            {
                isOnScreen = false;
                if (m_GameInformation.IsPointIsOnBoardPixels(Movable.PointOnScreen))
                {
                    isOnScreen = true;
                    saftyUpdate = 0;
                }

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

                if (isOnScreen && update == 20)
                {
                    Point PointUpdate = Movable.GetPointOnGrid();
                    Movable.OnUpdatePosition(PointUpdate);
                    update = 0;
                }
                else if(saftyUpdate > 70)
                {
                    if(m_GameInformation.Player.PlayerNumber == 1)
                    {
                        Point PointUpdate = Movable.GetPointOnGrid();
                        System.Diagnostics.Debug.WriteLine($"{isOnScreen} DDDDDDDD");
                        Movable.OnUpdatePosition(PointUpdate);
                    }
                    update = 0;
                    saftyUpdate = 0;
                }
                update++;
                saftyUpdate++;
                Movable.SetImageDirection(Movable.Direction);
            }
        }

        void checkIfWantToTurn(Direction i_Direction)
        {
            int x = Movable.Direction.ColumnOffset + i_Direction.ColumnOffset;
            int y = Movable.Direction.RowOffset + i_Direction.RowOffset;

            if (x != 0 && y != 0)
            {
                Point PointUpdate = Movable.GetPointOnGrid();

                Movable.WantToTurn = true;
                Movable.PointOnScreen = Movable.GetScreenPoint(PointUpdate, true);
                if(isOnScreen)
                {
                    Movable.OnUpdatePosition(PointUpdate);
                    update = 0;
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
