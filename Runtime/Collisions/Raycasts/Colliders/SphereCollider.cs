using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class SphereCollider : IDoubleCastCollider
    {
        private readonly Sphere _sphere;

        public SphereCollider(Sphere sphere)
        {
            _sphere = sphere;
        }

        public Collision ColliderCast(IRaycastCollider raycastCollider)
        {
            return raycastCollider.SphereCast(_sphere);
        }

        public Collision BoxCast(Box box)
        {
            return AnalyticCollisionsLibrary.SphereAgainstBox(_sphere, box);
        }

        public Collision SphereCast(Sphere sphere)
        {
            return AnalyticCollisionsLibrary.SphereAgainstSphere(_sphere, sphere);
        }

        public Collision ConvexHullCast(ConvexHull convexHull)
        {
            return AnalyticCollisionsLibrary.ConvexAgainstSphere(convexHull, _sphere);
        }

        public Collision AABBCast(AABB aabb)
        {
            return AnalyticCollisionsLibrary.AABBAgainstSphere(aabb, _sphere);
        }

        public Collision Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
