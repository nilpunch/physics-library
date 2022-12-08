namespace GameLibrary.Physics
{
    public interface IPhysicalBody<out TCollider>
    {
        TCollider Collider { get; }
    }
}
