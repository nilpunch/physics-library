using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IPhysicSimulationStep
    {
        void Step(SoftFloat deltaTime);
    }
}
