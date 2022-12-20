namespace GameLibrary.Physics
{
    public interface ICollisionsSolver<in TBody>
	{
		void Solve(ICollisionManifold<TBody>[] bodiesCollisions, long timeStep);
	}
}
