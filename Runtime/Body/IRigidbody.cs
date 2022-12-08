using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody<out TCollider> : IPhysicalBody<TCollider>
    {
        SoftVector3 Position { get; set; }
        SoftVector3 Velocity { get; set; }
    }
}
