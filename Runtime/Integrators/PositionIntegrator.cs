using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class PositionIntegrator : IIntegrator
    {
        private readonly IReadOnlyContainer<IRigidbody> _rigidbodies;

        public PositionIntegrator(IReadOnlyContainer<IRigidbody> rigidbodies)
        {
            _rigidbodies = rigidbodies;
        }

        public void Integrate(SoftFloat deltaTime)
        {
            foreach (var rigidbody in _rigidbodies.Items)
            {
                rigidbody.Position += rigidbody.LinearVelocity * deltaTime;

                SoftVector3 angularVelocity = rigidbody.AngularVelocity;

                SoftFloat halfDeltaTime = deltaTime * (SoftFloat)0.5f;
                rigidbody.Rotation = SoftUnitQuaternion.EulerRadians(angularVelocity * deltaTime) * rigidbody.Rotation;
            }
        }
    }
}
