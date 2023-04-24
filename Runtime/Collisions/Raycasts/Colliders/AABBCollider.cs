using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
	public class AABBCollider<TNumber> : IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
	{
		private readonly AABB<TNumber> _aabb;

		public AABBCollider(AABB<TNumber> aabb)
		{
			_aabb = aabb;
		}

		public Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> raycastCollider)
		{
			return raycastCollider.AABBCast(_aabb);
		}

		public Collision<TNumber> BoxCast(Box<TNumber> box)
		{
			return AnalyticCollisionsLibrary<TNumber>.AABBAgainstBox(_aabb, box);
		}

		public Collision<TNumber> SphereCast(Sphere<TNumber> sphere)
		{
			return AnalyticCollisionsLibrary<TNumber>.AABBAgainstSphere(_aabb, sphere);
		}

		public Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull)
		{
			return AnalyticCollisionsLibrary<TNumber>.AABBAgainstConvexHull(_aabb, convexHull);
		}

		public Collision<TNumber> AABBCast(AABB<TNumber> aabb)
		{
			return AnalyticCollisionsLibrary<TNumber>.AABBAgainstAABB(_aabb, aabb);
		}

        public Collision<TNumber> Raycast(Vector3<TNumber> @from, Vector3<TNumber> direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
