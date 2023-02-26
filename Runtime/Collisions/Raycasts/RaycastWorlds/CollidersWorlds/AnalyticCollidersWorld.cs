using System.Collections.Generic;

namespace GameLibrary.Physics.Raycast
{
    /// <summary>
    /// Code reusal.
    /// </summary>
    public abstract class AnalyticCollidersWorld<TCollider> : IRigidbodyCollidersWorld<TCollider>, IRigidbodyCollisionsFinder
    {
        private readonly Dictionary<TCollider, IRigidbody> _collidingBodies;

        public AnalyticCollidersWorld()
        {
            _collidingBodies = new Dictionary<TCollider, IRigidbody>();
        }

        public void Add(TCollider collider, IRigidbody rigidbody)
        {
            _collidingBodies.Add(collider, rigidbody);
        }

        public void Remove(TCollider collider)
        {
            _collidingBodies.Remove(collider);
        }

        public CollisionManifold<IRigidbody>[] FindCollisions()
        {
            List<CollisionManifold<IRigidbody>> collisionManifolds = new List<CollisionManifold<IRigidbody>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision collision = CalculateCollision(first.Key, second.Key);

                if (collision.Occure)
                    collisionManifolds.Add(new CollisionManifold<IRigidbody>(first.Value, second.Value, collision));
            }

            return collisionManifolds.ToArray();
        }

        protected abstract Collision CalculateCollision(TCollider first, TCollider second);
    }
}
