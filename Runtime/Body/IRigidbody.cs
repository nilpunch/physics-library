using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IRigidbody<TNumber> : IReadOnlyTransform<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Vector3<TNumber> Force { get; set; }

        Vector3<TNumber> LinearVelocity { get; set; }

        Vector3<TNumber> AngularVelocity { get; set; }

        Operand<TNumber> Mass { get; set; }

        Operand<TNumber> InverseMass
        {
            get
            {
                if (IsStatic)
                    return Operand<TNumber>.Zero;

                return Operand<TNumber>.One / Mass;
            }
        }

        Vector3<TNumber> CenterOfMass { get; set; }

        Vector3<TNumber> CenterOfMassWorldSpace => Rotation * CenterOfMass + Position;

        Operand<TNumber> ContactBeta => (Operand<TNumber>)0.7f;

        Operand<TNumber> Restitution => (Operand<TNumber>)0.0f;

        Operand<TNumber> Friction => (Operand<TNumber>)1f;

        Matrix3x3<TNumber> InertiaTensor
        {
            get
            {
                return Matrix3x3<TNumber>.Identity;
            }
        }

        Matrix3x3<TNumber> InverseInertiaWorldSpace
        {
            get
            {
                if (IsStatic)
                    return new Matrix3x3<TNumber>();

                var world2Local =
                    Matrix3x3<TNumber>.FromRows
                    (
                        UnitQuaternion<TNumber>.Inverse(Rotation) * new Vector3<TNumber>(Operand<TNumber>.One, Operand<TNumber>.Zero, Operand<TNumber>.Zero),
                        UnitQuaternion<TNumber>.Inverse(Rotation) * new Vector3<TNumber>(Operand<TNumber>.Zero, Operand<TNumber>.One, Operand<TNumber>.Zero),
                        UnitQuaternion<TNumber>.Inverse(Rotation) * new Vector3<TNumber>(Operand<TNumber>.Zero, Operand<TNumber>.Zero, Operand<TNumber>.One)
                    );
                return world2Local.Transposed * InertiaTensor.Inverted * world2Local;

                var invertedInertia = InertiaTensor.Inverted;

                return Matrix3x3<TNumber>.FromRows(
                    Rotation * invertedInertia.Row0,
                    Rotation * invertedInertia.Row1,
                    Rotation * invertedInertia.Row2);
            }
        }

        bool IsStatic { get; set; }
    }
}
