using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class CollisionManager : ICollisionsManager
    {
        protected readonly List<ICollidable> r_Collidables = new List<ICollidable>();
        protected readonly List<ICollidable> r_CollidablesThatWeCheck = new List<ICollidable>();
        protected readonly List<ICollidable> r_CollidablesToDispose = new List<ICollidable>();
      
        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if (!this.r_Collidables.Contains(i_Collidable))
            {
                this.r_Collidables.Add(i_Collidable);
                i_Collidable.Disposed += collidable_Disposed;
                if (i_Collidable.DoWeCheckTheObjectForCollision)
                {
                    r_CollidablesThatWeCheck.Add(i_Collidable);
                }
            }
        }

        public void CheckAllCollidablesForCollision()
        {
            foreach(var collidable in r_CollidablesThatWeCheck)
            {
                FindCollisions(collidable);
            }

            RemoveDisposedCollisions();
        }

        private void FindCollisions(ICollidable i_Collidable)
        {
            List<ICollidable> collidedComponents = new List<ICollidable>();

            foreach (ICollidable target in r_Collidables)
            {
                if (target.IsCollisionEnabled && i_Collidable != target )
                {
                    if (i_Collidable.CheckCollision(target))
                    {
                        collidedComponents.Add(target);
                    }
                }
            }

            foreach (ICollidable target in collidedComponents)
            {
                i_Collidable.Collided(target);
            }
        }

        private void RemoveDisposedCollisions()
        {
            foreach(var collidable in r_CollidablesToDispose)
            {
                if(collidable.DoWeCheckTheObjectForCollision)
                {
                    r_CollidablesThatWeCheck.Remove(collidable);
                }
                r_Collidables.Remove(collidable);
            }
            r_CollidablesToDispose.Clear();
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            r_CollidablesToDispose.Add(collidable);
            collidable.Disposed -= collidable_Disposed;
        }
    }
    internal interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}

