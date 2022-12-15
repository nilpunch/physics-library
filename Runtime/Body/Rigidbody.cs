using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class Rigidbody : IRigidbody
    {
        public Rigidbody()
        {
            Id = Guid.NewGuid();
        }

        public SoftVector3 Velocity { get; set; }
        public SoftUnitQuaternion Rotation { get; set; }
        public SoftVector3 Position { get; set; }
        public Guid Id { get; }
    }
}
