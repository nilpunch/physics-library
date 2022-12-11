namespace GameLibrary.Physics
{
    public interface ICollisionSolver<TBody>
	{
		void Solve(BodiesCollision<TBody>[] bodiesCollisions, long timeStep);
	}
}
