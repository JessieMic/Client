using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects;
using Objects.Enums;
using Point = Objects.Point;

namespace Objects
{
    public class Passage : GameObject
    {
        public override bool IsCollisionDetectionEnabled => true;
        public Passage(Point i_Point)
        {
            Point point = m_GameInformation.PointValuesToAddToScreen;
            //point.Row += 20;
            //point.Column += 15;
            ObjectNumber = 2;
            this.Initialize(eScreenObjectType.Image, 2, "pac7man_boarder.png", i_Point, true,
                point);
            Direction = Direction.Right;
            //this.m_Size.Height = 10;
            //this.m_Size.Width = 10;
        }
    }
}


//using DTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Objects
//{
//    public class Passage : ICollidable
//    {

//        public virtual bool IsCollisionDetectionEnabled { get; }
//        protected GameInformation m_GameInformation = GameInformation.Instance;
//        private Point m_ValuesToAdd;
//        private Point m_Point;
//        public Point PointOnScreen { get; set; }

//        public Passage(Point i_Point)
//        {
//            setScreenPoint(i_Point);
//            m_ValuesToAdd = m_GameInformation.PointValuesToAddToScreen;
//        }

//        public Direction Direction { get; set; } = Direction.Right;

//        public Rect Bounds
//        {
//            get
//            {
//                int size = GameSettings.GameGridSize;
//                return new Rect(m_Point.Column, m_Point.Row,
//                   size, size);
//            }
//        }

//        public virtual bool CheckCollision(ICollidable i_Source)
//        {
//            return false;
//        }

//        public void Collided(ICollidable i_Collidable)
//        {
//        }
//        private void setScreenPoint(Point i_Point)
//        {
//            Point point = new Point();

//            point = i_Point;
//            point.Column = i_Point.Column * GameSettings.GameGridSize + m_ValuesToAdd.Column;
//            point.Row = i_Point.Row * GameSettings.GameGridSize + m_ValuesToAdd.Row;
//            PointOnScreen = m_Point = point;
//        }
//    }
//}
