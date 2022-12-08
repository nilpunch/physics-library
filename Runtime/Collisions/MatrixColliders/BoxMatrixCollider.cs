namespace GameLibrary.Physics
{
    public class BoxMatrixCollider : IMatrixCollider
    {
        private readonly Box _box;
        private readonly IMatrixCollisionsLibrary _matrixCollisionsLibrary;

        public BoxMatrixCollider(Box box, IMatrixCollisionsLibrary matrixCollisionsLibrary)
        {
            _box = box;
            _matrixCollisionsLibrary = matrixCollisionsLibrary;
        }

        public Collision Collide(IMatrixCollider matrixCollider)
        {
            return matrixCollider.CollideAgainstBox(_box);
        }

        public Collision CollideAgainstBox(Box box)
        {
            return _matrixCollisionsLibrary.BoxAgainstBox(_box, box);
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            return _matrixCollisionsLibrary.SphereAgainstBox(sphere, _box);
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            return _matrixCollisionsLibrary.ConvexAgainstBox(convexHull, _box);
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            return _matrixCollisionsLibrary.AABBAgainstBox(aabb, _box);
        }
    }
}