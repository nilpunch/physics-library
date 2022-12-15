using System;

namespace GameLibrary.Physics
{
    public struct CollisionManifold
    {
        public CollisionManifold(Guid first, Guid second, Collision collision)
        {
            First = first;
            Second = second;
            Collision = collision;
        }

        public Guid First { get; }
        public Guid Second { get; }
        public Collision Collision { get; }
    }
}
