using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody : IReadOnlyTransform
    {
        SoftVector3 Force { get; set; }

        SoftVector3 LinearVelocity { get; set; }

        SoftVector3 AngularVelocity { get; set; }

        SoftFloat Mass { get; set; }

        SoftFloat InverseMass
        {
            get
            {
                if (IsStatic)
                    return SoftFloat.Zero;

                return SoftFloat.One / Mass;
            }
        }

        SoftVector3 CenterOfMass { get; set; }

        SoftVector3 CenterOfMassWorldSpace => Rotation * CenterOfMass + Position;

        SoftFloat ContactBeta => (SoftFloat)0.7f;

        SoftFloat Restitution => (SoftFloat)0.1f;

        SoftFloat Friction => (SoftFloat)0.9f;

        Matrix3x3 InertiaTensor
        {
            get
            {
                return Matrix3x3.Identity;
            }
        }

        Matrix3x3 InverseInertiaWorldSpace
        {
            get
            {
                if (IsStatic)
                    return new Matrix3x3();

                var world2Local =
                    Matrix3x3.FromRows
                    (
                        SoftUnitQuaternion.Inverse(Rotation) * new SoftVector3(SoftFloat.One, SoftFloat.Zero, SoftFloat.Zero),
                        SoftUnitQuaternion.Inverse(Rotation) * new SoftVector3(SoftFloat.Zero, SoftFloat.One, SoftFloat.Zero),
                        SoftUnitQuaternion.Inverse(Rotation) * new SoftVector3(SoftFloat.Zero, SoftFloat.Zero, SoftFloat.One)
                    );
                return world2Local.Transposed * InertiaTensor.Inverted * world2Local;

                var invertedInertia = InertiaTensor.Inverted;

                return Matrix3x3.FromRows(
                    Rotation * invertedInertia.Row0,
                    Rotation * invertedInertia.Row1,
                    Rotation * invertedInertia.Row2);
            }
        }

        bool IsStatic { get; set; }
    }
}
