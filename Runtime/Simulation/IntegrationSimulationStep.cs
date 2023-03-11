using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class IntegrationSimulationStep : IPhysicSimulationStep
    {
        private readonly IIntegrator _integrator;

        public IntegrationSimulationStep(IIntegrator integrator)
        {
            _integrator = integrator;
        }
        
        public void Step(SoftFloat deltaTime)
        {
            _integrator.Integrate(deltaTime);
        }
    }
}
