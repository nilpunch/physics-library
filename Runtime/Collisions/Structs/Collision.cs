using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public readonly struct Collision
    {
        public bool Occure { get; }

        public ContactPoint[] Contacts { get; }

        public SoftVector3 PenetrationNormal { get; }

        public SoftFloat PenetrationDepth { get; }

        public Collision(bool occure, ContactPoint[] contacts, SoftVector3 penetrationNormal, SoftFloat penetrationDepth)
        {
            Occure = occure;
            Contacts = contacts;
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
