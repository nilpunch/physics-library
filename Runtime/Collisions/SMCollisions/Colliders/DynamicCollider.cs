using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class DynamicCollider : ISMCollider
    {
        private readonly ISMCollider _collider;
        private readonly IReadOnlyTransform _readOnlyTransform;

        public DynamicCollider(IReadOnlyTransform readOnlyTransform, ISMCollider collider)
        {
            _collider = collider;
            _readOnlyTransform = readOnlyTransform;
        }

        public SoftVector3 Centre
        {
            get
            {
                var transformedCentre = _readOnlyTransform.Rotation * _collider.Centre + _readOnlyTransform.Position;
                return transformedCentre;
            }
        }

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            SoftVector3 rotatedDirection = SoftUnitQuaternion.Inverse(_readOnlyTransform.Rotation) * direction;
            var supportPoint = _collider.SupportPoint(rotatedDirection);
            var transformedSupportPoint = _readOnlyTransform.Rotation * supportPoint + _readOnlyTransform.Position;
            return transformedSupportPoint;
        }
    }
}
