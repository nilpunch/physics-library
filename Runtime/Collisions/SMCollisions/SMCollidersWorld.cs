using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class SMCollidersWorld<TNumber, TConcrete> : IConcreteCollidersWorld<ISMCollider<TNumber>, TConcrete>, ICollisions<TNumber, TConcrete> where TNumber : struct, INumber<TNumber>
    {
        private readonly int _maxGjkIterations;
        private readonly int _maxEpaIterations;
        private readonly List<(ISMCollider<TNumber> collider, TConcrete concrete)> _collidingBodies;

        public SMCollidersWorld(int maxGjkIterations, int maxEpaIterations)
        {
            _maxGjkIterations = maxGjkIterations;
            _maxEpaIterations = maxEpaIterations;
            _collidingBodies = new List<(ISMCollider<TNumber>, TConcrete)>();
        }

        public void Add(ISMCollider<TNumber> collider, TConcrete concrete)
        {
            _collidingBodies.Add((collider, concrete));
        }

        public void Remove(ISMCollider<TNumber> collider)
        {
            _collidingBodies.RemoveAll(item => item.Item1 == collider);
        }


        public void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TNumber, TConcrete>> output)
        {
            foreach (var (first, second) in _collidingBodies.DistinctPairs((a, b) => (a, b)))
            {
                GjkAlgorithm<TNumber>.Result result = GjkAlgorithm<TNumber>.Calculate(first.collider, second.collider, _maxGjkIterations);

                if (result.CollisionHappened)
                {
                    Collision<TNumber> collision = EpaAlgorithm<TNumber>.Calculate(result.Simplex, first.collider, second.collider, _maxEpaIterations);
                    output.Add(new CollisionManifold<TNumber, TConcrete>(first.concrete, second.concrete, collision));
                }
            }
        }
    }
}
