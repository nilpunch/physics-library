namespace GameLibrary.Physics
{
    public interface IRigidbodyCollisionsFinder
    {
        CollisionManifold<IRigidbody>[] FindCollisions();
    }
}
