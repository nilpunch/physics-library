using System;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public class BoxCollider<TNumber> : ISMCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly Vector3<TNumber> _centre;
        private readonly Vector3<TNumber> _extents;

        public BoxCollider(Vector3<TNumber> centre, Vector3<TNumber> extents)
        {
            _centre = centre;
            _extents = extents;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public Vector3<TNumber> Centre => _centre;

        public Vector3<TNumber> SupportPoint(Vector3<TNumber> direction)
        {
            return _centre + _extents * Vector3<TNumber>.SignComponents(direction);
        }
    }
}
