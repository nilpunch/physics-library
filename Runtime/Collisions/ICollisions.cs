namespace GameLibrary.Physics
{
    public interface ICollisions<out TBody>
    {
        ICollisionManifold<TBody>[] FindCollisions();
    }
}
