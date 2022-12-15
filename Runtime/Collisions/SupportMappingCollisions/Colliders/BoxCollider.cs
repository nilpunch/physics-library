using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public class BoxCollider : ISMCollider
    {
        private readonly SoftVector3 _centre;
        private readonly SoftVector3 _extents;

        public BoxCollider(SoftVector3 centre, SoftVector3 extents)
        {
            _centre = centre;
            _extents = extents;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public SoftVector3 Centre => _centre;

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            return _centre + _extents * SoftVector3.SignComponents(direction);
        }
    }
}
