using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public static class EpaAlgorithm
    {
        // public static SoftVector3 CalcBarycentric2(SoftVector3 a, SoftVector3 b, SoftVector3 c, bool clamp = false)
        // {
        //     // Calculate the barycentric coordinates of the origin (0,0,0) projected
        //     // onto the plane of the triangle.
        //     //
        //     // [W. Heidrich, Journal of Graphics, GPU, and Game Tools,Volume 10, Issue 3, 2005.]
        //
        //     SoftVector3 u, v, w, tmp;
        //     u = a - b;
        //     v = a - c;
        //
        //     SoftFloat t = tri.NormalSq;
        //     tmp = SoftVector3.Cross(u, a);
        //     SoftFloat gamma = SoftVector3.Dot(tmp, tri.Normal) / t;
        //     tmp = SoftVector3.Cross(a, v);
        //     SoftFloat beta = SoftVector3.Dot(tmp, tri.Normal) / t;
        //     SoftFloat alpha = SoftFloat.One - gamma - beta;
        //
        //     if (clamp)
        //     {
        //         // Clamp the projected barycentric coordinates to lie within the triangle,
        //         // such that the clamped coordinates are closest (euclidean) to the original point.
        //         //
        //         // [https://math.stackexchange.com/questions/
        //         //  1092912/find-closest-point-in-triangle-given-barycentric-coordinates-outside]
        //
        //         if (alpha >= SoftFloat.Zero && beta < SoftFloat.Zero)
        //         {
        //             t = SoftVector3.Dot(a, u);
        //             if ((gamma < SoftFloat.Zero) && (t > SoftFloat.Zero))
        //             {
        //                 beta = SoftMath.Min(SoftFloat.One, t / SoftVector3.LengthSqr(u));
        //                 alpha = SoftFloat.One - beta;
        //                 gamma = SoftFloat.Zero;
        //             }
        //             else
        //             {
        //                 gamma = SoftMath.Min(SoftFloat.One,
        //                     SoftMath.Max(SoftFloat.Zero, SoftVector3.Dot(a, v) / SoftVector3.LengthSqr(v)));
        //                 alpha = SoftFloat.One - gamma;
        //                 beta = SoftFloat.Zero;
        //             }
        //         }
        //         else if (beta >= SoftFloat.Zero && gamma < SoftFloat.Zero)
        //         {
        //             w = b - c;
        //             t = SoftVector3.Dot(b, w);
        //             if ((alpha < SoftFloat.Zero) && (t > SoftFloat.Zero))
        //             {
        //                 gamma = SoftMath.Min(SoftFloat.One, t / SoftVector3.LengthSqr(w));
        //                 beta = SoftFloat.One - gamma;
        //                 alpha = SoftFloat.Zero;
        //             }
        //             else
        //             {
        //                 alpha = SoftMath.Min(SoftFloat.One,
        //                     SoftMath.Max(SoftFloat.Zero, -SoftVector3.Dot(b, u) / SoftVector3.LengthSqr(u)));
        //                 beta = SoftFloat.One - alpha;
        //                 gamma = SoftFloat.Zero;
        //             }
        //         }
        //         else if (gamma >= SoftFloat.Zero && alpha < SoftFloat.Zero)
        //         {
        //             w = b - c;
        //             t = -SoftVector3.Dot(c, v);
        //             if ((beta < SoftFloat.Zero) && (t > SoftFloat.Zero))
        //             {
        //                 alpha = SoftMath.Min(SoftFloat.One, t / SoftVector3.LengthSqr(v));
        //                 gamma = SoftFloat.One - alpha;
        //                 beta = SoftFloat.Zero;
        //             }
        //             else
        //             {
        //                 beta = SoftMath.Min(SoftFloat.One,
        //                     SoftMath.Max(SoftFloat.Zero, -SoftVector3.Dot(c, w) / SoftVector3.LengthSqr(w)));
        //                 gamma = SoftFloat.One - beta;
        //                 alpha = SoftFloat.Zero;
        //             }
        //         }
        //     }
        //
        //     return new SoftVector3(alpha, beta, gamma);
        // }

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

        public static (Collision collision, List<SoftVector3> polytope, List<PolytopeFace> polytopeFaces) Calculate(List<SoftVector3> simplex, ISupportMappingCollider shapeA,
            ISupportMappingCollider shapeB, int maxIterations, out List<SoftVector3> specialPoints)
        {
            List<SoftVector3> polytope = simplex.ToList();
            List<PolytopeFace> polytopeFaces = new List<PolytopeFace>
            {
                new PolytopeFace(0, 1, 2),
                new PolytopeFace(0, 1, 3),
                new PolytopeFace(0, 2, 3),
                new PolytopeFace(1, 2, 3)
            };

            SoftVector3 polytopeCentre = shapeB.Centre - shapeA.Centre;

            int iteration = 0;

            SoftVector3 closestNormal = default;
            SoftFloat closestDistance = default;

            specialPoints = new List<SoftVector3>();

            while (iteration < maxIterations)
            {
                iteration += 1;

                var closestFace = FindClosestFace(polytope, polytopeFaces, polytopeCentre);
                closestNormal = closestFace.normal;
                closestDistance = closestFace.distance;
                SoftVector3 supportPoint = GjkAlgorithm.MinkowskiDifference(shapeA, shapeB, closestFace.normal);
                SoftFloat distance = SoftVector3.Dot(closestFace.normal, supportPoint);

                if(SoftMath.ApproximatelyEqual(distance, closestFace.distance, (SoftFloat)0.00001f))
                {
                    return (new Collision(true, Array.Empty<ContactPoint>(), closestFace.normal, closestFace.distance + (SoftFloat)0.00001), polytope, polytopeFaces);
                }

                polytope.Add(supportPoint);
                ExpandPolytope(polytope, polytopeFaces, supportPoint, out specialPoints);
            }

            // SoftVector3 bc = Barycentric(closestTriangle, !_originEnclosed);
            //
            // point1 = bc.X * _verticesA[closestTriangle.A] + bc.Y * _verticesA[closestTriangle.B] +
            //          bc.Z * _verticesA[closestTriangle.C];
            // point2 = bc.X * _verticesB[closestTriangle.A] + bc.Y * _verticesB[closestTriangle.B] +
            //          bc.Z * _verticesB[closestTriangle.C];
            //
            // normal = closestTriangle.Normal * (SoftFloat.One / SoftMath.Sqrt(closestTriangle.NormalSq));
            return (new Collision(true, Array.Empty<ContactPoint>(), closestNormal, closestDistance), polytope, polytopeFaces);
        }

        public static void ExpandPolytope(List<SoftVector3> polytope, List<PolytopeFace> faces,
            SoftVector3 extendPoint, out List<SoftVector3> specialPoints)
        {
            var removalFacesIndices = new List<int>();

            for (int i = 0; i < faces.Count; i++)
            {
                var face = faces[i];

                var ab = polytope[face.B] - polytope[face.A];
                var ac = polytope[face.C] - polytope[face.A];
                var normal = SoftVector3.Normalize(SoftVector3.Cross(ab, ac));

                if (SoftVector3.Dot(polytope[face.A], normal) < SoftFloat.Zero)
                {
                    normal = -normal;
                }

                if (SoftVector3.Dot(normal, extendPoint - polytope[face.A]) > SoftFloat.Zero)
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

                // edges.Add(edgeAB);
                // edges.Add(edgeAC);
                // edges.Add(edgeBC);

                AddOrDeleteEdge(edges, edgeAB);
                AddOrDeleteEdge(edges, edgeAC);
                AddOrDeleteEdge(edges, edgeBC);
            }

            //remove the faces from the polytope
            specialPoints = new List<SoftVector3>();
            specialPoints.Clear();
            foreach (int index in Enumerable.Reverse(removalFacesIndices))
            {
                specialPoints.Add(polytope[faces[index].A]);
                specialPoints.Add(polytope[faces[index].B]);
                specialPoints.Add(polytope[faces[index].C]);

                faces.RemoveAt(index);
            }

            //form new faces with the edges and new point
            foreach ((int a, int b) in edges)
            {
                faces.Add(new PolytopeFace(a, b, polytope.Count - 1));
            }
        }

        public static (int index, SoftFloat distance, SoftVector3 normal, PolytopeFace face) FindClosestFace(
            List<SoftVector3> polytope, List<PolytopeFace> faces, SoftVector3 polytopeCentre)
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

                if (distance < SoftFloat.Zero)
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

        public static void AddOrDeleteEdge(List<(int a, int b)> edges, (int a, int b) edge)
        {
            int edgeIndex = edges.FindIndex(pair => pair.a == edge.b && pair.b == edge.a);

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
