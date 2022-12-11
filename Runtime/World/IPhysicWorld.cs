namespace GameLibrary.Physics
{
    public interface IPhysicWorld<TBody, in TCollider> : ICollisionsWorld<TBody>
    {
        void Add(TBody body, TCollider collider);
        void Remove(TBody body);
    }
}
