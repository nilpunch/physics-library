using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public readonly struct Sphere<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public readonly Vector3<TNumber> Center;
        public readonly Operand<TNumber> Radius;
    }
}
