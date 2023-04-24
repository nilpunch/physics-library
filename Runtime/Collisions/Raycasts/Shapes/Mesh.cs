using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public struct Mesh<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public readonly Vector3<TNumber>[] Vertices;
        public readonly int[] Indices;
    }
}
