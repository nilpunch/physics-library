namespace GameLibrary.Physics
{
    public interface IPhysicWorld<TBody> : ICollisionsWorld<TBody>
    {
        void Add(TBody body);
        void Remove(TBody body);
    }
}
