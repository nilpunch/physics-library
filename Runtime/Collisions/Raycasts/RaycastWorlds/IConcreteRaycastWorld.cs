using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public interface IConcreteRaycastWorld<TNumber, TConcrete> : IRaycastShooter<TNumber, TConcrete> where TNumber : struct, INumber<TNumber>
    {
        void Add(IRaycastCollider<TNumber> collider, TConcrete concrete);
        void Remove(IRaycastCollider<TNumber> collider);
    }
}
