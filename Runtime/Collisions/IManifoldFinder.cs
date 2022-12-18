namespace GameLibrary.Physics
{
    public interface IManifoldFinder<out TBody>
    {
        ICollisionManifold<TBody>[] FindCollisions();
    }
}
