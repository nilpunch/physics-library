namespace GameLibrary.Physics
{
    public interface ICollisionDetector<TCollider>
    {
        CollisionManifold<TCollider>[] FindManifolds(TCollider[] colliders);
    }
}
