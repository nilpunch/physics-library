using System.Collections.Generic;

namespace GameLibrary.Physics.Raycast
{
    /// <summary>
    /// Code reusal.
    /// </summary>
    public abstract class AnalyticCollidersWorld<TConcrete, TCollider> : IConcreteCollidersWorld<TCollider, TConcrete>, ICollisions<TConcrete>
    {
        private readonly Dictionary<TCollider, TConcrete> _collidingBodies;

        public AnalyticCollidersWorld()
        {
            _collidingBodies = new Dictionary<TCollider, TConcrete>();
        }

        public void Add(TCollider collider, TConcrete concrete)
        {
            _collidingBodies.Add(collider, concrete);
        }

        public void Remove(TCollider collider)
        {
            _collidingBodies.Remove(collider);
        }

        public CollisionManifold<TConcrete>[] FindCollisions()
        {
            List<CollisionManifold<TConcrete>> collisionManifolds = new List<CollisionManifold<TConcrete>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision collision = CalculateCollision(first.Key, second.Key);

                // if (collision.Occure)
                    collisionManifolds.Add(new CollisionManifold<TConcrete>(first.Value, second.Value, collision));
            }

            return collisionManifolds.ToArray();
        }

        protected abstract Collision CalculateCollision(TCollider first, TCollider second);

        public void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TConcrete>> output)
        {
            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision collision = CalculateCollision(first.Key, second.Key);

                // if (collision.Occure)
                output.Add(new CollisionManifold<TConcrete>(first.Value, second.Value, collision));
            }
        }
    }
}
