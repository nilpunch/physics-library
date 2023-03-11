using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class ConstraintSimulationStep : IPhysicSimulationStep
    {
        private readonly IConstraint _constraint;

        public ConstraintSimulationStep(IConstraint constraint)
        {
            _constraint = constraint;
        }
        
        public void Step(SoftFloat deltaTime)
        {
            _constraint.Solve(deltaTime);
        }
    }
}
