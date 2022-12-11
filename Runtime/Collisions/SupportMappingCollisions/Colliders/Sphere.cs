using GameLibrary.Mathematics;

namespace GameLibrary.Physics.GJK
{
    public class Sphere : ISupportMappingCollider
    {
        private readonly SoftVector3 _centre;
        private readonly SoftFloat _radius;

        public Sphere(SoftVector3 centre, SoftFloat radius)
        {
            _centre = centre;
            _radius = radius;
        }

        public SoftVector3 Centre => _centre;

        public SoftVector3 SupportPoint(SoftVector3 direction)
        {
            return _centre + _radius * SoftVector3.Normalize(direction);
        }
    }
}
