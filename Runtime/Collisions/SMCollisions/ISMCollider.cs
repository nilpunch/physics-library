using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    /// <summary>
    /// Collider for GJK collision detection, described by support mapping function.
    /// </summary>
    public interface ISMCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Vector3<TNumber> Centre { get; }

        /// <summary>
        /// Returns furthest point of object in some direction.
        /// </summary>
        Vector3<TNumber> SupportPoint(Vector3<TNumber> direction);
    }
}
