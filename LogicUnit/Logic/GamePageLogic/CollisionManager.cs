using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicUnit.Logic.GamePageLogic
{
    internal class CollisionManager : ICollisionsManager
    {
        protected readonly List<ICollidable> m_Collidables = new List<ICollidable>();
        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if(!this.m_Collidables.Contains(i_Collidable))
            {
                this.m_Collidables.Add(i_Collidable);
                //i_Collidable.PositionChanged += collidable_PositionChanged;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_PositionChanged(ICollidable i_Collidable)
        {
            List<ICollidable> collidedComponents = new List<ICollidable>();

            foreach(ICollidable target in m_Collidables)
            {
                if(i_Collidable != target && target.Visible)////////
                {
                    if(target.CheckCollision(i_Collidable))
                    {
                        collidedComponents.Add(target);
                    }
                }
            }

            foreach(ICollidable target in collidedComponents)
            {
                target.Collided(i_Collidable);
                i_Collidable.Collided(target);
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;
            //collidable.PositionChanged -= collidable_PositionChanged;
            collidable.Disposed -= collidable_Disposed;
            m_Collidables.Remove(collidable);
        }
        
    }
    internal interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}
