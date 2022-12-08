namespace GameLibrary.Physics
{
    public interface ICollisionCollector<TCollider>
    {
        CollisionManifold<TCollider>[] CollectManifolds(TCollider[] colliders);
    }
}
