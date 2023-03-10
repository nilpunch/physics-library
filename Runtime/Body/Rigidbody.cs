using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class Rigidbody : IRigidbody
    {
        public SoftVector3 Force { get; set; }
        public SoftVector3 LinearVelocity { get; set; } = SoftVector3.Zero;
        public SoftVector3 AngularVelocity { get; set; }
        public SoftUnitQuaternion Rotation { get; set; } = SoftUnitQuaternion.Identity;
        public SoftVector3 Position { get; set; } = SoftVector3.Zero;
        public SoftFloat Mass { get; set; } = SoftFloat.One;
        public bool IsStatic { get; set; }
    }
}
