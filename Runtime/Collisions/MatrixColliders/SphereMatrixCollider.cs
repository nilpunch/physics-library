namespace GameLibrary.Physics
{
    public class SphereMatrixCollider : IMatrixCollider
    {
        private readonly Sphere _sphere;
        private readonly IMatrixCollisionsLibrary _matrixCollisionsLibrary;

        public SphereMatrixCollider(Sphere sphere, IMatrixCollisionsLibrary matrixCollisionsLibrary)
        {
            _sphere = sphere;
            _matrixCollisionsLibrary = matrixCollisionsLibrary;
        }

        public Collision Collide(IMatrixCollider matrixCollider)
        {
            return matrixCollider.CollideAgainstSphere(_sphere);
        }

        public Collision CollideAgainstBox(Box box)
        {
            return _matrixCollisionsLibrary.SphereAgainstBox(_sphere, box);
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            return _matrixCollisionsLibrary.SphereAgainstSphere(_sphere, sphere);
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            return _matrixCollisionsLibrary.ConvexAgainstSphere(convexHull, _sphere);
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            return _matrixCollisionsLibrary.AABBAgainstSphere(aabb, _sphere);
        }
    }
}