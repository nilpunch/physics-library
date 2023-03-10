using System.Collections.Generic;

namespace GameLibrary.Physics.SupportMapping
{
    public class SMCollidersWorld<TConcrete> : IConcreteCollidersWorld<ISMCollider, TConcrete>, ICollisions<TConcrete>
    {
        private readonly int _maxGjkIterations;
        private readonly int _maxEpaIterations;
        private readonly List<(ISMCollider collider, TConcrete concrete)> _collidingBodies;

        public SMCollidersWorld(int maxGjkIterations, int maxEpaIterations)
        {
            _maxGjkIterations = maxGjkIterations;
            _maxEpaIterations = maxEpaIterations;
            _collidingBodies = new List<(ISMCollider, TConcrete)>();
        }

        public void Add(ISMCollider collider, TConcrete concrete)
        {
            _collidingBodies.Add((collider, concrete));
        }

        public void Remove(ISMCollider collider)
        {
            _collidingBodies.RemoveAll(item => item.Item1 == collider);
        }


        public void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TConcrete>> output)
        {
            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm.Result result = GjkAlgorithm.Calculate(first.collider, second.collider, _maxGjkIterations);

                if (result.CollisionHappened)
                {
                    Collision collision = EpaAlgorithm.Calculate(result.Simplex, first.collider, second.collider, _maxEpaIterations);
                    output.Add(new CollisionManifold<TConcrete>(first.concrete, second.concrete, collision));
                }
            }
        }
    }
}
