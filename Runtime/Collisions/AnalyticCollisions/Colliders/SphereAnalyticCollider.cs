namespace GameLibrary.Physics.AnalyticColliders
{
    public class SphereAnalyticCollider : IAnalyticCollider
    {
        private readonly Sphere _sphere;
        private readonly IAnalyticCollisionsLibrary _analyticCollisionsLibrary;

        public SphereAnalyticCollider(Sphere sphere, IAnalyticCollisionsLibrary analyticCollisionsLibrary)
        {
            _sphere = sphere;
            _analyticCollisionsLibrary = analyticCollisionsLibrary;
        }

        public Collision Collide(IAnalyticCollider analyticCollider)
        {
            return analyticCollider.CollideAgainstSphere(_sphere);
        }

        public Collision CollideAgainstBox(Box box)
        {
            return _analyticCollisionsLibrary.SphereAgainstBox(_sphere, box);
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            return _analyticCollisionsLibrary.SphereAgainstSphere(_sphere, sphere);
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            return _analyticCollisionsLibrary.ConvexAgainstSphere(convexHull, _sphere);
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            return _analyticCollisionsLibrary.AABBAgainstSphere(aabb, _sphere);
        }
    }
}
