using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class TranslateRotateCollider : ISMCollider
    {
        private readonly ISMCollider _collider;
        private readonly SoftUnitQuaternion _rotation;
        private readonly SoftVector3 _translation;

        public TranslateRotateCollider(ISMCollider collider,
            SoftUnitQuaternion rotation,
            SoftVector3 translation)
        {
            _collider = collider;
            _rotation = rotation;
            _translation = translation;
        }

        public Guid Id => _collider.Id;

        public SoftVector3 Centre
        {
            get
            {
                var transformedCentre = _rotation * _collider.Centre + _translation;
                return transformedCentre;
            }
        }

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            SoftVector3 rotatedDirection = SoftUnitQuaternion.Inverse(_rotation) * direction;
            var supportPoint = _collider.SupportPoint(rotatedDirection);
            var transformedSupportPoint = _rotation * supportPoint + _translation;
            return transformedSupportPoint;
        }
    }
}
