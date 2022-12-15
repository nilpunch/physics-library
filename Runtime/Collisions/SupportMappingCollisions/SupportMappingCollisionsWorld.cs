using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SupportMappingCollisionsWorld : ICollidersWorld<ISMCollider>
    {
        private readonly int _maxIterations;
        private readonly List<ISMCollider> _colliders;

        public SupportMappingCollisionsWorld(int maxIterations)
        {
            _maxIterations = maxIterations;
            _colliders = new List<ISMCollider>();
        }

        public void Add(ISMCollider collider)
        {
            _colliders.Add(collider);
        }

        public void Remove(ISMCollider collider)
        {
            _colliders.Remove(collider);
        }

        public CollisionManifold[] FindCollisions()
        {
            List<CollisionManifold> collisionManifolds = new List<CollisionManifold>();

            foreach (var collidersPair in _colliders.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(collidersPair.a, collidersPair.b, _maxIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = default; //EpaAlgorithm.Calculate(result.Simplex, collidersPair.a, collidersPair.b, _maxIterations);
                    collisionManifolds.Add(new CollisionManifold(collidersPair.a.Id, collidersPair.b.Id, collision));
                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
