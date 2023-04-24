using PluggableMath;

namespace GameLibrary.Physics
{
    public class VelocityIntegrator<TNumber> : IIntegrator<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IReadOnlyContainer<IRigidbody<TNumber>> _rigidbodies;

        public VelocityIntegrator(IReadOnlyContainer<IRigidbody<TNumber>> rigidbodies)
        {
            _rigidbodies = rigidbodies;
        }

        public void Integrate(Operand<TNumber> deltaTime)
        {
            foreach (var rigidbody in _rigidbodies.Items)
            {
                rigidbody.LinearVelocity += rigidbody.Force / rigidbody.Mass * deltaTime;
                rigidbody.Force = Vector3<TNumber>.Zero;
            }
        }
    }
}
