namespace GameLibrary.Physics.AnalyticColliders
{
    public class CompoundAnalyticCollider : IAnalyticCollider
    {
        private readonly IAnalyticCollider[] _collidingShells;

        public CompoundAnalyticCollider(IAnalyticCollider[] collidingShells)
        {
            _collidingShells = collidingShells;
        }

        public Collision Collide(IAnalyticCollider analyticCollider)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.Collide(analyticCollider));

            return collision;
        }

        public Collision CollideAgainstBox(Box box)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.CollideAgainstBox(box));

            return collision;
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.CollideAgainstSphere(sphere));

            return collision;
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.CollideAgainstConvexHull(convexHull));

            return collision;
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.CollideAgainstAABB(aabb));

            return collision;
        }
    }
}
