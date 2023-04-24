using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class GravityForceConstraint<TNumber> : IConstraint<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IReadOnlyContainer<IRigidbody<TNumber>> _rigidbodies;
        private readonly Vector3<TNumber> _force;

        public GravityForceConstraint(IReadOnlyContainer<IRigidbody<TNumber>> rigidbodies, Vector3<TNumber> force)
        {
            _rigidbodies = rigidbodies;
            _force = force;
        }

        public void Solve(Operand<TNumber> deltaTime)
        {
            foreach (var rigidbody in _rigidbodies.Items)
            {
                if (!rigidbody.IsStatic)
                {
                    rigidbody.LinearVelocity += _force * deltaTime;
                }
            }
        }
    }
}
