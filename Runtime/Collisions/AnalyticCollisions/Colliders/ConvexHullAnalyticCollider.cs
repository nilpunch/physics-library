namespace GameLibrary.Physics.MatrixColliders
{
    public class ConvexHullAnalyticCollider : IAnalyticCollider
    {
        private readonly ConvexHull _convexHull;
        private readonly IAnalyticCollisionsLibrary _analyticCollisionsLibrary;

        public ConvexHullAnalyticCollider(ConvexHull convexHull, IAnalyticCollisionsLibrary analyticCollisionsLibrary)
        {
            _convexHull = convexHull;
            _analyticCollisionsLibrary = analyticCollisionsLibrary;
        }

        public Collision Collide(IAnalyticCollider analyticCollider)
        {
            return analyticCollider.CollideAgainstConvexHull(_convexHull);
        }

        public Collision CollideAgainstBox(Box box)
        {
            return _analyticCollisionsLibrary.ConvexAgainstBox(_convexHull, box);
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            return _analyticCollisionsLibrary.ConvexAgainstSphere(_convexHull, sphere);
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            return _analyticCollisionsLibrary.ConvexAgainstConvex(convexHull, _convexHull);
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            return _analyticCollisionsLibrary.AABBAgainstConvexHull(aabb, _convexHull);
        }
    }
}
