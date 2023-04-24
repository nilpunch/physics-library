using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class SphereCollider<TNumber> : IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly Sphere<TNumber> _sphere;

        public SphereCollider(Sphere<TNumber> sphere)
        {
            _sphere = sphere;
        }

        public Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> raycastCollider)
        {
            return raycastCollider.SphereCast(_sphere);
        }

        public Collision<TNumber> BoxCast(Box<TNumber> box)
        {
            return AnalyticCollisionsLibrary<TNumber>.SphereAgainstBox(_sphere, box);
        }

        public Collision<TNumber> SphereCast(Sphere<TNumber> sphere)
        {
            return AnalyticCollisionsLibrary<TNumber>.SphereAgainstSphere(_sphere, sphere);
        }

        public Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull)
        {
            return AnalyticCollisionsLibrary<TNumber>.ConvexAgainstSphere(convexHull, _sphere);
        }

        public Collision<TNumber> AABBCast(AABB<TNumber> aabb)
        {
            return AnalyticCollisionsLibrary<TNumber>.AABBAgainstSphere(aabb, _sphere);
        }

        public Collision<TNumber> Raycast(Vector3<TNumber> @from, Vector3<TNumber> direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
