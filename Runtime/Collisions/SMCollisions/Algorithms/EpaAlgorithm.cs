using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public static class EpaAlgorithm<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private static Operand<TNumber> Tolerance => (Operand<TNumber>)0.0001f;

        public static List<Vector3<TNumber>> PolytopeShared { get; } = new List<Vector3<TNumber>>();
        public static List<Vector3<TNumber>> MinkowskiSharedA { get; } = new List<Vector3<TNumber>>();
        public static List<Vector3<TNumber>> MinkowskiSharedB { get; } = new List<Vector3<TNumber>>();
        public static List<PolytopeFace> PolytopeFacesShared { get; } = new List<PolytopeFace>();
        private static List<int> RemovalFacesIndicesShared { get; } = new List<int>();
        private static List<(int a, int b)> RemovalEdgesShared { get; } = new List<(int a, int b)>();

        public static Vector3<TNumber> Barycentric(Vector3<TNumber> a, Vector3<TNumber> b, Vector3<TNumber> c, Vector3<TNumber> point, bool clamp = false)
        {
            Vector3<TNumber> v0 = b - a;
            Vector3<TNumber> v1 = c - a;
            Vector3<TNumber> v2 = point - a;
            Operand<TNumber> d00 = Vector3<TNumber>.Dot(v0, v0);
            Operand<TNumber> d01 = Vector3<TNumber>.Dot(v0, v1);
            Operand<TNumber> d11 = Vector3<TNumber>.Dot(v1, v1);
            Operand<TNumber> d20 = Vector3<TNumber>.Dot(v2, v0);
            Operand<TNumber> d21 = Vector3<TNumber>.Dot(v2, v1);
            Operand<TNumber> denominator = d00 * d11 - d01 * d01;
            Operand<TNumber> v = (d11 * d20 - d01 * d21) / denominator;
            Operand<TNumber> w = (d00 * d21 - d01 * d20) / denominator;
            Operand<TNumber> u = Operand<TNumber>.One - v - w;

            return new Vector3<TNumber>(u, v, w);
        }

        private static Vector3<TNumber> ProjectedBarycentric( Vector3<TNumber> p, Vector3<TNumber> q, Vector3<TNumber> u, Vector3<TNumber> v)
        {
            Vector3<TNumber> n= Vector3<TNumber>.Cross( u, v );
            Operand<TNumber> oneOver4ASquared= Operand<TNumber>.One / Vector3<TNumber>.LengthSqr(n);
            Vector3<TNumber> w= p - q;
            Operand<TNumber> c = Vector3<TNumber>.Dot( Vector3<TNumber>.Cross( u, w ), n ) * oneOver4ASquared;
            Operand<TNumber> b = Vector3<TNumber>.Dot( Vector3<TNumber>.Cross( w, v ), n ) * oneOver4ASquared;
            Operand<TNumber> a = Operand<TNumber>.One - b - c;

            return new Vector3<TNumber>(a, b, c);
        }


        public struct PolytopeFace
        {
            public PolytopeFace(int a, int b, int c)
            {
                A = a;
                B = b;
                C = c;
            }

            public int A { get; }
            public int B { get; }
            public int C { get; }
        }

        public static Collision<TNumber> Calculate(IReadOnlyList<MinkowskiDifference<TNumber>> simplex, ISMCollider<TNumber> shapeA,
            ISMCollider<TNumber> shapeB, int maxIterations)
        {
            PolytopeShared.Clear();
            PolytopeFacesShared.Clear();
            MinkowskiSharedA.Clear();
            MinkowskiSharedB.Clear();

            var polytope = PolytopeShared;
            foreach (var minkowskiDifference in simplex)
            {
                polytope.Add(minkowskiDifference.Difference);
                MinkowskiSharedA.Add(minkowskiDifference.SupportA);
                MinkowskiSharedB.Add(minkowskiDifference.SupportB);
            }

            List<PolytopeFace> polytopeFaces = PolytopeFacesShared;
            polytopeFaces.Add(new PolytopeFace(0, 1, 2));
            polytopeFaces.Add(new PolytopeFace(0, 1, 3));
            polytopeFaces.Add(new PolytopeFace(0, 2, 3));
            polytopeFaces.Add(new PolytopeFace(1, 2, 3));

            int iteration = 0;

            (int index, Operand<TNumber> distance, Vector3<TNumber> normal, PolytopeFace face) closestFace = default;

            FixNormals(polytope, polytopeFaces);

            while (iteration < maxIterations)
            {
                iteration += 1;

                closestFace = FindClosestFace(polytope, polytopeFaces);

                MinkowskiDifference<TNumber> supportPoint = MinkowskiDifference<TNumber>.Calculate(shapeA, shapeB, closestFace.normal);

                Operand<TNumber> minkowskiDistance = Vector3<TNumber>.Dot(closestFace.normal, supportPoint.Difference);
                Operand<TNumber> closestFaceDistance = closestFace.distance * closestFace.distance;

                if (Math<TNumber>.ApproximatelyEqual(closestFaceDistance, minkowskiDistance, Tolerance))
                {
                    break;
                }

                polytope.Add(supportPoint.Difference);
                MinkowskiSharedA.Add(supportPoint.SupportA);
                MinkowskiSharedB.Add(supportPoint.SupportB);
                ExpandPolytope(polytope, polytopeFaces, supportPoint.Difference);
            }

            Vector3<TNumber> barycentric = Barycentric(
                polytope[closestFace.face.A],
                polytope[closestFace.face.B],
                polytope[closestFace.face.C],
                closestFace.normal * closestFace.distance);

            Vector3<TNumber> supportAA = MinkowskiSharedA[closestFace.face.A];
            Vector3<TNumber> supportAB = MinkowskiSharedA[closestFace.face.B];
            Vector3<TNumber> supportAC = MinkowskiSharedA[closestFace.face.C];
            Vector3<TNumber> supportBA = MinkowskiSharedB[closestFace.face.A];
            Vector3<TNumber> supportBB = MinkowskiSharedB[closestFace.face.B];
            Vector3<TNumber> supportBC = MinkowskiSharedB[closestFace.face.C];

            Vector3<TNumber> point1 = barycentric.X * supportAA + barycentric.Y * supportAB + barycentric.Z * supportAC;
            Vector3<TNumber> point2 = barycentric.X * supportBA + barycentric.Y * supportBB + barycentric.Z * supportBC;

            return new Collision<TNumber>(new ContactPoint<TNumber>(point1), new ContactPoint<TNumber>(point2), closestFace.normal, closestFace.distance + Tolerance);
        }

        public static void ExpandPolytope(List<Vector3<TNumber>> polytope, List<PolytopeFace> faces, Vector3<TNumber> extendPoint)
        {
            RemovalFacesIndicesShared.Clear();
            var removalFacesIndices = RemovalFacesIndicesShared;

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];

                var ab = polytope[face.B] - polytope[face.A];
                var ac = polytope[face.C] - polytope[face.A];
                var normal = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(ab, ac));

                if (Vector3<TNumber>.Dot(polytope[face.A], normal) < Operand<TNumber>.Zero - Tolerance)
                {
                    normal = -normal;
                }

                if (Vector3<TNumber>.Dot(normal, extendPoint - polytope[face.A]) > Operand<TNumber>.Zero + Tolerance)
                {
                    removalFacesIndices.Add(i);
                }
            }

            // get the edges that are not shared between the faces that should be removed
            RemovalEdgesShared.Clear();
            var edges = RemovalEdgesShared;
            foreach (int removalFaceIndex in removalFacesIndices)
            {
                var face = faces[removalFaceIndex];
                (int a, int b) edgeAB = (face.A, face.B);
                (int a, int b) edgeAC = (face.A, face.C);
                (int a, int b) edgeBC = (face.B, face.C);

                AddOrDeleteEdge(edges, edgeAB);
                AddOrDeleteEdge(edges, edgeAC);
                AddOrDeleteEdge(edges, edgeBC);
            }

            //remove the faces from the polytope
            for (int i = removalFacesIndices.Count - 1; i >= 0; i--)
            {
                int index = removalFacesIndices[i];
                faces.RemoveAt(index);
            }

            //form new faces with the edges and new point
            Vector3<TNumber> center = PolytopeCenter(polytope);
            foreach ((int a, int b) in edges)
            {
                var fixedFace = FixFaceNormal(new PolytopeFace(a, b, polytope.Count - 1), polytope, center);

                faces.Add(fixedFace);
            }
        }

        public static (int index, Operand<TNumber> distance, Vector3<TNumber> normal, PolytopeFace face) FindClosestFace(
            List<Vector3<TNumber>> polytope, List<PolytopeFace> faces)
        {
            (int index, Operand<TNumber> distance, Vector3<TNumber> normal, PolytopeFace face) closest = (-1, Operand<TNumber>.MaxValue,
                default, default);

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];

                var ab = polytope[face.B] - polytope[face.A];
                var ac = polytope[face.C] - polytope[face.A];

                var normal = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(ab, ac));
                var distance = Vector3<TNumber>.Dot(polytope[face.A], normal);

                if (distance < Operand<TNumber>.Zero - Tolerance)
                {
                    normal = -normal;
                    distance = -distance;
                }

                if (distance < closest.distance)
                {
                    closest = (i, distance, normal, face);
                }
            }

            return closest;
        }

        public static void FixNormals(List<Vector3<TNumber>> polytope, List<PolytopeFace> faces)
        {
            Vector3<TNumber> center = PolytopeCenter(polytope);

            for (int i = 0; i < faces.Count; i++)
            {
                faces[i] = FixFaceNormal(faces[i], polytope, center);
            }
        }

        private static PolytopeFace FixFaceNormal(PolytopeFace face, List<Vector3<TNumber>> polytope, Vector3<TNumber> center)
        {
            var ab = polytope[face.B] - polytope[face.A];
            var ac = polytope[face.C] - polytope[face.A];

            var normal = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(ab, ac));

            if (Vector3<TNumber>.Dot(-center, normal) < Operand<TNumber>.Zero)
            {
                return new PolytopeFace(face.A, face.C, face.B);
            }

            return face;
        }

        private static Vector3<TNumber> PolytopeCenter(List<Vector3<TNumber>> polytope)
        {
            Vector3<TNumber> center = Vector3<TNumber>.Zero;
            foreach (var vertex in polytope)
                center += vertex;
            center /= (Operand<TNumber>)polytope.Count;
            return center;
        }

        public static void AddOrDeleteEdge(List<(int a, int b)> edges, (int a, int b) edge)
        {
            int edgeIndex = -1;

            for (int index = 0; index < edges.Count; index++)
            {
                (int a, int b) pair = edges[index];

                if (pair.a == edge.a && pair.b == edge.b || pair.a == edge.b && pair.b == edge.a)
                {
                    edgeIndex = index;
                    break;
                }
            }

            if (edgeIndex != -1)
            {
                edges.RemoveAt(edgeIndex);
            }
            else
            {
                edges.Add(edge);
            }
        }
    }
}
