using GameLibrary.Mathematics;

namespace GameLibrary.Physics.GJK
{
    public class Box : ISupportMappingCollider
    {
        private readonly SoftVector3 _centre;
        private readonly SoftVector3 _extents;

        public Box(SoftVector3 centre, SoftVector3 extents)
        {
            _centre = centre;
            _extents = extents;
        }

        public SoftVector3 Centre => _centre;

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            return _centre + _extents * SoftVector3.SignComponents(direction);
        }
    }
}
