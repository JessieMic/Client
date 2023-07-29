using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Objects;
using Objects.Enums.BoardEnum;
using Point = Objects.Point;

namespace LogicUnit.Logic.GamePageLogic.Games.Snake
{
    [Serializable]
    public class SnakeObject : GameObject 
    {
        //private int[,] m_Board;
        //public List<Direction> m_DirectionsForTail = new List<Direction>();
        ////private List<Ga>
        ////public SnakeObject()
        ////{
            
        ////}
        //public SnakeObject(ref int[,] i_Board)
        //{
        //    m_Board = i_Board;
        //}

        //public void Eat(GameObject i, Direction i_LastDirection, Direction i_CurrentDirection)
        //{
        //    CombineGameObjectsTop(i);
        //    SetImageDirection(0, i_CurrentDirection);

        //    if (i_LastDirection != i_CurrentDirection)
        //    {
        //        turnBody(i_LastDirection, i_CurrentDirection);
        //        m_ImageSources[1] = "snakeplayer" + m_ObjectNumber.ToString() + "turn.png";
        //        m_DirectionsForTail.Add(i_CurrentDirection);
        //    }
        //    else
        //    {
        //        m_ImageSources[1] = "snakeplayer"+ m_ObjectNumber.ToString() +"body.png";
        //        SetImageDirection(1, i_CurrentDirection);
        //    }
        //}

        //public void Move(Point i_NewPoint, Direction i_LastDirection,Direction i_CurrentDirection)
        //{
        //    removeTail();
        //    addHead(i_NewPoint);
        //    moveTail();
        //    moveBody(i_CurrentDirection);
        //    if (i_LastDirection != i_CurrentDirection)
        //    {
        //        SetImageDirection(0, i_CurrentDirection);//Set head to a new direction
        //        turnBody(i_LastDirection, i_CurrentDirection);
        //        m_ImageSources[1] = "snakeplayer" + m_ObjectNumber.ToString() + "turn.png";
        //        m_DirectionsForTail.Add(i_CurrentDirection);
        //    }
        //}

        //private void turnBody(Direction i_LastDirection, Direction i_CurrentDirection)
        //{
        //    if((i_LastDirection == Direction.Right && i_CurrentDirection == Direction.Up)
        //       || (i_LastDirection == Direction.Down && i_CurrentDirection == Direction.Left))
        //    {
        //        m_Rotatation[1] = 180;
        //    }
        //    else if((i_LastDirection == Direction.Right && i_CurrentDirection == Direction.Down)
        //            || (i_LastDirection == Direction.Up && i_CurrentDirection == Direction.Left))
        //    {
        //        m_Rotatation[1] = 90;
        //    }
        //    else if ((i_LastDirection == Direction.Down && i_CurrentDirection == Direction.Right)
        //             || (i_LastDirection == Direction.Left && i_CurrentDirection == Direction.Up))
        //    {
        //        m_Rotatation[1] = 270;
        //    }
        //    else
        //    {
        //        m_Rotatation[1] = 0;
        //    }
        //}

        //private void moveBody(Direction i_CurrentDirection)
        //{
        //    for(int i = getAmountOfCombinedObjects() - 2; i > 1; i--)
        //    {
        //        m_ImageSources[i] = m_ImageSources[i - 1];
        //        copyNextImageDirection(i);
        //    }

        //    m_ImageSources[1] = "snakeplayer" + m_ObjectNumber.ToString() + "body.png";
        //    SetImageDirection(1, i_CurrentDirection);
        //}

        //private void moveTail()
        //{
        //    if (m_ImageSources[getAmountOfCombinedObjects() - 2] == "snakeplayer" + m_ObjectNumber.ToString() + "body.png")
        //    {
        //        copyNextImageDirection(getAmountOfCombinedObjects() - 1);
        //    }
        //    else
        //    {
        //        SetImageDirection(getAmountOfCombinedObjects() - 1, m_DirectionsForTail[0]);
        //        //m_Rotatation[getAmountOfCombinedObjects() - 1] += 180;
        //        m_DirectionsForTail.RemoveAt(0);
        //        m_ImageSources[getAmountOfCombinedObjects() - 1] = "snakeplayer" + m_ObjectNumber.ToString() + "tail.png";
        //    }
        //}

        //private void copyNextImageDirection(int i_Index)
        //{
        //    m_Rotatation[i_Index] = m_Rotatation[i_Index - 1];
        //}

        //private Direction getDirectionOfTurnPart()
        //{
        //    Direction direction = Direction.Down;

        //    if (m_Rotatation[getAmountOfCombinedObjects() - 2] != 0 ||
        //        m_ScaleY[getAmountOfCombinedObjects() - 2] != 1)
        //    {
        //        direction = Direction.Up;
        //    }

        //    return direction;
        //}

        //private bool isPartTurnBodyPart()
        //{
        //    bool isPartTurnBodyPart = false;

        //    if(m_ImageSources[getAmountOfCombinedObjects() - 2][12] == 't')
        //    {
        //        isPartTurnBodyPart = true;
        //    }

        //    return isPartTurnBodyPart;
        //}

        //public Point getSnakeHead()
        //{
        //    return m_PointsOnGrid.First();
        //}

        //public Point getSnakeTail()
        //{
        //    return m_PointsOnGrid.Last();
        //}

        //public void addHead(Point i_Point)
        //{
        //    AddPointTop(i_Point);
        //    m_Board[i_Point.Column, i_Point.Row] = m_ObjectNumber + 2;
        //}

        //public void removeTail()
        //{
        //    Point tail = getSnakeTail();
        //    if(m_Board[tail.Column, tail.Row] == m_ObjectNumber + 2)
        //    {
        //        m_Board[tail.Column, tail.Row] = 0;
        //    }
        //    PopPoint();
        //}

        //public Point GetOneMoveAhead()
        //{
        //    Point newHeadPoint = getSnakeHead().Move(m_Direction);

        //    return newHeadPoint;
        //}

        //public int whatSnakeWillHit(Point i_Point,bool i_IsPointOnTheBoard)//new snake head
        //{
        //    int res;
        //    Point newHeadPoint = getSnakeHead().Move(m_Direction);

        //    if (!i_IsPointOnTheBoard)
        //    {
        //        res = (int)eBoardObjectSnake.OutOfBounds;
        //    }
        //    else if (i_Point == getSnakeTail())
        //    {
        //        res = (int)eBoardObjectSnake.Empty;
        //    }
        //    else
        //    {
        //        res = m_Board[i_Point.Column, i_Point.Row];
        //    }

        //    return res;
        //}

        //public void deleteSnake()
        //{
        //    foreach (var point in m_PointsOnGrid)
        //    {
        //        m_Board[point.Column, point.Row] = (int)eBoardObjectSnake.Empty;
        //    }
        //}

        ////public SnakeObject Clone()
        ////{
        ////    string json = JsonSerializer.Serialize(this);
        ////    return JsonSerializer.Deserialize<SnakeObject>(json);
        ////}
    }
}

