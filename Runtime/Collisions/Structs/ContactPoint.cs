using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public struct ContactPoint
    {
        public ContactPoint(SoftVector3 position)
        {
            Position = position;
        }

        public SoftVector3 Position { get; }
    }
}
