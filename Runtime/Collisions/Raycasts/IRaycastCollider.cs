using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public interface IRaycastCollider
    {
        Collision BoxCast(Box box);
        Collision SphereCast(Sphere sphere);
        Collision ConvexHullCast(ConvexHull convexHull);
        Collision AABBCast(AABB aabb);

        Collision Raycast(SoftVector3 from, SoftVector3 direction);
    }
}
