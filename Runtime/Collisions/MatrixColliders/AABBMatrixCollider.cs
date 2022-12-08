namespace GameLibrary.Physics
{
	public class AABBMatrixCollider : IMatrixCollider
	{
		private readonly AABB _aabb;
		private readonly IMatrixCollisionsLibrary _matrixCollisionsLibrary;

		public AABBMatrixCollider(AABB aabb, IMatrixCollisionsLibrary matrixCollisionsLibrary)
		{
			_aabb = aabb;
			_matrixCollisionsLibrary = matrixCollisionsLibrary;
		}
        
		public Collision Collide(IMatrixCollider matrixCollider)
		{
			return matrixCollider.CollideAgainstAABB(_aabb);
		}

		public Collision CollideAgainstBox(Box box)
		{
			return _matrixCollisionsLibrary.AABBAgainstBox(_aabb, box);
		}

		public Collision CollideAgainstSphere(Sphere sphere)
		{
			return _matrixCollisionsLibrary.AABBAgainstSphere(_aabb, sphere);
		}

		public Collision CollideAgainstConvexHull(ConvexHull convexHull)
		{
			return _matrixCollisionsLibrary.AABBAgainstConvexHull(_aabb, convexHull);
		}

		public Collision CollideAgainstAABB(AABB aabb)
		{
			return _matrixCollisionsLibrary.AABBAgainstAABB(_aabb, aabb);
		}
	}
}