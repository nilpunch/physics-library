using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics
{
    public class PositionIntegrator<TNumber> : IIntegrator<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IReadOnlyContainer<IRigidbody<TNumber>> _rigidbodies;

        public PositionIntegrator(IReadOnlyContainer<IRigidbody<TNumber>> rigidbodies)
        {
            _rigidbodies = rigidbodies;
        }

        public void Integrate(Operand<TNumber> deltaTime)
        {
            foreach (var rigidbody in _rigidbodies.Items)
            {
                rigidbody.Position += rigidbody.LinearVelocity * deltaTime;

                Vector3<TNumber> angularVelocity = rigidbody.AngularVelocity;

                Operand<TNumber> halfDeltaTime = deltaTime * (Operand<TNumber>)0.5f;
                rigidbody.Rotation = UnitQuaternion<TNumber>.EulerRadians(angularVelocity * deltaTime) * rigidbody.Rotation;
            }
        }
    }
}
