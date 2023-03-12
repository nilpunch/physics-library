using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IReadOnlyTransform
    {
        SoftVector3 Position { get; set; }
        SoftUnitQuaternion Rotation { get; set; }
    }
}
