using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class Rigidbody : IRigidbody
    {
        public SoftVector3 Velocity { get; set; }
        public SoftUnitQuaternion Rotation { get; set; }
        public SoftVector3 Position { get; set; }
    }
}
