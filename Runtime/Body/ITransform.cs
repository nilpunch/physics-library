using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface ITransform
    {
        SoftVector3 Position { get; set; }
        SoftUnitQuaternion Rotation { get; set; }
    }
}
