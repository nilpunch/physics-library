using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class PhysicSimulation : IPhysicSimulation
    {
        private readonly IPhysicSimulationStep[] _physicSimulationSteps;
        private readonly int _substeps;

        public PhysicSimulation(IPhysicSimulationStep[] physicSimulationSteps, int substeps = 4)
        {
            _physicSimulationSteps = physicSimulationSteps;
            _substeps = substeps;
        }

        public void Step(SoftFloat deltaTime)
        {
            SoftFloat subStepDeltaTime = deltaTime / (SoftFloat)_substeps;

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
