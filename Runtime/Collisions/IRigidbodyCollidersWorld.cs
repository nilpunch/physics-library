namespace GameLibrary.Physics
{
    public interface IRigidbodyCollidersWorld<in TCollider>
    {
        void Add(TCollider collider, IRigidbody rigidbody);
        void Remove(TCollider collider);
    }
}
