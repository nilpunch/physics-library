using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class RigidbodyPositionSolver : ICollisionsSolver<IRigidbody>
    {
        public void Solve(ICollisionManifold<IRigidbody>[] bodiesCollisions, long timeStep)
        {
            foreach (ICollisionManifold<IRigidbody> collisionManifold in bodiesCollisions)
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
