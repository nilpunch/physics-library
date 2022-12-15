using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody : ITransform
    {
        SoftVector3 Velocity { get; set; }
    }
}
