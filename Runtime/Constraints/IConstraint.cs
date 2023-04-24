using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IConstraint<TNumber> where TNumber : struct, INumber<TNumber>
    {
		void Solve(Operand<TNumber> deltaTime);
	}
}
