using PluggableMath;

namespace GameLibrary.Physics
{
    public readonly struct RaycastHit<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public bool Occure { get; }

        public ContactPoint<TNumber> ContactPoint { get; }
    }
}
