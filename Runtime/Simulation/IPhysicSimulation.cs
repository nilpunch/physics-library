using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IPhysicSimulation
    {
        void Step(SoftFloat deltaTime);
    }
}
