using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class DynamicCollider<TNumber> : ISMCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly ISMCollider<TNumber> _collider;
        private readonly IReadOnlyTransform<TNumber> _readOnlyTransform;

        public DynamicCollider(IReadOnlyTransform<TNumber> readOnlyTransform, ISMCollider<TNumber> collider)
        {
            _collider = collider;
            _readOnlyTransform = readOnlyTransform;
        }

        public Vector3<TNumber> Centre
        {
            get
            {
                var transformedCentre = _readOnlyTransform.Rotation * _collider.Centre + _readOnlyTransform.Position;
                return transformedCentre;
            }
        }

        public Vector3<TNumber> SupportPoint(Vector3<TNumber> direction)
        {
            Vector3<TNumber> rotatedDirection = UnitQuaternion<TNumber>.Inverse(_readOnlyTransform.Rotation) * direction;
            var supportPoint = _collider.SupportPoint(rotatedDirection);
            var transformedSupportPoint = _readOnlyTransform.Rotation * supportPoint + _readOnlyTransform.Position;
            return transformedSupportPoint;
        }
    }
}
