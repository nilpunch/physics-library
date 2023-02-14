using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody : IReadOnlyTransform
    {
        SoftVector3 Velocity { get; set; }

        SoftVector3 Position { get; set; }

        SoftFloat Mass { get; set; }

        bool IsStatic { get; set; }
    }
}
