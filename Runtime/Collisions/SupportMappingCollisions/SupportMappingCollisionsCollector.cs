using System.Collections.Generic;

namespace GameLibrary.Physics.GJK
{
    public class SupportMappingCollisionsCollector : ICollisionCollector<ISupportMappingCollider>
    {
        private readonly int _maxIterations;

        public SupportMappingCollisionsCollector(int maxIterations)
        {
            _maxIterations = maxIterations;
        }

        public CollisionManifold<ISupportMappingCollider>[] CollectManifolds(ISupportMappingCollider[] colliders)
        {
            List<CollisionManifold<ISupportMappingCollider>> collisionManifolds = new List<CollisionManifold<ISupportMappingCollider>>();

            foreach (var collidersPair in colliders.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(collidersPair.a, collidersPair.b, _maxIterations);

                if (result.CollisionHappened)
                {

                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
