namespace GameLibrary.Physics
{
    public class ConvexHullMatrixCollider : IMatrixCollider
    {
        private readonly ConvexHull _convexHull;
        private readonly IMatrixCollisionsLibrary _matrixCollisionsLibrary;

        public ConvexHullMatrixCollider(ConvexHull convexHull, IMatrixCollisionsLibrary matrixCollisionsLibrary)
        {
            _convexHull = convexHull;
            _matrixCollisionsLibrary = matrixCollisionsLibrary;
        }

        public Collision Collide(IMatrixCollider matrixCollider)
        {
            return matrixCollider.CollideAgainstConvexHull(_convexHull);
        }

        public Collision CollideAgainstBox(Box box)
        {
            return _matrixCollisionsLibrary.ConvexAgainstBox(_convexHull, box);
        }

        public Collision CollideAgainstSphere(Sphere sphere)
        {
            return _matrixCollisionsLibrary.ConvexAgainstSphere(_convexHull, sphere);
        }

        public Collision CollideAgainstConvexHull(ConvexHull convexHull)
        {
            return _matrixCollisionsLibrary.ConvexAgainstConvex(convexHull, _convexHull);
        }

        public Collision CollideAgainstAABB(AABB aabb)
        {
            return _matrixCollisionsLibrary.AABBAgainstConvexHull(aabb, _convexHull);
        }
    }
}