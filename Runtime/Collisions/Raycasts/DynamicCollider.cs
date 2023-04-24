using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class DynamicCollider<TNumber> : IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IDoubleCastCollider<TNumber> _collider;
        private readonly IReadOnlyTransform<TNumber> _transform;

        public DynamicCollider(IReadOnlyTransform<TNumber> transform, IDoubleCastCollider<TNumber> collider)
        {
            _collider = collider;
            _transform = transform;
        }

        public Collision<TNumber> BoxCast(Box<TNumber> box)
        {
            return _collider.BoxCast(box);
        }

        public Collision<TNumber> SphereCast(Sphere<TNumber> sphere)
        {
            return _collider.SphereCast(sphere);
        }

        public Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull)
        {
            return _collider.ConvexHullCast(convexHull);
        }

        public Collision<TNumber> AABBCast(AABB<TNumber> aabb)
        {
            return _collider.AABBCast(aabb);
        }

        public Collision<TNumber> Raycast(Vector3<TNumber> @from, Vector3<TNumber> direction)
        {
            return _collider.Raycast(from, direction);
        }

        public Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> collider)
        {
            return _collider.ColliderCast(collider);
        }
    }
}
