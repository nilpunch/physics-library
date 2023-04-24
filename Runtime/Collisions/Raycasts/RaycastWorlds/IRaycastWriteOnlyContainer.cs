using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public interface IRaycastWriteOnlyContainer<TNumber> : IWriteOnlyContainer<IRaycastCollider<TNumber>>, IRaycastShooter<TNumber, IRaycastCollider<TNumber>> where TNumber : struct, INumber<TNumber>
    {
    }
}
