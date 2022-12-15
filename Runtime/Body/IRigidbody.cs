using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody : ITransform, IUnique
    {
        SoftVector3 Velocity { get; set; }
    }
}
