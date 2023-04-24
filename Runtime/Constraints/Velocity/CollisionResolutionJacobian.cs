using PluggableMath;

namespace GameLibrary.Physics
{
    public struct CollisionResolutionJacobian<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly CollisionManifold<TNumber, IRigidbody<TNumber>> _collisionManifold;

        public Operand<TNumber> Lambda { get; private set; }

        public CollisionResolutionJacobian(CollisionManifold<TNumber, IRigidbody<TNumber>> collisionManifold)
        {
            _collisionManifold = collisionManifold;
            Lambda = Operand<TNumber>.Zero;
        }

        public void Resolve(Operand<TNumber> dt)
        {
            Vector3<TNumber> fromCenterToContactA = _collisionManifold.Collision.ContactFirst.Position -
                                               _collisionManifold.First.CenterOfMassWorldSpace;
            Vector3<TNumber> fromCenterToContactB = _collisionManifold.Collision.ContactSecond.Position -
                                               _collisionManifold.Second.CenterOfMassWorldSpace;

            Vector3<TNumber> jacobianDirection = _collisionManifold.Collision.PenetrationNormal;

            var jacobianLinearVelocityA = -jacobianDirection;
            var jacobianAngularVelocityA = -Vector3<TNumber>.Cross(fromCenterToContactA, jacobianDirection);
            var jactobianLinearVelocityB = jacobianDirection;
            var jacobianAngularVelocityB = Vector3<TNumber>.Cross(fromCenterToContactB, jacobianDirection);


            Operand<TNumber> beta = _collisionManifold.First.ContactBeta * _collisionManifold.Second.ContactBeta;
            Operand<TNumber> restitution = _collisionManifold.First.Restitution * _collisionManifold.Second.Restitution;
            Vector3<TNumber> relativeVelocity = -_collisionManifold.First.LinearVelocity -
                                           Vector3<TNumber>.Cross(_collisionManifold.First.AngularVelocity,
                                               fromCenterToContactA) +
                                           _collisionManifold.Second.LinearVelocity +
                                           Vector3<TNumber>.Cross(_collisionManifold.Second.AngularVelocity,
                                               fromCenterToContactB);
            Operand<TNumber> closingVelocity = Vector3<TNumber>.Dot(relativeVelocity, jacobianDirection);
            var bias = -(beta / dt) * _collisionManifold.Collision.PenetrationDepth + restitution * closingVelocity;

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

            lambda = Math<TNumber>.Max(Operand<TNumber>.Zero, lambda);

            Lambda = lambda;

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

    // public struct Jacobian
    // {
    //     public enum Type
    //     {
    //         Normal,
    //         Tangent,
    //     }
    //
    //     Type m_type;
    //
    //     private Vector3<TNumber> _linearVelocityA; // Jacobian components for linear velocity of body A
    //     private Vector3<TNumber> _angularVelocityA; // Jacobian components for angular velocity of body A
    //     private Vector3<TNumber> _linearVelocityB; // Jacobian components for linear velocity of body B
    //     private Vector3<TNumber> _angularVelocityB; // Jacobian components for angular velocity of body B
    //     private Operand<TNumber> _bias;
    //     private Operand<TNumber> _effectiveMass;
    //     private Operand<TNumber> _totalLambda;
    //
    //     public Jacobian(Type type)
    //     {
    //         m_type = type;
    //         _linearVelocityA = Vector3<TNumber>.Zero;
    //         _angularVelocityA = Vector3<TNumber>.Zero;
    //         _linearVelocityB = Vector3<TNumber>.Zero;
    //         _angularVelocityB = Vector3<TNumber>.Zero;
    //         _bias = Operand<TNumber>.Zero;
    //         _effectiveMass = Operand<TNumber>.Zero;
    //         _totalLambda = Operand<TNumber>.Zero;
    //     }
    //
    //     public void Init(Contact _collisionManifold, Vector3<TNumber> dir, Operand<TNumber> dt)
    //     {
    //         _linearVelocityA = -dir;
    //         _angularVelocityA = -Vector3<TNumber>.Cross(fromCenterToContactA, dir);
    //         _linearVelocityB = dir;
    //         _angularVelocityB = Vector3<TNumber>.Cross(fromCenterToContactB, dir);
    //
    //         _bias = Operand<TNumber>.Zero;
    //         if (m_type == Type.Normal)
    //         {
    //             Operand<TNumber> beta = _collisionManifold.First.ContactBeta * _collisionManifold.Second.ContactBeta;
    //             Operand<TNumber> restitution = _collisionManifold.First.Restitution * _collisionManifold.Second.Restitution;
    //             Vector3<TNumber> relativeVelocity = -_collisionManifold.First.LinearVelocity -
    //                                            Vector3<TNumber>.Cross(_collisionManifold.First.AngularVelocity,
    //                                                fromCenterToContactA) +
    //                                            _collisionManifold.Second.LinearVelocity +
    //                                            Vector3<TNumber>.Cross(_collisionManifold.Second.AngularVelocity,
    //                                                fromCenterToContactB);
    //             Operand<TNumber> closingVelocity = Vector3<TNumber>.Dot(relativeVelocity, dir);
    //             _bias = -(beta / dt) * _collisionManifold.Penetration + restitution * closingVelocity;
    //         }
    //
    //         Operand<TNumber> effectiveMassInverse = _collisionManifold.First.InverseMass + Vector3<TNumber>.Dot(_angularVelocityA, _collisionManifold.First.InverseInertiaWs * _angularVelocityA) +
    //                       _collisionManifold.Second.InverseMass + Vector3<TNumber>.Dot(_angularVelocityB, _collisionManifold.Second.InverseInertiaWs * _angularVelocityB);
    //
    //         _effectiveMass = Operand<TNumber>.One / effectiveMassInverse;
    //         _totalLambda = Operand<TNumber>.Zero;
    //     }
    //
    //     public void Resolve(Contact _collisionManifold, Operand<TNumber> dt)
    //     {
    //         Vector3<TNumber> dir = _linearVelocityB;
    //
    //         // JV = Jacobian * velocity vector
    //         Operand<TNumber> jv = Vector3<TNumber>.Dot(_linearVelocityA, _collisionManifold.First.LinearVelocity) +
    //                        Vector3<TNumber>.Dot(_angularVelocityA, _collisionManifold.First.AngularVelocity) +
    //                        Vector3<TNumber>.Dot(_linearVelocityB, _collisionManifold.Second.LinearVelocity) +
    //                        Vector3<TNumber>.Dot(_angularVelocityB, _collisionManifold.Second.AngularVelocity);
    //
    //         // raw lambda
    //         Operand<TNumber> lambda = _effectiveMass * (-(jv + _bias));
    //
    //         // clamped lambda
    //         //   normal  / _collisionManifold resolution  :  lambda >= 0
    //         //   tangent / friction            :  -maxFriction <= lambda <= maxFriction
    //         Operand<TNumber> oldTotalLambda = _totalLambda;
    //         switch (m_type)
    //         {
    //             case Type.Normal:
    //                 _totalLambda = Mathf.Max(Operand<TNumber>.Zero, _totalLambda + lambda);
    //                 break;
    //
    //             case Type.Tangent:
    //                 Operand<TNumber> friction = _collisionManifold.First.Friction * _collisionManifold.Second.Friction;
    //                 Operand<TNumber> maxFriction = friction * _collisionManifold.m_jN.m_totalLambda;
    //                 _totalLambda = Mathf.Clamp(_totalLambda + lambda, -maxFriction, maxFriction);
    //                 break;
    //         }
    //
    //         lambda = _totalLambda - oldTotalLambda;
    //
    //         // velocity correction
    //         _collisionManifold.First.LinearVelocity += _collisionManifold.First.InverseMass * _linearVelocityA * lambda;
    //         _collisionManifold.First.AngularVelocity += _collisionManifold.First.InverseInertiaWs * _angularVelocityA * lambda;
    //         _collisionManifold.Second.LinearVelocity += _collisionManifold.Second.InverseMass * _linearVelocityB * lambda;
    //         _collisionManifold.Second.AngularVelocity += _collisionManifold.Second.InverseInertiaWs * _angularVelocityB * lambda;
    //     }
    // }
}
