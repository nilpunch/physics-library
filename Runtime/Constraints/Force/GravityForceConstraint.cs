using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class GravityForceConstraint : IConstraint
    {
        private readonly IReadOnlyContainer<IRigidbody> _rigidbodies;
        private readonly SoftVector3 _force;

        public GravityForceConstraint(IReadOnlyContainer<IRigidbody> rigidbodies, SoftVector3 force)
        {
            _rigidbodies = rigidbodies;
            _force = force;
        }

        public void Solve(SoftFloat deltaTime)
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
