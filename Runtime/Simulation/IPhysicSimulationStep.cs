using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IPhysicSimulationStep<TNumber> where TNumber : struct, INumber<TNumber>
    {
        void Step(Operand<TNumber> deltaTime);
    }
}
