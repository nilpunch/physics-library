using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public struct ConvexHull<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Vector3<TNumber> Center { get; }
        Mesh<TNumber> Mesh { get; }
    }
}
