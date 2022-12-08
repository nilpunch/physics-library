namespace GameLibrary.Physics
{
    public interface IMatrixCollider
    {
        /// <summary>
        /// Double dispatch method.
        /// </summary>
        Collision Collide(IMatrixCollider matrixCollider);

        Collision CollideAgainstBox(Box box);
        Collision CollideAgainstSphere(Sphere sphere);
        Collision CollideAgainstConvexHull(ConvexHull convexHull);

        Collision CollideAgainstAABB(AABB aabb);
    }
}
