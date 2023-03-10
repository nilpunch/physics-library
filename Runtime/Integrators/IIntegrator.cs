using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IIntegrator
    {
        void Integrate(SoftFloat deltaTime);
    }
}
