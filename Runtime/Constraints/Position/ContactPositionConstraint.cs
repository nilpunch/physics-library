using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class ContactPositionConstraint<TNumber> : IConstraint<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly ICollisions<TNumber, IRigidbody<TNumber>> _collisions;
        private readonly IContainer<CollisionManifold<TNumber, IRigidbody<TNumber>>> _collisionsBuffer;

        public ContactPositionConstraint(ICollisions<TNumber, IRigidbody<TNumber>> collisions)
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
                var firstBody = collisionManifold.First;
                var secondBody = collisionManifold.Second;

                Operand<TNumber> firstStatic = firstBody.IsStatic ? Operand<TNumber>.One : Operand<TNumber>.Zero;
                Operand<TNumber> secondStatic = secondBody.IsStatic ? Operand<TNumber>.One : Operand<TNumber>.Zero;

                Vector3<TNumber> resolution = collisionManifold.Collision.PenetrationNormal
                    * collisionManifold.Collision.PenetrationDepth
                    / Math<TNumber>.Max(Operand<TNumber>.One, firstStatic + secondStatic);

                firstBody.Position -= resolution * (Operand<TNumber>.One - firstStatic);
                secondBody.Position += resolution * (Operand<TNumber>.One - secondStatic);
            }
        }
    }
}
