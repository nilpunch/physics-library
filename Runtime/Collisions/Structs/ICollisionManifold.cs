namespace GameLibrary.Physics
{
    public interface ICollisionManifold<out TBody>
    {
        TBody First { get; }
        TBody Second { get; }
        Collision Collision { get; }
    }
}
