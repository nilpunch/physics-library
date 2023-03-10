namespace GameLibrary.Physics
{
    public interface ICollisions<TBody>
    {
        void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TBody>> output);
    }
}
