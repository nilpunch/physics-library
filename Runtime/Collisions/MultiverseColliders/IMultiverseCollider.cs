using GameLibrary.Physics.Raycast;
using GameLibrary.Physics.SupportMapping;
using PluggableMath;

namespace GameLibrary.Physics
{
    public interface IMultiverseCollider<TNumber> : ISMCollider<TNumber>, IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
    }
}
