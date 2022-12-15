using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class TransformCollider : ISupportMappingCollider
    {
        private readonly ISupportMappingCollider _collider;
        private readonly ITransform _transform;

        public TransformCollider(ISupportMappingCollider collider, ITransform transform)
        {
            _collider = collider;
            _transform = transform;
        }

        public SoftVector3 Centre
        {
            get
            {
                var transformedCentre = _transform.Rotation * _collider.Centre + _transform.Position;
                return transformedCentre;
            }
        }

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            SoftVector3 rotatedDirection = SoftUnitQuaternion.Inverse(_transform.Rotation) * direction;
            var supportPoint = _collider.SupportPoint(rotatedDirection);
            var transformedSupportPoint = _transform.Rotation * supportPoint + _transform.Position;
            return transformedSupportPoint;
        }
    }
}
