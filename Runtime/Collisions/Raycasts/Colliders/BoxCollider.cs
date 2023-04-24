using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class BoxCollider<TNumber> : IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly Box<TNumber> _box;

        public BoxCollider(Box<TNumber> box)
        {
            _box = box;
        }

        public Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> raycastCollider)
        {
            return raycastCollider.BoxCast(_box);
        }

        public Collision<TNumber> BoxCast(Box<TNumber> box)
        {
            return AnalyticCollisionsLibrary<TNumber>.BoxAgainstBox(_box, box);
        }

        public Collision<TNumber> SphereCast(Sphere<TNumber> sphere)
        {
            return AnalyticCollisionsLibrary<TNumber>.SphereAgainstBox(sphere, _box);
        }

        public Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull)
        {
            return AnalyticCollisionsLibrary<TNumber>.ConvexAgainstBox(convexHull, _box);
        }

        public Collision<TNumber> AABBCast(AABB<TNumber> aabb)
        {
            return AnalyticCollisionsLibrary<TNumber>.AABBAgainstBox(aabb, _box);
        }

        public Collision<TNumber> Raycast(Vector3<TNumber> @from, Vector3<TNumber> direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
