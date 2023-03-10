using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody : IReadOnlyTransform
    {
        SoftVector3 Velocity { get; set; }
    }
}
