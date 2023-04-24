using PluggableMath;

namespace GameLibrary.Physics
{
    public readonly struct AABB<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public Vector3<TNumber> Center { get; }
        public Vector3<TNumber> Extents { get; }

        public AABB(Vector3<TNumber> center, Vector3<TNumber> extents)
        {
            Center = center;
            Extents = extents;
        }

        public Vector3<TNumber> Size { get { return Extents * (Operand<TNumber>)2; } }
        public Vector3<TNumber> Min { get { return Center - Extents; } }
        public Vector3<TNumber> Max { get { return Center + Extents; } }

        /// <summary>Returns a string representation of the AABB<TNumber>.</summary>
        public override string ToString()
        {
            return $"AABB<TNumber>(Center:{Center}, Extents:{Extents}";
        }

        public bool Contains(Vector3<TNumber> point)
        {
            if (point.X < Center.X - Extents.X)
                return false;
            if (point.X > Center.X + Extents.X)
                return false;

            if (point.Y < Center.Y - Extents.Y)
                return false;
            if (point.Y > Center.Y + Extents.Y)
                return false;

            if (point.Z < Center.Z - Extents.Z)
                return false;
            if (point.Z > Center.Z + Extents.Z)
                return false;

            return true;
        }

        public bool Contains(AABB<TNumber> b)
        {
            return Contains(b.Center + new Vector3<TNumber>(-b.Extents.X, -b.Extents.Y, -b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(-b.Extents.X, -b.Extents.Y,  b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(-b.Extents.X,  b.Extents.Y, -b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(-b.Extents.X,  b.Extents.Y,  b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(b.Extents.X, -b.Extents.Y, -b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(b.Extents.X, -b.Extents.Y,  b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(b.Extents.X,  b.Extents.Y, -b.Extents.Z))
                && Contains(b.Center + new Vector3<TNumber>(b.Extents.X,  b.Extents.Y,  b.Extents.Z));
        }

        public Operand<TNumber> DistanceSqr(Vector3<TNumber> point)
        {
            return Vector3<TNumber>.LengthSqr(Vector3<TNumber>.MaxComponents(Vector3<TNumber>.AbsComponents(point - Center), Extents) - Extents);
        }

        // public static AABB<TNumber> Transform(Float4X4 transform, AABB<TNumber> localBounds)
        // {
        //     Float3 center = UnityMath.Transform(transform, localBounds.Center);
        //     Float3 extents = RotateExtents(localBounds.Extents, transform.c0.Xyz, transform.c1.Xyz, transform.c2.Xyz);
        //     return new AABB<TNumber>(center, extents);
        // }

        private static Vector3<TNumber> RotateExtents(Vector3<TNumber> extents, Vector3<TNumber> m0, Vector3<TNumber> m1, Vector3<TNumber> m2)
        {
            return Vector3<TNumber>.AbsComponents(m0 * extents.X) + Vector3<TNumber>.AbsComponents(m1 * extents.Y) + Vector3<TNumber>.AbsComponents(m2 * extents.Z);
        }
    }
}
