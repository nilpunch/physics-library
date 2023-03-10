using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IConstraint
    {
		void Solve(SoftFloat deltaTime);
	}
}
