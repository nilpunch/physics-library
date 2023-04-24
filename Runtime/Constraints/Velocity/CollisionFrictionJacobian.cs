using PluggableMath;

namespace GameLibrary.Physics
{
    public readonly struct CollisionFrictionJacobian<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public enum FrictionDirection
        {
            Tangent,
            Bitangent
        }

        private readonly CollisionManifold<TNumber, IRigidbody<TNumber>> _collisionManifold;
        private readonly FrictionDirection _frictionDirection;
        private readonly Operand<TNumber> _normalJacobianLambda;

        public CollisionFrictionJacobian(CollisionManifold<TNumber, IRigidbody<TNumber>> collisionManifold, FrictionDirection frictionDirection, Operand<TNumber> normalJacobianLambda)
        {
            _collisionManifold = collisionManifold;
            _frictionDirection = frictionDirection;
            _normalJacobianLambda = normalJacobianLambda;
        }

        public void Resolve(Operand<TNumber> dt)
        {
            Vector3<TNumber> fromCenterToContactA = _collisionManifold.Collision.ContactFirst.Position -
                                               _collisionManifold.First.CenterOfMassWorldSpace;
            Vector3<TNumber> fromCenterToContactB = _collisionManifold.Collision.ContactSecond.Position -
                                               _collisionManifold.Second.CenterOfMassWorldSpace;

            Vector3<TNumber> tangent = Vector3<TNumber>.Orthonormal(_collisionManifold.Collision.PenetrationNormal);
            Vector3<TNumber> bitangent = Vector3<TNumber>.Cross(_collisionManifold.Collision.PenetrationNormal, tangent);

            Vector3<TNumber> jacobianDirection = _frictionDirection == FrictionDirection.Tangent ? tangent : bitangent;

            var jacobianLinearVelocityA = -jacobianDirection;
            var jacobianAngularVelocityA = -Vector3<TNumber>.Cross(fromCenterToContactA, jacobianDirection);
            var jactobianLinearVelocityB = jacobianDirection;
            var jacobianAngularVelocityB = Vector3<TNumber>.Cross(fromCenterToContactB, jacobianDirection);

            var bias = Operand<TNumber>.Zero;

            Operand<TNumber> effectiveMassInverse = _collisionManifold.First.InverseMass +
                                             Vector3<TNumber>.Dot(jacobianAngularVelocityA,
                                                 _collisionManifold.First.InverseInertiaWorldSpace * jacobianAngularVelocityA) +
                                             _collisionManifold.Second.InverseMass +
                                             Vector3<TNumber>.Dot(jacobianAngularVelocityB,
                                                 _collisionManifold.Second.InverseInertiaWorldSpace * jacobianAngularVelocityB);

            var effectiveMass = Operand<TNumber>.One / effectiveMassInverse;

            // JV = Jacobian * velocity vector
            Operand<TNumber> jv = Vector3<TNumber>.Dot(jacobianLinearVelocityA, _collisionManifold.First.LinearVelocity) +
                           Vector3<TNumber>.Dot(jacobianAngularVelocityA, _collisionManifold.First.AngularVelocity) +
                           Vector3<TNumber>.Dot(jactobianLinearVelocityB, _collisionManifold.Second.LinearVelocity) +
                           Vector3<TNumber>.Dot(jacobianAngularVelocityB, _collisionManifold.Second.AngularVelocity);

            // raw lambda
            Operand<TNumber> lambda = effectiveMass * (-(jv + bias));

            Operand<TNumber> friction = _collisionManifold.First.Friction * _collisionManifold.Second.Friction;
            Operand<TNumber> maxFriction = friction * _normalJacobianLambda;
            lambda = Math<TNumber>.Clamp(lambda, -maxFriction, maxFriction);

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
