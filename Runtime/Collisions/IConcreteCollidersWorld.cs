namespace GameLibrary.Physics
{
    public interface IConcreteCollidersWorld<TCollider, TConcrete>
    {
        void Add(TCollider collider, TConcrete concrete);
        void Remove(TCollider collider);
    }
}
