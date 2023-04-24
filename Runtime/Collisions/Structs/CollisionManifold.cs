using PluggableMath;

namespace GameLibrary.Physics
{
    public readonly struct CollisionManifold<TNumber, TBody> where TNumber : struct, INumber<TNumber>
    {
        public CollisionManifold(TBody first, TBody second, Collision<TNumber> collision)
        {
            First = first;
            Second = second;
            Collision = collision;
        }

        public TBody First { get; }
        public TBody Second { get; }
        public Collision<TNumber> Collision { get; }
    }
}
