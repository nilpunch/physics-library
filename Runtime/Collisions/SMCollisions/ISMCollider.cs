using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    /// <summary>
    /// Collider for GJK collision detection, described by support mapping function.
    /// </summary>
    public interface ISMCollider
    {
        SoftVector3 Centre { get; }

        SoftVector3 SupportPoint(SoftVector3 direction);
    }
}
