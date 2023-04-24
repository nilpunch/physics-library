using System;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class SphereCollider<TNumber> : ISMCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly Vector3<TNumber> _centre;
        private readonly Operand<TNumber> _radius;

        public SphereCollider(Vector3<TNumber> centre, Operand<TNumber> radius)
        {
            _centre = centre;
            _radius = radius;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public Vector3<TNumber> Centre => _centre;

        public Vector3<TNumber> SupportPoint(Vector3<TNumber> direction)
        {
            return _centre + _radius * Vector3<TNumber>.Normalize(direction);
        }
    }
}
