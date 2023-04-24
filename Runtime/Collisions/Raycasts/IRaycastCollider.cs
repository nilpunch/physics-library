using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public interface IRaycastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Collision<TNumber> BoxCast(Box<TNumber> box);
        Collision<TNumber> SphereCast(Sphere<TNumber> sphere);
        Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull);
        Collision<TNumber> AABBCast(AABB<TNumber> aabb);

        Collision<TNumber> Raycast(Vector3<TNumber> from, Vector3<TNumber> direction);
    }
}
