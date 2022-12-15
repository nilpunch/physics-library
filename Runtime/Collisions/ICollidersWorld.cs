namespace GameLibrary.Physics
{
    public interface ICollidersWorld<in TCollider> : ICollisionDetector
    {
        void Add(TCollider collider);
        void Remove(TCollider collider);
    }
}
