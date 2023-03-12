using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public readonly struct Collision
    {
        public ContactPoint ContactFirst { get; }

        public ContactPoint ContactSecond { get; }

        public SoftVector3 PenetrationNormal { get; }

        public SoftFloat PenetrationDepth { get; }

        public Collision(ContactPoint contactFirst, ContactPoint contactSecond, SoftVector3 penetrationNormal, SoftFloat penetrationDepth)
        {
            ContactFirst = contactFirst;
            PenetrationNormal = penetrationNormal;
            PenetrationDepth = penetrationDepth;
            ContactSecond = contactSecond;
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
