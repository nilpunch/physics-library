using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public struct CollisionResolutionJacobian
    {
        private readonly CollisionManifold<IRigidbody> _collisionManifold;

        public SoftFloat Lambda { get; private set; }

        public CollisionResolutionJacobian(CollisionManifold<IRigidbody> collisionManifold)
        {
            _collisionManifold = collisionManifold;
            Lambda = SoftFloat.Zero;
        }

        public void Resolve(SoftFloat dt)
        {
            SoftVector3 fromCenterToContactA = _collisionManifold.Collision.ContactFirst.Position -
                                               _collisionManifold.First.CenterOfMassWorldSpace;
            SoftVector3 fromCenterToContactB = _collisionManifold.Collision.ContactSecond.Position -
                                               _collisionManifold.Second.CenterOfMassWorldSpace;

            SoftVector3 jacobianDirection = _collisionManifold.Collision.PenetrationNormal;

            var jacobianLinearVelocityA = -jacobianDirection;
            var jacobianAngularVelocityA = -SoftVector3.Cross(fromCenterToContactA, jacobianDirection);
            var jactobianLinearVelocityB = jacobianDirection;
            var jacobianAngularVelocityB = SoftVector3.Cross(fromCenterToContactB, jacobianDirection);


            SoftFloat beta = _collisionManifold.First.ContactBeta * _collisionManifold.Second.ContactBeta;
            SoftFloat restitution = _collisionManifold.First.Restitution * _collisionManifold.Second.Restitution;
            SoftVector3 relativeVelocity = -_collisionManifold.First.LinearVelocity -
                                           SoftVector3.Cross(_collisionManifold.First.AngularVelocity,
                                               fromCenterToContactA) +
                                           _collisionManifold.Second.LinearVelocity +
                                           SoftVector3.Cross(_collisionManifold.Second.AngularVelocity,
                                               fromCenterToContactB);
            SoftFloat closingVelocity = SoftVector3.Dot(relativeVelocity, jacobianDirection);
            var bias = -(beta / dt) * _collisionManifold.Collision.PenetrationDepth + restitution * closingVelocity;

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

            lambda = SoftMath.Max(SoftFloat.Zero, lambda);

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
    //     private SoftVector3 _linearVelocityA; // Jacobian components for linear velocity of body A
    //     private SoftVector3 _angularVelocityA; // Jacobian components for angular velocity of body A
    //     private SoftVector3 _linearVelocityB; // Jacobian components for linear velocity of body B
    //     private SoftVector3 _angularVelocityB; // Jacobian components for angular velocity of body B
    //     private SoftFloat _bias;
    //     private SoftFloat _effectiveMass;
    //     private SoftFloat _totalLambda;
    //
    //     public Jacobian(Type type)
    //     {
    //         m_type = type;
    //         _linearVelocityA = SoftVector3.Zero;
    //         _angularVelocityA = SoftVector3.Zero;
    //         _linearVelocityB = SoftVector3.Zero;
    //         _angularVelocityB = SoftVector3.Zero;
    //         _bias = SoftFloat.Zero;
    //         _effectiveMass = SoftFloat.Zero;
    //         _totalLambda = SoftFloat.Zero;
    //     }
    //
    //     public void Init(Contact _collisionManifold, SoftVector3 dir, SoftFloat dt)
    //     {
    //         _linearVelocityA = -dir;
    //         _angularVelocityA = -SoftVector3.Cross(fromCenterToContactA, dir);
    //         _linearVelocityB = dir;
    //         _angularVelocityB = SoftVector3.Cross(fromCenterToContactB, dir);
    //
    //         _bias = SoftFloat.Zero;
    //         if (m_type == Type.Normal)
    //         {
    //             SoftFloat beta = _collisionManifold.First.ContactBeta * _collisionManifold.Second.ContactBeta;
    //             SoftFloat restitution = _collisionManifold.First.Restitution * _collisionManifold.Second.Restitution;
    //             SoftVector3 relativeVelocity = -_collisionManifold.First.LinearVelocity -
    //                                            SoftVector3.Cross(_collisionManifold.First.AngularVelocity,
    //                                                fromCenterToContactA) +
    //                                            _collisionManifold.Second.LinearVelocity +
    //                                            SoftVector3.Cross(_collisionManifold.Second.AngularVelocity,
    //                                                fromCenterToContactB);
    //             SoftFloat closingVelocity = SoftVector3.Dot(relativeVelocity, dir);
    //             _bias = -(beta / dt) * _collisionManifold.Penetration + restitution * closingVelocity;
    //         }
    //
    //         SoftFloat effectiveMassInverse = _collisionManifold.First.InverseMass + SoftVector3.Dot(_angularVelocityA, _collisionManifold.First.InverseInertiaWs * _angularVelocityA) +
    //                       _collisionManifold.Second.InverseMass + SoftVector3.Dot(_angularVelocityB, _collisionManifold.Second.InverseInertiaWs * _angularVelocityB);
    //
    //         _effectiveMass = SoftFloat.One / effectiveMassInverse;
    //         _totalLambda = SoftFloat.Zero;
    //     }
    //
    //     public void Resolve(Contact _collisionManifold, SoftFloat dt)
    //     {
    //         SoftVector3 dir = _linearVelocityB;
    //
    //         // JV = Jacobian * velocity vector
    //         SoftFloat jv = SoftVector3.Dot(_linearVelocityA, _collisionManifold.First.LinearVelocity) +
    //                        SoftVector3.Dot(_angularVelocityA, _collisionManifold.First.AngularVelocity) +
    //                        SoftVector3.Dot(_linearVelocityB, _collisionManifold.Second.LinearVelocity) +
    //                        SoftVector3.Dot(_angularVelocityB, _collisionManifold.Second.AngularVelocity);
    //
    //         // raw lambda
    //         SoftFloat lambda = _effectiveMass * (-(jv + _bias));
    //
    //         // clamped lambda
    //         //   normal  / _collisionManifold resolution  :  lambda >= 0
    //         //   tangent / friction            :  -maxFriction <= lambda <= maxFriction
    //         SoftFloat oldTotalLambda = _totalLambda;
    //         switch (m_type)
    //         {
    //             case Type.Normal:
    //                 _totalLambda = Mathf.Max(SoftFloat.Zero, _totalLambda + lambda);
    //                 break;
    //
    //             case Type.Tangent:
    //                 SoftFloat friction = _collisionManifold.First.Friction * _collisionManifold.Second.Friction;
    //                 SoftFloat maxFriction = friction * _collisionManifold.m_jN.m_totalLambda;
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
