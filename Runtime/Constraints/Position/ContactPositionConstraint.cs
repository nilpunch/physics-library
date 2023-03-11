using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class ContactPositionConstraint : IConstraint
    {
        private readonly ICollisions<IRigidbody> _collisions;
        private readonly IContainer<CollisionManifold<IRigidbody>> _collisionsBuffer;

        public ContactPositionConstraint(ICollisions<IRigidbody> collisions)
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
                var firstBody = collisionManifold.First;
                var secondBody = collisionManifold.Second;

                SoftFloat firstStatic = firstBody.IsStatic ? SoftFloat.One : SoftFloat.Zero;
                SoftFloat secondStatic = secondBody.IsStatic ? SoftFloat.One : SoftFloat.Zero;

                SoftVector3 resolution = collisionManifold.Collision.PenetrationNormal
                    * collisionManifold.Collision.PenetrationDepth
                    / SoftMath.Max(SoftFloat.One, firstStatic + secondStatic);

                firstBody.Position -= resolution * (SoftFloat.One - firstStatic);
                secondBody.Position += resolution * (SoftFloat.One - secondStatic);
            }
        }
    }
}
