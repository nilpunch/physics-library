using PluggableMath;

namespace GameLibrary.Physics
{
    public readonly struct ConcreteCastHit<TNumber, THitResult> where TNumber : struct, INumber<TNumber>
    {
        public bool Occure { get; }

        public ContactPoint<TNumber>[] ContactPoint { get; }

        public THitResult HitResult { get; }
    }
}
