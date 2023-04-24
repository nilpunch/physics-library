using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class TranslateRotateCollider<TNumber> : ISMCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly ISMCollider<TNumber> _collider;
        private readonly UnitQuaternion<TNumber> _rotation;
        private readonly Vector3<TNumber> _translation;

        public TranslateRotateCollider(ISMCollider<TNumber> collider,
            UnitQuaternion<TNumber> rotation,
            Vector3<TNumber> translation)
        {
            _collider = collider;
            _rotation = rotation;
            _translation = translation;
        }

        public Vector3<TNumber> Centre
        {
            get
            {
                var transformedCentre = _rotation * _collider.Centre + _translation;
                return transformedCentre;
            }
        }

        public Vector3<TNumber> SupportPoint(Vector3<TNumber> direction)
        {
            Vector3<TNumber> rotatedDirection = UnitQuaternion<TNumber>.Inverse(_rotation) * direction;
            var supportPoint = _collider.SupportPoint(rotatedDirection);
            var transformedSupportPoint = _rotation * supportPoint + _translation;
            return transformedSupportPoint;
        }
    }
}
