using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public interface IRaycastShooter<TRaycastTarget>
    {
        // ConcreteCastHit<TRaycastTarget> BoxCast(Box box);
        // ConcreteCastHit<TRaycastTarget> SphereCast(Sphere sphere);
        // ConcreteCastHit<TRaycastTarget> ConvexHullCast(ConvexHull convexHull);
        // ConcreteCastHit<TRaycastTarget> AABBCast(AABB aabb);

        ConcreteRaycastHit<TRaycastTarget> Raycast(SoftVector3 from, SoftVector3 direction);
    }
}
