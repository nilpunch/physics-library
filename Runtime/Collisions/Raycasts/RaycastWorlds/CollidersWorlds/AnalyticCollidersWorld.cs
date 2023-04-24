using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    /// <summary>
    /// Code reusal.
    /// </summary>
    public abstract class AnalyticCollidersWorld<TNumber, TConcrete, TCollider> : IConcreteCollidersWorld<TCollider, TConcrete>, ICollisions<TNumber, TConcrete> where TNumber : struct, INumber<TNumber>
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

        public CollisionManifold<TNumber, TConcrete>[] FindCollisions()
        {
            List<CollisionManifold<TNumber, TConcrete>> collisionManifolds = new List<CollisionManifold<TNumber, TConcrete>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision<TNumber> collision = CalculateCollision(first.Key, second.Key);

                // if (collision.Occure)
                    collisionManifolds.Add(new CollisionManifold<TNumber, TConcrete>(first.Value, second.Value, collision));
            }

            return collisionManifolds.ToArray();
        }

        protected abstract Collision<TNumber> CalculateCollision(TCollider first, TCollider second);

        public void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TNumber, TConcrete>> output)
        {
            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision<TNumber> collision = CalculateCollision(first.Key, second.Key);

                // if (collision.Occure)
                output.Add(new CollisionManifold<TNumber, TConcrete>(first.Value, second.Value, collision));
            }
        }
    }
}
