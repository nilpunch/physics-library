using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody : IReadOnlyTransform
    {
        SoftVector3 Force { get; set; }

        SoftVector3 LinearVelocity { get; set; }

        SoftVector3 AngularVelocity { get; set; }

        SoftVector3 Position { get; set; }

        SoftUnitQuaternion Rotation { get; set; }

        SoftFloat Mass { get; set; }

        bool IsStatic { get; set; }
    }
}
