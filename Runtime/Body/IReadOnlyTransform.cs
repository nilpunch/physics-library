using GameLibrary.Mathematics;
using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IReadOnlyTransform<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Vector3<TNumber> Position { get; set; }
        UnitQuaternion<TNumber> Rotation { get; set; }
    }
}
