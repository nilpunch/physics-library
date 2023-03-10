namespace GameLibrary.Physics
{
    public interface IConcreteCollidersWorld<in TCollider, in TConcrete>
    {
        void Add(TCollider collider, TConcrete concrete);
        void Remove(TCollider collider);
    }
}
