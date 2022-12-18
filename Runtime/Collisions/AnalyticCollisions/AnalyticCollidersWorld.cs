using System.Collections.Generic;

namespace GameLibrary.Physics.AnalyticColliders
{
    public class AnalyticCollidersWorld<TConcrete> : IConcreteCollidersWorld<IAnalyticCollider, TConcrete>, IManifoldFinder<TConcrete>
    {
        private readonly List<ConcreteCollider<IAnalyticCollider, TConcrete>> _collidingBodies;

        public AnalyticCollidersWorld()
        {
            _collidingBodies = new List<ConcreteCollider<IAnalyticCollider, TConcrete>>();
        }

        public void Add(ConcreteCollider<IAnalyticCollider, TConcrete> collider)
        {
            _collidingBodies.Add(collider);
        }

        public void Remove(ConcreteCollider<IAnalyticCollider, TConcrete> collider)
        {
            _collidingBodies.Remove(collider);
        }

        public ICollisionManifold<TConcrete>[] FindCollisions()
        {
            List<ICollisionManifold<TConcrete>> collisionManifolds = new List<ICollisionManifold<TConcrete>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision collision = first.Collider.Collide(second.Collider);

                if (collision.Occure)
                    collisionManifolds.Add(new CollisionManifold<TConcrete>(first.Concrete, second.Concrete, collision));
            }

            return collisionManifolds.ToArray();
        }
    }
}
