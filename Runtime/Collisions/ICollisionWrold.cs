namespace GameLibrary.Physics
{
    public interface ICollisionDetector
    {
        CollisionManifold[] FindCollisions();
    }
}
