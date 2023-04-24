using PluggableMath;

namespace GameLibrary.Physics
{
    public class PhysicSimulation<TNumber> : IPhysicSimulation<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IPhysicSimulationStep<TNumber>[] _physicSimulationSteps;
        private readonly int _substeps;

        public PhysicSimulation(IPhysicSimulationStep<TNumber>[] physicSimulationSteps, int substeps = 4)
        {
            _physicSimulationSteps = physicSimulationSteps;
            _substeps = substeps;
        }

        public void Step(Operand<TNumber> deltaTime)
        {
            Operand<TNumber> subStepDeltaTime = deltaTime / (Operand<TNumber>)_substeps;

            for (int substep = 0; substep < _substeps; ++substep)
            {
                foreach (var physicSimulationStep in _physicSimulationSteps)
                {
                    physicSimulationStep.Step(subStepDeltaTime);
                }
            }
        }
    }
}
