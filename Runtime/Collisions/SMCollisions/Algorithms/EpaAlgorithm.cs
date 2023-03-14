using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public static class EpaAlgorithm
    {
        private static SoftFloat Tolerance => (SoftFloat)0.0001f;

        public static List<SoftVector3> PolytopeShared { get; } = new List<SoftVector3>();
        public static List<SoftVector3> MinkowskiSharedA { get; } = new List<SoftVector3>();
        public static List<SoftVector3> MinkowskiSharedB { get; } = new List<SoftVector3>();
        public static List<PolytopeFace> PolytopeFacesShared { get; } = new List<PolytopeFace>();
        private static List<int> RemovalFacesIndicesShared { get; } = new List<int>();
        private static List<(int a, int b)> RemovalEdgesShared { get; } = new List<(int a, int b)>();

        public static SoftVector3 Barycentric(SoftVector3 a, SoftVector3 b, SoftVector3 c, SoftVector3 point, bool clamp = false)
        {
            SoftVector3 v0 = b - a;
            SoftVector3 v1 = c - a;
            SoftVector3 v2 = point - a;
            SoftFloat d00 = SoftVector3.Dot(v0, v0);
            SoftFloat d01 = SoftVector3.Dot(v0, v1);
            SoftFloat d11 = SoftVector3.Dot(v1, v1);
            SoftFloat d20 = SoftVector3.Dot(v2, v0);
            SoftFloat d21 = SoftVector3.Dot(v2, v1);
            SoftFloat denominator = d00 * d11 - d01 * d01;
            SoftFloat v = (d11 * d20 - d01 * d21) / denominator;
            SoftFloat w = (d00 * d21 - d01 * d20) / denominator;
            SoftFloat u = SoftFloat.One - v - w;

            return new SoftVector3(u, v, w);
        }

        private static SoftVector3 ProjectedBarycentric( SoftVector3 p, SoftVector3 q, SoftVector3 u, SoftVector3 v)
        {
            SoftVector3 n= SoftVector3.Cross( u, v );
            SoftFloat oneOver4ASquared= SoftFloat.One / SoftVector3.LengthSqr(n);
            SoftVector3 w= p - q;
            SoftFloat c = SoftVector3.Dot( SoftVector3.Cross( u, w ), n ) * oneOver4ASquared;
            SoftFloat b = SoftVector3.Dot( SoftVector3.Cross( w, v ), n ) * oneOver4ASquared;
            SoftFloat a = SoftFloat.One - b - c;

            return new SoftVector3(a, b, c);
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

        public static Collision Calculate(IReadOnlyList<MinkowskiDifference> simplex, ISMCollider shapeA,
            ISMCollider shapeB, int maxIterations)
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

            (int index, SoftFloat distance, SoftVector3 normal, PolytopeFace face) closestFace = default;

            FixNormals(polytope, polytopeFaces);

            while (iteration < maxIterations)
            {
                iteration += 1;

                closestFace = FindClosestFace(polytope, polytopeFaces);

                MinkowskiDifference supportPoint = MinkowskiDifference.Calculate(shapeA, shapeB, closestFace.normal);

                SoftFloat minkowskiDistance = SoftVector3.Dot(closestFace.normal, supportPoint.Difference);
                SoftFloat closestFaceDistance = closestFace.distance * closestFace.distance;

                if (SoftMath.ApproximatelyEqual(closestFaceDistance, minkowskiDistance, Tolerance))
                {
                    break;
                }

                polytope.Add(supportPoint.Difference);
                MinkowskiSharedA.Add(supportPoint.SupportA);
                MinkowskiSharedB.Add(supportPoint.SupportB);
                ExpandPolytope(polytope, polytopeFaces, supportPoint.Difference);
            }

            SoftVector3 barycentric = Barycentric(
                polytope[closestFace.face.A],
                polytope[closestFace.face.B],
                polytope[closestFace.face.C],
                closestFace.normal * closestFace.distance);

            SoftVector3 supportAA = MinkowskiSharedA[closestFace.face.A];
            SoftVector3 supportAB = MinkowskiSharedA[closestFace.face.B];
            SoftVector3 supportAC = MinkowskiSharedA[closestFace.face.C];
            SoftVector3 supportBA = MinkowskiSharedB[closestFace.face.A];
            SoftVector3 supportBB = MinkowskiSharedB[closestFace.face.B];
            SoftVector3 supportBC = MinkowskiSharedB[closestFace.face.C];

            SoftVector3 point1 = barycentric.X * supportAA + barycentric.Y * supportAB + barycentric.Z * supportAC;
            SoftVector3 point2 = barycentric.X * supportBA + barycentric.Y * supportBB + barycentric.Z * supportBC;

            return new Collision(new ContactPoint(point1), new ContactPoint(point2), closestFace.normal, closestFace.distance + Tolerance);
        }

        public static void ExpandPolytope(List<SoftVector3> polytope, List<PolytopeFace> faces, SoftVector3 extendPoint)
        {
            RemovalFacesIndicesShared.Clear();
            var removalFacesIndices = RemovalFacesIndicesShared;

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];

                var ab = polytope[face.B] - polytope[face.A];
                var ac = polytope[face.C] - polytope[face.A];
                var normal = SoftVector3.Normalize(SoftVector3.Cross(ab, ac));

                if (SoftVector3.Dot(polytope[face.A], normal) < SoftFloat.Zero - Tolerance)
                {
                    normal = -normal;
                }

                if (SoftVector3.Dot(normal, extendPoint - polytope[face.A]) > SoftFloat.Zero + Tolerance)
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
            SoftVector3 center = PolytopeCenter(polytope);
            foreach ((int a, int b) in edges)
            {
                var fixedFace = FixFaceNormal(new PolytopeFace(a, b, polytope.Count - 1), polytope, center);

                faces.Add(fixedFace);
            }
        }

        public static (int index, SoftFloat distance, SoftVector3 normal, PolytopeFace face) FindClosestFace(
            List<SoftVector3> polytope, List<PolytopeFace> faces)
        {
            (int index, SoftFloat distance, SoftVector3 normal, PolytopeFace face) closest = (-1, SoftFloat.MaxValue,
                default, default);

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];

                var ab = polytope[face.B] - polytope[face.A];
                var ac = polytope[face.C] - polytope[face.A];

                var normal = SoftVector3.Normalize(SoftVector3.Cross(ab, ac));
                var distance = SoftVector3.Dot(polytope[face.A], normal);

                if (distance < SoftFloat.Zero - Tolerance)
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

        public static void FixNormals(List<SoftVector3> polytope, List<PolytopeFace> faces)
        {
            SoftVector3 center = PolytopeCenter(polytope);

            for (int i = 0; i < faces.Count; i++)
            {
                faces[i] = FixFaceNormal(faces[i], polytope, center);
            }
        }

        private static PolytopeFace FixFaceNormal(PolytopeFace face, List<SoftVector3> polytope, SoftVector3 center)
        {
            var ab = polytope[face.B] - polytope[face.A];
            var ac = polytope[face.C] - polytope[face.A];

            var normal = SoftVector3.Normalize(SoftVector3.Cross(ab, ac));

            if (SoftVector3.Dot(-center, normal) < SoftFloat.Zero)
            {
                return new PolytopeFace(face.A, face.C, face.B);
            }

            return face;
        }

        private static SoftVector3 PolytopeCenter(List<SoftVector3> polytope)
        {
            SoftVector3 center = SoftVector3.Zero;
            foreach (var vertex in polytope)
                center += vertex;
            center /= (SoftFloat)polytope.Count;
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
