using System.Linq;

namespace GameLibrary.Physics
{
    public class MergedRigidbodyCollisionsFinder : IRigidbodyCollisionsFinder
    {
        private readonly IRigidbodyCollisionsFinder[] _manifoldFinders;

        public MergedRigidbodyCollisionsFinder(IRigidbodyCollisionsFinder[] manifoldFinders)
        {
            _manifoldFinders = manifoldFinders;
        }

        public CollisionManifold<IRigidbody>[] FindCollisions()
        {
            return _manifoldFinders.SelectMany(finder => finder.FindCollisions()).ToArray();
        }
    }
}
