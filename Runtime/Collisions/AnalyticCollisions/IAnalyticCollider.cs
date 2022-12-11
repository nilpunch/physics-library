namespace GameLibrary.Physics.MatrixColliders
{
    /// <summary>
    /// Monolith collider interface with one-to-one collision description.
    /// </summary>
    public interface IAnalyticCollider
    {
        /// <summary>
        /// Double dispatch method.
        /// </summary>
        Collision Collide(IAnalyticCollider analyticCollider);

        Collision CollideAgainstBox(Box box);
        Collision CollideAgainstSphere(Sphere sphere);
        Collision CollideAgainstConvexHull(ConvexHull convexHull);

        Collision CollideAgainstAABB(AABB aabb);
    }
}
