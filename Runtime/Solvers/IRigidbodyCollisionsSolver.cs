namespace GameLibrary.Physics
{
    public interface IRigidbodyCollisionsSolver
	{
		void Solve(CollisionManifold<IRigidbody>[] bodiesCollisions, long timeStep);
	}
}
