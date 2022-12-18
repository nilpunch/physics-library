using GameLibrary.Mathematics;

namespace GameLibrary.Physics.AnalyticColliders
{
    public readonly struct Box
    {
        SoftVector3 Center { get; }
        SoftQuaternion Rotation { get; }
        SoftVector3 Size { get; }
    }
}
