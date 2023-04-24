using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IIntegrator<TNumber> where TNumber : struct, INumber<TNumber>
    {
        void Integrate(Operand<TNumber> deltaTime);
    }
}
