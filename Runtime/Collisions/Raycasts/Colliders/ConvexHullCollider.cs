using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class ConvexHullCollider<TNumber> : IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly ConvexHull<TNumber> _convexHull;

        public ConvexHullCollider(ConvexHull<TNumber> convexHull)
        {
            _convexHull = convexHull;
        }

        public Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> raycastCollider)
        {
            return raycastCollider.ConvexHullCast(_convexHull);
        }

        public Collision<TNumber> BoxCast(Box<TNumber> box)
        {
            return AnalyticCollisionsLibrary<TNumber>.ConvexAgainstBox(_convexHull, box);
        }

        public Collision<TNumber> SphereCast(Sphere<TNumber> sphere)
        {
            return AnalyticCollisionsLibrary<TNumber>.ConvexAgainstSphere(_convexHull, sphere);
        }

        public Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull)
        {
            return AnalyticCollisionsLibrary<TNumber>.ConvexAgainstConvex(convexHull, _convexHull);
        }

        public Collision<TNumber> AABBCast(AABB<TNumber> aabb)
        {
            return AnalyticCollisionsLibrary<TNumber>.AABBAgainstConvexHull(aabb, _convexHull);
        }

        public Collision<TNumber> Raycast(Vector3<TNumber> @from, Vector3<TNumber> direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
