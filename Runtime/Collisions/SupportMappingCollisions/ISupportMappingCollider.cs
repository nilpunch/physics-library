using GameLibrary.Mathematics;

namespace GameLibrary.Physics.GJK
{
    /// <summary>
    /// Collider for GJK collision detection, described by support mapping function.
    /// </summary>
    public interface ISupportMappingCollider
    {
        SoftVector3 Centre { get; }

        SoftVector3 SupportPoint(SoftVector3 direction);
    }
}
