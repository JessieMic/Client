using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public delegate void PositionChangedEventHandler(object i_Collidable);
    public class ICollidable
    {
        event PositionChangedEventHandler PositionChanged;
        event EventHandler<EventArgs> Disposed;
        bool Visible { get; }
        //bool CheckCollision(ICollidable i_Source);
        //void Collided(ICollidable i_Collidable);
    }
}
