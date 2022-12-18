using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SMCollisionsWorld<TCollidingBody> : IConcreteCollidersWorld<ISMCollider, TCollidingBody>, IManifoldFinder<TCollidingBody>
    {
        private readonly int _maxIterations;
        private readonly List<ConcreteCollider<ISMCollider, TCollidingBody>> _collidingBodies;

        public SMCollisionsWorld(int maxIterations)
        {
            _maxIterations = maxIterations;
            _collidingBodies = new List<ConcreteCollider<ISMCollider, TCollidingBody>>();
        }

        public void Add(ConcreteCollider<ISMCollider, TCollidingBody> collider)
        {
            _collidingBodies.Add(collider);
        }

        public void Remove(ConcreteCollider<ISMCollider, TCollidingBody> collider)
        {
            _collidingBodies.Remove(collider);
        }

        public ICollisionManifold<TCollidingBody>[] FindCollisions()
        {
            List<ICollisionManifold<TCollidingBody>> collisionManifolds = new List<ICollisionManifold<TCollidingBody>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(first.Collider, second.Collider, _maxIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = default; //EpaAlgorithm.Calculate(result.Simplex, collidersPair.a, collidersPair.b, _maxIterations);
                    collisionManifolds.Add(new CollisionManifold<TCollidingBody>(first.Concrete, second.Concrete, collision));
                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
