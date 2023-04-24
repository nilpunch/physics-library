using PluggableMath;

namespace GameLibrary.Physics
{
    public class MergedCollisions<TNumber, TBody> : ICollisions<TNumber, TBody> where TNumber : struct, INumber<TNumber>
    {
        private readonly ICollisions<TNumber, TBody>[] _manifoldFinders;

        public MergedCollisions(ICollisions<TNumber, TBody>[] manifoldFinders)
        {
            _manifoldFinders = manifoldFinders;
        }

        public void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TNumber, TBody>> output)
        {
            foreach (var manifoldFinder in _manifoldFinders)
            {
                manifoldFinder.FindCollisionsNonAlloc(output);
            }
        }
    }
}
