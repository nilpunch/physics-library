using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public readonly struct CollisionFrictionJacobian
    {
        public enum FrictionDirection
        {
            Tangent,
            Bitangent
        }

        private readonly CollisionManifold<IRigidbody> _collisionManifold;
        private readonly FrictionDirection _frictionDirection;
        private readonly SoftFloat _normalJacobianLambda;

        public CollisionFrictionJacobian(CollisionManifold<IRigidbody> collisionManifold, FrictionDirection frictionDirection, SoftFloat normalJacobianLambda)
        {
            _collisionManifold = collisionManifold;
            _frictionDirection = frictionDirection;
            _normalJacobianLambda = normalJacobianLambda;
        }

        public void Resolve(SoftFloat dt)
        {
            SoftVector3 fromCenterToContactA = _collisionManifold.Collision.ContactFirst.Position -
                                               _collisionManifold.First.CenterOfMassWorldSpace;
            SoftVector3 fromCenterToContactB = _collisionManifold.Collision.ContactSecond.Position -
                                               _collisionManifold.Second.CenterOfMassWorldSpace;

            SoftVector3 tangent = SoftVector3.Orthonormal(_collisionManifold.Collision.PenetrationNormal);
            SoftVector3 bitangent = SoftVector3.Cross(_collisionManifold.Collision.PenetrationNormal, tangent);

            SoftVector3 jacobianDirection = _frictionDirection == FrictionDirection.Tangent ? tangent : bitangent;

            var jacobianLinearVelocityA = -jacobianDirection;
            var jacobianAngularVelocityA = -SoftVector3.Cross(fromCenterToContactA, jacobianDirection);
            var jactobianLinearVelocityB = jacobianDirection;
            var jacobianAngularVelocityB = SoftVector3.Cross(fromCenterToContactB, jacobianDirection);

            var bias = SoftFloat.Zero;

            SoftFloat effectiveMassInverse = _collisionManifold.First.InverseMass +
                                             SoftVector3.Dot(jacobianAngularVelocityA,
                                                 _collisionManifold.First.InverseInertiaWorldSpace * jacobianAngularVelocityA) +
                                             _collisionManifold.Second.InverseMass +
                                             SoftVector3.Dot(jacobianAngularVelocityB,
                                                 _collisionManifold.Second.InverseInertiaWorldSpace * jacobianAngularVelocityB);

            var effectiveMass = SoftFloat.One / effectiveMassInverse;

            // JV = Jacobian * velocity vector
            SoftFloat jv = SoftVector3.Dot(jacobianLinearVelocityA, _collisionManifold.First.LinearVelocity) +
                           SoftVector3.Dot(jacobianAngularVelocityA, _collisionManifold.First.AngularVelocity) +
                           SoftVector3.Dot(jactobianLinearVelocityB, _collisionManifold.Second.LinearVelocity) +
                           SoftVector3.Dot(jacobianAngularVelocityB, _collisionManifold.Second.AngularVelocity);

            // raw lambda
            SoftFloat lambda = effectiveMass * (-(jv + bias));

            SoftFloat friction = _collisionManifold.First.Friction * _collisionManifold.Second.Friction;
            SoftFloat maxFriction = friction * _normalJacobianLambda;
            lambda = SoftMath.Clamp(lambda, -maxFriction, maxFriction);

            // velocity correction
            _collisionManifold.First.LinearVelocity +=
                _collisionManifold.First.InverseMass * jacobianLinearVelocityA * lambda;
            _collisionManifold.First.AngularVelocity +=
                _collisionManifold.First.InverseInertiaWorldSpace * jacobianAngularVelocityA * lambda;
            _collisionManifold.Second.LinearVelocity +=
                _collisionManifold.Second.InverseMass * jactobianLinearVelocityB * lambda;
            _collisionManifold.Second.AngularVelocity +=
                _collisionManifold.Second.InverseInertiaWorldSpace * jacobianAngularVelocityB * lambda;
        }
    }
}
