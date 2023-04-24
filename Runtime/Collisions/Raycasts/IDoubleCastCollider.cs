using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public interface IDoubleCastCollider<TNumber> : IRaycastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> collider);
    }
}
