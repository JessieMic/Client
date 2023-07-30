using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
   // public delegate void PositionChangedEventHandler(object i_Collidable);
    public interface ICollidable
    {
        //event PositionChangedEventHandler PositionChanged;
        //event EventHandler<EventArgs> Disposed;
        public Rect Bounds { get; }

        bool IsCollisionDetectionEnabled { get; }

        // List<ICollidable> CollideList;// = new List<ICollidable>();
        //public bool Visible { get; }
        bool CheckCollision(ICollidable i_Source);
        void Collided(ICollidable i_Collidable);
    }
}
