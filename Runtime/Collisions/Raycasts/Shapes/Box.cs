using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public readonly struct Box<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Vector3<TNumber> Center { get; }
        Quaternion<TNumber> Rotation { get; }
        Vector3<TNumber> Size { get; }
    }
}
