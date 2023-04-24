using PluggableMath;

namespace GameLibrary.Physics
{
    public class ContactVelocityConstraints<TNumber> : IConstraint<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly ICollisions<TNumber, IRigidbody<TNumber>> _collisions;
        private readonly IContainer<CollisionManifold<TNumber, IRigidbody<TNumber>>> _collisionsBuffer;

        public ContactVelocityConstraints(ICollisions<TNumber, IRigidbody<TNumber>> collisions)
        {
            _collisions = collisions;
            _collisionsBuffer = new Container<CollisionManifold<TNumber, IRigidbody<TNumber>>>();
        }

        public void Solve(Operand<TNumber> deltaTime)
        {
            _collisionsBuffer.Clear();
            _collisions.FindCollisionsNonAlloc(_collisionsBuffer);

            foreach (CollisionManifold<TNumber, IRigidbody<TNumber>> collisionManifold in _collisionsBuffer.Items)
            {
                if (collisionManifold.First.IsStatic && collisionManifold.Second.IsStatic)
                    continue;

                var collisionResolution = new CollisionResolutionJacobian<TNumber>(collisionManifold);

                collisionResolution.Resolve(deltaTime);

                var tangentFriction = new CollisionFrictionJacobian<TNumber>(collisionManifold,
                    CollisionFrictionJacobian<TNumber>.FrictionDirection.Tangent,
                    collisionResolution.Lambda);

                tangentFriction.Resolve(deltaTime);

                var bitangentFriction = new CollisionFrictionJacobian<TNumber>(collisionManifold,
                    CollisionFrictionJacobian<TNumber>.FrictionDirection.Bitangent,
                    collisionResolution.Lambda);

                bitangentFriction.Resolve(deltaTime);
            }
        }
    }
}
