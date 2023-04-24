using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics
{
    public class Rigidbody<TNumber> : IRigidbody<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public Vector3<TNumber> Force { get; set; } = Vector3<TNumber>.Zero;
        public Vector3<TNumber> LinearVelocity { get; set; } = Vector3<TNumber>.Zero;
        public Vector3<TNumber> AngularVelocity { get; set; } = Vector3<TNumber>.Zero;
        public UnitQuaternion<TNumber> Rotation { get; set; } = UnitQuaternion<TNumber>.Identity;
        public Vector3<TNumber> Position { get; set; } = Vector3<TNumber>.Zero;
        public Operand<TNumber> Mass { get; set; } = Operand<TNumber>.One;
        public Vector3<TNumber> CenterOfMass { get; set; } = Vector3<TNumber>.Zero;
        public bool IsStatic { get; set; }
    }
}
