using PluggableMath;

namespace GameLibrary.Physics
{
    public class ConstraintSimulationStep<TNumber> : IPhysicSimulationStep<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IConstraint<TNumber> _constraint;

        public ConstraintSimulationStep(IConstraint<TNumber> constraint)
        {
            _constraint = constraint;
        }

        public void Step(Operand<TNumber> deltaTime)
        {
            _constraint.Solve(deltaTime);
        }
    }
}
