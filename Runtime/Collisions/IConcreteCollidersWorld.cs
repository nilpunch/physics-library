namespace GameLibrary.Physics
{
    public interface IConcreteCollidersWorld<TCollider, TConcrete> : ICollidersWorld<ConcreteCollider<TCollider, TConcrete>>
    {
    }
}
