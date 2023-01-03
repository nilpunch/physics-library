using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
	public class AABBCollider : IDoubleCastCollider
	{
		private readonly AABB _aabb;

		public AABBCollider(AABB aabb)
		{
			_aabb = aabb;
		}

		public Collision ColliderCast(IRaycastCollider raycastCollider)
		{
			return raycastCollider.AABBCast(_aabb);
		}

		public Collision BoxCast(Box box)
		{
			return AnalyticCollisionsLibrary.AABBAgainstBox(_aabb, box);
		}

		public Collision SphereCast(Sphere sphere)
		{
			return AnalyticCollisionsLibrary.AABBAgainstSphere(_aabb, sphere);
		}

		public Collision ConvexHullCast(ConvexHull convexHull)
		{
			return AnalyticCollisionsLibrary.AABBAgainstConvexHull(_aabb, convexHull);
		}

		public Collision AABBCast(AABB aabb)
		{
			return AnalyticCollisionsLibrary.AABBAgainstAABB(_aabb, aabb);
		}

        public Collision Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
