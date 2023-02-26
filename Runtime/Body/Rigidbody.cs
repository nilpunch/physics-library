using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class Rigidbody : IRigidbody
    {
        public SoftVector3 Velocity { get; set; }
        public SoftUnitQuaternion Rotation { get; set; }
        public SoftVector3 Position { get; set; }
        public SoftFloat Mass { get; set; } = SoftFloat.One;
        public bool IsStatic { get; set; }
    }
}
