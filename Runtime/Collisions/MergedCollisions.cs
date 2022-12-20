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

        public ICollisionManifold<TBody>[] FindCollisions()
        {
            return _manifoldFinders.SelectMany(finder => finder.FindCollisions()).ToArray();
        }
    }
}
