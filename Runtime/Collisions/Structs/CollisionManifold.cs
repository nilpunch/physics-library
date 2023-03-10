using System;

namespace GameLibrary.Physics
{
    public readonly struct CollisionManifold<TBody>
    {
        public CollisionManifold(TBody first, TBody second, Collision collision)
        {
            First = first;
            Second = second;
            Collision = collision;
        }

        public TBody First { get; }
        public TBody Second { get; }
        public Collision Collision { get; }
    }
}
