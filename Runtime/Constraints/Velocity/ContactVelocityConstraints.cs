using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class ContactVelocityConstraints : IConstraint
    {
        private readonly ICollisions<IRigidbody> _collisions;
        private readonly IContainer<CollisionManifold<IRigidbody>> _collisionsBuffer;

        public ContactVelocityConstraints(ICollisions<IRigidbody> collisions)
        {
            _collisions = collisions;
            _collisionsBuffer = new Container<CollisionManifold<IRigidbody>>();
        }

        public void Solve(SoftFloat deltaTime)
        {
            _collisionsBuffer.Clear();
            _collisions.FindCollisionsNonAlloc(_collisionsBuffer);

            foreach (CollisionManifold<IRigidbody> collisionManifold in _collisionsBuffer.Items)
            {
                var collisionResolution = new CollisionResolutionJacobian(collisionManifold);

                collisionResolution.Resolve(deltaTime);

                var tangentFriction = new CollisionFrictionJacobian(collisionManifold,
                    CollisionFrictionJacobian.FrictionDirection.Tangent,
                    collisionResolution.Lambda);

                tangentFriction.Resolve(deltaTime);

                var bitangentFriction = new CollisionFrictionJacobian(collisionManifold,
                    CollisionFrictionJacobian.FrictionDirection.Bitangent,
                    collisionResolution.Lambda);

                bitangentFriction.Resolve(deltaTime);
            }
        }
    }
}
