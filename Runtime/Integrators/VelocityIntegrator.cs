using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class VelocityIntegrator : IIntegrator
    {
        private readonly IReadOnlyContainer<IRigidbody> _rigidbodies;

        public VelocityIntegrator(IReadOnlyContainer<IRigidbody> rigidbodies)
        {
            _rigidbodies = rigidbodies;
        }

        public void Integrate(SoftFloat deltaTime)
        {
            foreach (var rigidbody in _rigidbodies.Items)
            {
                rigidbody.LinearVelocity += rigidbody.Force / rigidbody.Mass * deltaTime;
                rigidbody.Force = SoftVector3.Zero;
            }
        }
    }
}
