using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public interface IRaycastShooter<TNumber, TRaycastTarget> where TNumber : struct, INumber<TNumber>
    {
        // ConcreteCastHit<TRaycastTarget> BoxCast(Box<TNumber> box);
        // ConcreteCastHit<TRaycastTarget> SphereCast(Sphere<TNumber> sphere);
        // ConcreteCastHit<TRaycastTarget> ConvexHullCast(ConvexHull<TNumber> convexHull);
        // ConcreteCastHit<TRaycastTarget> AABBCast(AABB<TNumber> aabb);

        ConcreteRaycastHit<TNumber, TRaycastTarget> Raycast(Vector3<TNumber> from, Vector3<TNumber> direction);
    }
}
