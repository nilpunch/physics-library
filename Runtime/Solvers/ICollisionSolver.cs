namespace GameLibrary.Physics
{
    public interface ICollisionSolver<in TBody>
	{
		void Solve(ICollisionManifold<TBody>[] bodiesCollisions, long timeStep);
	}
}
