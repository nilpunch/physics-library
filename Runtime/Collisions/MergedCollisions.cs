using System.Linq;

namespace GameLibrary.Physics
{
    public class MergedCollisions<TBody> : ICollisions<TBody>
    {
        private readonly ICollisions<TBody>[] _manifoldFinders;

        public MergedCollisions(ICollisions<TBody>[] manifoldFinders)
        {
            _manifoldFinders = manifoldFinders;
        }

        public void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TBody>> output)
        {
            foreach (var manifoldFinder in _manifoldFinders)
            {
                manifoldFinder.FindCollisionsNonAlloc(output);
            }
        }
    }
}
