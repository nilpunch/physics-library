namespace GameLibrary.Physics
{
    public interface IConcreteRaycastWorld<TConcrete> : IWorld<ConcreteCollider<IRaycastCollider, TConcrete>>, IRaycastShooter<TConcrete>
    {
    }
}
