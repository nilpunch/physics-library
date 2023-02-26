using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class DynamicCollider : ISMCollider
    {
        private readonly ISMCollider _collider;
        private readonly IRigidbody _rigidbody;

        public DynamicCollider(IRigidbody rigidbody, ISMCollider collider)
        {
            _collider = collider;
            _rigidbody = rigidbody;
        }

        public SoftVector3 Centre
        {
            get
            {
                var transformedCentre = _rigidbody.Rotation * _collider.Centre + _rigidbody.Position;
                return transformedCentre;
            }
        }

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            SoftVector3 rotatedDirection = SoftUnitQuaternion.Inverse(_rigidbody.Rotation) * direction;
            var supportPoint = _collider.SupportPoint(rotatedDirection);
            var transformedSupportPoint = _rigidbody.Rotation * supportPoint + _rigidbody.Position;
            return transformedSupportPoint;
        }
    }
}
