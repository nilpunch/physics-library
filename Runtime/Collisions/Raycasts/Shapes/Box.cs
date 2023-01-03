using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public readonly struct Box
    {
        SoftVector3 Center { get; }
        SoftQuaternion Rotation { get; }
        SoftVector3 Size { get; }
    }
}
