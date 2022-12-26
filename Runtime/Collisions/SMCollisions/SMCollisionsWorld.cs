using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SMCollisionsWorld<TCollidingBody> : IConcreteCollidersWorld<ISMCollider, TCollidingBody>, ICollisions<TCollidingBody>
    {
        private readonly int _maxGjkIterations;
        private readonly int _maxEpaIterations;
        private readonly List<ConcreteCollider<ISMCollider, TCollidingBody>> _collidingBodies;

        public SMCollisionsWorld(int maxGjkIterations, int maxEpaIterations)
        {
            _maxGjkIterations = maxGjkIterations;
            _maxEpaIterations = maxEpaIterations;
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
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(first.Collider, second.Collider, _maxGjkIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = EpaAlgorithm.Calculate(result.Simplex, first.Collider, second.Collider, _maxEpaIterations);
                    collisionManifolds.Add(new CollisionManifold<TCollidingBody>(first.Concrete, second.Concrete, collision));
                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
