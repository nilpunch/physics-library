using System.Linq;

namespace GameLibrary.Physics
{
    public class MergedManifoldFinders<TBody> : IManifoldFinder<TBody>
    {
        private readonly IManifoldFinder<TBody>[] _manifoldFinders;

        public MergedManifoldFinders(IManifoldFinder<TBody>[] manifoldFinders)
        {
            _manifoldFinders = manifoldFinders;
        }

        public ICollisionManifold<TBody>[] FindCollisions()
        {
            return _manifoldFinders.SelectMany(finder => finder.FindCollisions()).ToArray();
        }
    }
}
