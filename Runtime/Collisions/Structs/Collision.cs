using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public readonly struct Collision
    {
        public ContactPoint Contact { get; }

        public SoftVector3 PenetrationNormal { get; }

        public SoftFloat PenetrationDepth { get; }

        public Collision(ContactPoint contact, SoftVector3 penetrationNormal, SoftFloat penetrationDepth)
        {
            Contact = contact;
            PenetrationNormal = penetrationNormal;
            PenetrationDepth = penetrationDepth;
        }

        public Collision Merge(Collision other)
        {
            throw new NotImplementedException();
        }

        public Collision Merge(ContactPoint contactPoint)
        {
            throw new NotImplementedException();
        }
    }
}
