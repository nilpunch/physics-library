namespace GameLibrary.Physics.MatrixColliders
{
    public class BoxAnalyticCollider : IAnalyticCollider
    {
        private readonly Box _box;
        private readonly IAnalyticCollisionsLibrary _analyticCollisionsLibrary;

        public BoxAnalyticCollider(Box box, IAnalyticCollisionsLibrary analyticCollisionsLibrary)
        {
            _box = box;
            _analyticCollisionsLibrary = analyticCollisionsLibrary;
        }

        public Collision Collide(IAnalyticCollider analyticCollider)
        {
            return analyticCollider.CollideAgainstBox(_box);
        }

        public Collision CollideAgainstBox(Box box)
        {
            return _analyticCollisionsLibrary.BoxAgainstBox(_box, box);
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            return _analyticCollisionsLibrary.SphereAgainstBox(sphere, _box);
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            return _analyticCollisionsLibrary.ConvexAgainstBox(convexHull, _box);
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            return _analyticCollisionsLibrary.AABBAgainstBox(aabb, _box);
        }
    }
}
