using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic
{
    public delegate void PositionChangedEventHandler(object i_Collidable);
    internal interface ICollidable
    {
        event PositionChangedEventHandler PositionChanged;
        event EventHandler<EventArgs> Disposed;
        bool Visible { get; }
        bool CheckCollision(ICollidable i_Source);
        void Collided(ICollidable i_Collidable);
    }
}
