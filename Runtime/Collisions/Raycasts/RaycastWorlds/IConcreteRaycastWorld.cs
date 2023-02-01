namespace GameLibrary.Physics.Raycast
{
    public interface IConcreteRaycastWorld<TConcrete> : IRaycastShooter<TConcrete>
    {
        void Add(IRaycastCollider collider, TConcrete concrete);
        void Remove(IRaycastCollider collider);
    }
}
