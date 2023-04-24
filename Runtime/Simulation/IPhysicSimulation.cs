using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IPhysicSimulation<TNumber> where TNumber : struct, INumber<TNumber>
    {
        void Step(Operand<TNumber> deltaTime);
    }
}
