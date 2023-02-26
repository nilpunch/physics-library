using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SMCollidersWorld : IRigidbodyCollidersWorld<ISMCollider>, IRigidbodyCollisionsFinder
    {
        private readonly int _maxGjkIterations;
        private readonly int _maxEpaIterations;
        private readonly Dictionary<ISMCollider, IRigidbody> _collidingBodies;

        public SMCollidersWorld(int maxGjkIterations, int maxEpaIterations)
        {
            _maxGjkIterations = maxGjkIterations;
            _maxEpaIterations = maxEpaIterations;
            _collidingBodies = new Dictionary<ISMCollider, IRigidbody>();
        }

        public void Add(ISMCollider collider, IRigidbody rigidbody)
        {
            _collidingBodies.Add(collider, rigidbody);
        }

        public void Remove(ISMCollider collider)
        {
            _collidingBodies.Remove(collider);
        }

        public CollisionManifold<IRigidbody>[] FindCollisions()
        {
            List<CollisionManifold<IRigidbody>> collisionManifolds = new List<CollisionManifold<IRigidbody>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(first.Key, second.Key, _maxGjkIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = EpaAlgorithm.Calculate(result.Simplex, first.Key, second.Key, _maxEpaIterations);
                    collisionManifolds.Add(new CollisionManifold<IRigidbody>(first.Value, second.Value, collision));
                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
