using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public static class AnalyticCollisionsLibrary<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public static Collision<TNumber> BoxAgainstBox(Box<TNumber> first, Box<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> SphereAgainstBox(Sphere<TNumber> first, Box<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> SphereAgainstSphere(Sphere<TNumber> first, Sphere<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> ConvexAgainstBox(ConvexHull<TNumber> first, Box<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> ConvexAgainstSphere(ConvexHull<TNumber> first, Sphere<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> ConvexAgainstConvex(ConvexHull<TNumber> first, ConvexHull<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> AABBAgainstBox(AABB<TNumber> first, Box<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> AABBAgainstSphere(AABB<TNumber> first, Sphere<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> AABBAgainstConvexHull(AABB<TNumber> first, ConvexHull<TNumber> second)
        {
            throw new System.NotImplementedException();
        }

        public static Collision<TNumber> AABBAgainstAABB(AABB<TNumber> first, AABB<TNumber> second)
        {
            throw new System.NotImplementedException();
        }
    }
}
