using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class ConvexHullCollider : IDoubleCastCollider
    {
        private readonly ConvexHull _convexHull;

        public ConvexHullCollider(ConvexHull convexHull)
        {
            _convexHull = convexHull;
        }

        public Collision ColliderCast(IRaycastCollider raycastCollider)
        {
            return raycastCollider.ConvexHullCast(_convexHull);
        }

        public Collision BoxCast(Box box)
        {
            return AnalyticCollisionsLibrary.ConvexAgainstBox(_convexHull, box);
        }

        public Collision SphereCast(Sphere sphere)
        {
            return AnalyticCollisionsLibrary.ConvexAgainstSphere(_convexHull, sphere);
        }

        public Collision ConvexHullCast(ConvexHull convexHull)
        {
            return AnalyticCollisionsLibrary.ConvexAgainstConvex(convexHull, _convexHull);
        }

        public Collision AABBCast(AABB aabb)
        {
            return AnalyticCollisionsLibrary.AABBAgainstConvexHull(aabb, _convexHull);
        }

        public Collision Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
