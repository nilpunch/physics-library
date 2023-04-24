using PluggableMath;

namespace GameLibrary.Physics
{
    public class IntegrationSimulationStep<TNumber> : IPhysicSimulationStep<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IIntegrator<TNumber> _integrator;

        public IntegrationSimulationStep(IIntegrator<TNumber> integrator)
        {
            _integrator = integrator;
        }

        public void Step(Operand<TNumber> deltaTime)
        {
            _integrator.Integrate(deltaTime);
        }
    }
}
