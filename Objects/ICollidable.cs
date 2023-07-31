using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Objects.Point;

namespace Objects
{
   // public delegate void PositionChangedEventHandler(object i_Collidable);
    public interface ICollidable
    {
        //event PositionChangedEventHandler PositionChanged;
        //event EventHandler<EventArgs> Disposed;
        public Rect Bounds { get; }

        bool IsCollisionDetectionEnabled { get; }
        public Point PointOnScreen { get;}
        bool CheckCollision(ICollidable i_Source);
        public Direction Direction { get; set; }
        void Collided(ICollidable i_Collidable);
    }
}
