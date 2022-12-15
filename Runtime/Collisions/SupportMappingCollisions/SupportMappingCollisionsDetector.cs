using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SupportMappingCollisionsDetector : ICollisionDetector<ISupportMappingCollider>
    {
        private readonly int _maxIterations;

        public SupportMappingCollisionsDetector(int maxIterations)
        {
            _maxIterations = maxIterations;
        }

        public CollisionManifold<ISupportMappingCollider>[] FindManifolds(ISupportMappingCollider[] colliders)
        {
            List<CollisionManifold<ISupportMappingCollider>> collisionManifolds = new List<CollisionManifold<ISupportMappingCollider>>();

            foreach (var collidersPair in colliders.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(collidersPair.a, collidersPair.b, _maxIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = default; //EpaAlgorithm.Calculate(result.Simplex, collidersPair.a, collidersPair.b, _maxIterations);
                    collisionManifolds.Add(new CollisionManifold<ISupportMappingCollider>(collidersPair.a, collidersPair.b, collision));
                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
