using System;
using PluggableMath;

namespace GameLibrary.Physics
{
    public readonly struct Collision<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public ContactPoint<TNumber> ContactFirst { get; }

        public ContactPoint<TNumber> ContactSecond { get; }

        public Vector3<TNumber> PenetrationNormal { get; }

        public Operand<TNumber> PenetrationDepth { get; }

        public Collision(ContactPoint<TNumber> contactFirst, ContactPoint<TNumber> contactSecond, Vector3<TNumber> penetrationNormal, Operand<TNumber> penetrationDepth)
        {
            ContactFirst = contactFirst;
            PenetrationNormal = penetrationNormal;
            PenetrationDepth = penetrationDepth;
            ContactSecond = contactSecond;
        }

        public Collision<TNumber> Merge(Collision<TNumber> other)
        {
            throw new NotImplementedException();
        }

        public Collision<TNumber> Merge(ContactPoint<TNumber> contactPoint)
        {
            throw new NotImplementedException();
        }
    }
}
