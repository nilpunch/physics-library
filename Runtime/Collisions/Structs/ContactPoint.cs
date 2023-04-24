using PluggableMath;

namespace GameLibrary.Physics
{
    public struct ContactPoint<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public ContactPoint(Vector3<TNumber> position)
        {
            Position = position;
        }

        public Vector3<TNumber> Position { get; }
    }
}
