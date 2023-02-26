using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public static class EpaAlgorithm
    {
        private static SoftFloat Tolerance => (SoftFloat)0.0001f;


        public static SoftVector3 CalcBarycentric2(SoftVector3 a, SoftVector3 b, SoftVector3 c, SoftVector3 normal, SoftFloat distance, bool clamp = false)
        {
            // Calculate the barycentric coordinates of the origin (0,0,0) projected
            // onto the plane of the triangle.
            //
            // [W. Heidrich, Journal of Graphics, GPU, and Game Tools,Volume 10, Issue 3, 2005.]

            SoftVector3 u, v, w, tmp;
            u = a - b;
            v = a - c;

            SoftFloat t = distance;
            tmp = SoftVector3.Cross(u, a);
            SoftFloat gamma = SoftVector3.Dot(tmp, normal) / t;
            tmp = SoftVector3.Cross(a, v);
            SoftFloat beta = SoftVector3.Dot(tmp, normal) / t;
            SoftFloat alpha = SoftFloat.One - gamma - beta;

            if (clamp)
            {
                // Clamp the projected barycentric coordinates to lie within the triangle,
                // such that the clamped coordinates are closest (euclidean) to the original point.
                //
                // [https://math.stackexchange.com/questions/
                //  1092912/find-closest-point-in-triangle-given-barycentric-coordinates-outside]

                if (alpha >= SoftFloat.Zero && beta < SoftFloat.Zero)
                {
                    t = SoftVector3.Dot(a, u);
                    if ((gamma < SoftFloat.Zero) && (t > SoftFloat.Zero))
                    {
                        beta = SoftMath.Min(SoftFloat.One, t / SoftVector3.LengthSqr(u));
                        alpha = SoftFloat.One - beta;
                        gamma = SoftFloat.Zero;
                    }
                    else
                    {
                        gamma = SoftMath.Min(SoftFloat.One,
                            SoftMath.Max(SoftFloat.Zero, SoftVector3.Dot(a, v) / SoftVector3.LengthSqr(v)));
                        alpha = SoftFloat.One - gamma;
                        beta = SoftFloat.Zero;
                    }
                }
                else if (beta >= SoftFloat.Zero && gamma < SoftFloat.Zero)
                {
                    w = b - c;
                    t = SoftVector3.Dot(b, w);
                    if ((alpha < SoftFloat.Zero) && (t > SoftFloat.Zero))
                    {
                        gamma = SoftMath.Min(SoftFloat.One, t / SoftVector3.LengthSqr(w));
                        beta = SoftFloat.One - gamma;
                        alpha = SoftFloat.Zero;
                    }
                    else
                    {
                        alpha = SoftMath.Min(SoftFloat.One,
                            SoftMath.Max(SoftFloat.Zero, -SoftVector3.Dot(b, u) / SoftVector3.LengthSqr(u)));
                        beta = SoftFloat.One - alpha;
                        gamma = SoftFloat.Zero;
                    }
                }
                else if (gamma >= SoftFloat.Zero && alpha < SoftFloat.Zero)
                {
                    w = b - c;
                    t = -SoftVector3.Dot(c, v);
                    if ((beta < SoftFloat.Zero) && (t > SoftFloat.Zero))
                    {
                        alpha = SoftMath.Min(SoftFloat.One, t / SoftVector3.LengthSqr(v));
                        gamma = SoftFloat.One - alpha;
                        beta = SoftFloat.Zero;
                    }
                    else
                    {
                        beta = SoftMath.Min(SoftFloat.One,
                            SoftMath.Max(SoftFloat.Zero, -SoftVector3.Dot(c, w) / SoftVector3.LengthSqr(w)));
                        gamma = SoftFloat.One - beta;
                        alpha = SoftFloat.Zero;
                    }
                }
            }

            return new SoftVector3(alpha, beta, gamma);
        }

        public static SoftVector3 Barycentric(SoftVector3 a, SoftVector3 b, SoftVector3 c, SoftVector3 p)
        {
            SoftVector3 v0 = b - a;
            SoftVector3 v1 = c - a;
            SoftVector3 v2 = p - a;
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

        public static Collision Calculate(List<SoftVector3> simplex, ISMCollider shapeA,
            ISMCollider shapeB, int maxIterations)
        {
            List<SoftVector3> polytope = simplex.ToList();
            List<PolytopeFace> polytopeFaces = new List<PolytopeFace>
            {
                new PolytopeFace(0, 1, 2),
                new PolytopeFace(0, 1, 3),
                new PolytopeFace(0, 2, 3),
                new PolytopeFace(1, 2, 3)
            };

            int iteration = 0;

            (int index, SoftFloat distance, SoftVector3 normal, PolytopeFace face) closestFace = default;

            FixNormals(polytope, polytopeFaces);

            while (iteration < maxIterations)
            {
                iteration += 1;

                closestFace = FindClosestFace(polytope, polytopeFaces);
                SoftVector3 supportPoint = GjkAlgorithm.MinkowskiDifference(shapeA, shapeB, closestFace.normal);
                SoftFloat distance = SoftVector3.Dot(closestFace.normal, supportPoint);

                if(SoftMath.ApproximatelyEqual(distance, closestFace.distance, Tolerance))
                {
                    break;
                }

                polytope.Add(supportPoint);
                ExpandPolytope(polytope, polytopeFaces, supportPoint);
            }

            SoftVector3 barycentric = Barycentric(
                polytope[closestFace.face.A],
                polytope[closestFace.face.B],
                polytope[closestFace.face.C],
                closestFace.normal * closestFace.distance);

            SoftVector3 supportA = shapeA.SupportPoint(polytope[closestFace.face.A]);
            SoftVector3 supportB = shapeA.SupportPoint(polytope[closestFace.face.B]);
            SoftVector3 supportC = shapeA.SupportPoint(polytope[closestFace.face.C]);

            SoftVector3 point1 = barycentric.X * supportA + barycentric.Y * supportB +
                                barycentric.Z * supportC;

            return new Collision(true, new ContactPoint[]{ new ContactPoint(point1) }, closestFace.normal, closestFace.distance + Tolerance);
        }

        public static void ExpandPolytope(List<SoftVector3> polytope, List<PolytopeFace> faces, SoftVector3 extendPoint)
        {
            var removalFacesIndices = new List<int>();

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
            var edges = new List<(int a, int b)>();
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
            foreach (int index in Enumerable.Reverse(removalFacesIndices))
            {
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
            int edgeIndex = edges.FindIndex(pair => pair.a == edge.a && pair.b == edge.b || pair.a == edge.b && pair.b == edge.a);

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
