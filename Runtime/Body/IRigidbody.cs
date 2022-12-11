using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody
    {
        SoftVector3 Position { get; set; }
        SoftVector3 Velocity { get; set; }
    }
}
