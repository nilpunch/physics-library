using System.Collections.Generic;

namespace GameLibrary.Physics.Raycast
{
    public class AnalyticCollidersWorld<TConcrete> : IConcreteCollidersWorld<IDoubleCastCollider, TConcrete>, ICollisions<TConcrete>
    {
        private readonly List<ConcreteCollider<IDoubleCastCollider, TConcrete>> _collidingBodies;

        public AnalyticCollidersWorld()
        {
            _collidingBodies = new List<ConcreteCollider<IDoubleCastCollider, TConcrete>>();
        }

        public void Add(ConcreteCollider<IDoubleCastCollider, TConcrete> collider)
        {
            _collidingBodies.Add(collider);
        }

        public void Remove(ConcreteCollider<IDoubleCastCollider, TConcrete> collider)
        {
            _collidingBodies.Remove(collider);
        }

        public ICollisionManifold<TConcrete>[] FindCollisions()
        {
            List<ICollisionManifold<TConcrete>> collisionManifolds = new List<ICollisionManifold<TConcrete>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                Collision collision = first.Collider.ColliderCast(second.Collider);

                if (collision.Occure)
                    collisionManifolds.Add(new CollisionManifold<TConcrete>(first.Concrete, second.Concrete, collision));
            }

            return collisionManifolds.ToArray();
        }
    }
}
