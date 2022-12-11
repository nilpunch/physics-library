namespace GameLibrary.Physics.MatrixColliders
{
	public class AABBAnalyticCollider : IAnalyticCollider
	{
		private readonly AABB _aabb;
		private readonly IAnalyticCollisionsLibrary _analyticCollisionsLibrary;

		public AABBAnalyticCollider(AABB aabb, IAnalyticCollisionsLibrary analyticCollisionsLibrary)
		{
			_aabb = aabb;
			_analyticCollisionsLibrary = analyticCollisionsLibrary;
		}

		public Collision Collide(IAnalyticCollider analyticCollider)
		{
			return analyticCollider.CollideAgainstAABB(_aabb);
		}

		public Collision CollideAgainstBox(Box box)
		{
			return _analyticCollisionsLibrary.AABBAgainstBox(_aabb, box);
		}

		public Collision CollideAgainstSphere(Sphere sphere)
		{
			return _analyticCollisionsLibrary.AABBAgainstSphere(_aabb, sphere);
		}

		public Collision CollideAgainstConvexHull(ConvexHull convexHull)
		{
			return _analyticCollisionsLibrary.AABBAgainstConvexHull(_aabb, convexHull);
		}

		public Collision CollideAgainstAABB(AABB aabb)
		{
			return _analyticCollisionsLibrary.AABBAgainstAABB(_aabb, aabb);
		}
	}
}
