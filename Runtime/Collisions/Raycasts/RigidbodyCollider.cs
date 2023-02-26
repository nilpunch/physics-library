using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class RigidbodyCollider : IDoubleCastCollider
    {
        private readonly IDoubleCastCollider _collider;
        private readonly IRigidbody _rigidbody;

        public RigidbodyCollider(IRigidbody rigidbody, IDoubleCastCollider collider)
        {
            _collider = collider;
            _rigidbody = rigidbody;
        }

        public Collision BoxCast(Box box)
        {
            return _collider.BoxCast(box);
        }

        public Collision SphereCast(Sphere sphere)
        {
            return _collider.SphereCast(sphere);
        }

        public Collision ConvexHullCast(ConvexHull convexHull)
        {
            return _collider.ConvexHullCast(convexHull);
        }

        public Collision AABBCast(AABB aabb)
        {
            return _collider.AABBCast(aabb);
        }

        public Collision Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            return _collider.Raycast(from, direction);
        }

        public Collision ColliderCast(IRaycastCollider collider)
        {
            return _collider.ColliderCast(collider);
        }
    }
}
