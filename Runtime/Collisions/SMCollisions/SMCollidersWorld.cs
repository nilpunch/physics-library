using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SMCollidersWorld<TConcrete> : IConcreteCollidersWorld<ISMCollider, TConcrete>, ICollisions<TConcrete>
    {
        private readonly int _maxGjkIterations;
        private readonly int _maxEpaIterations;
        private readonly Dictionary<ISMCollider, TConcrete> _collidingBodies;

        public SMCollidersWorld(int maxGjkIterations, int maxEpaIterations)
        {
            _maxGjkIterations = maxGjkIterations;
            _maxEpaIterations = maxEpaIterations;
            _collidingBodies = new Dictionary<ISMCollider, TConcrete>();
        }

        public void Add(ISMCollider collider, TConcrete concrete)
        {
            _collidingBodies.Add(collider, concrete);
        }

        public void Remove(ISMCollider collider)
        {
            _collidingBodies.Remove(collider);
        }

        public ICollisionManifold<TConcrete>[] FindCollisions()
        {
            List<ICollisionManifold<TConcrete>> collisionManifolds = new List<ICollisionManifold<TConcrete>>();

            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(first.Key, second.Key, _maxGjkIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = EpaAlgorithm.Calculate(result.Simplex, first.Key, second.Key, _maxEpaIterations);
                    collisionManifolds.Add(new CollisionManifold<TConcrete>(first.Value, second.Value, collision));
                }
            }

            return collisionManifolds.ToArray();
        }
    }
}
