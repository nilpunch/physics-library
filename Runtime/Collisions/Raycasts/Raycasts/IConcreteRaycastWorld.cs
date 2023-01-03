namespace GameLibrary.Physics.Raycast
{
    public interface IConcreteRaycastWorld<TConcrete> : IWorld<ConcreteCollider<IRaycastCollider, TConcrete>>, IRaycastShooter<TConcrete>
    {
    }
}
