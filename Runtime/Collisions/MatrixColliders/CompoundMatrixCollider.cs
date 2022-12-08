namespace GameLibrary.Physics
{
    public class CompoundMatrixCollider : IMatrixCollider
    {
        private readonly IMatrixCollider[] _collidingShells;

        public CompoundMatrixCollider(IMatrixCollider[] collidingShells)
        {
            _collidingShells = collidingShells;
        }

        public Collision Collide(IMatrixCollider matrixCollider)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.Collide(matrixCollider));

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