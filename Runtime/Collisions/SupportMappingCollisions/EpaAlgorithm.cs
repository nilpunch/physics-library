using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.GJK
{
    public static class EpaAlgorithm
    {
        public struct Vector4
        {
            public SoftFloat X;
            public SoftFloat Y;
            public SoftFloat Z;
            public SoftFloat W;

            public Vector4(SoftFloat x, SoftFloat y, SoftFloat z, SoftFloat w)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }
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

        public static Collision Calculate(List<SoftVector3> simplex, ISupportMappingCollider shapeA, ISupportMappingCollider shapeB)
        {
            List<SoftVector3> polytope = simplex.ToList();
            List<int> faces = new[] {0, 1, 2, 0, 3, 1, 0, 2, 3, 1, 3, 2}.ToList();

            // list: vector4(normal, distance), index: min distance
            var (normals, minFace) = GetFaceNormals(polytope, faces);

            SoftVector3 minNormal;
            SoftFloat minDistance = SoftFloat.MaxValue;

            while (minDistance == SoftFloat.MaxValue)
            {
                minNormal = normals[minFace].Normal;
                minDistance = normals[minFace].Distance;

                SoftVector3 support = GjkAlgorithm.MinkowskiDifference(shapeA, shapeB, minNormal);
                SoftFloat supportDistance = SoftVector3.Dot(minNormal, support);
                SoftVector3 supportNormalized = SoftVector3.Normalize(support);

                if (SoftMath.Abs(supportDistance - minDistance) > (SoftFloat)0.001f)
                {
                    minDistance = SoftFloat.MaxValue;
                    List<(int, int)> uniqueEdges = new List<(int, int)>();

                    for (int i = 0; i < normals.Count; i++)
                    {
                        if (SoftVector3.ApproximatelyEqual(normals[i].Normal, supportNormalized, (SoftFloat)0.001f))
                        {
                            int f = i * 3;

                            AddIfUniqueEdge(uniqueEdges, faces, f, f + 1);
                            AddIfUniqueEdge(uniqueEdges, faces, f + 1, f + 2);
                            AddIfUniqueEdge(uniqueEdges, faces, f + 2, f);

                            faces[f + 2] = faces[^1];
                            faces.RemoveAt(faces.Count - 1);
                            faces[f + 1] = faces[^1];
                            faces.RemoveAt(faces.Count - 1);
                            faces[f] = faces[^1];
                            faces.RemoveAt(faces.Count - 1);

                            normals[i] = normals[^1];
                            normals.RemoveAt(normals.Count - 1);

                            i--;
                        }
                    }

                    List<int> newFaces = new List<int>();
                    foreach (var (edgeIndex1, edgeIndex2) in uniqueEdges)
                    {
                        newFaces.Add(edgeIndex1);
                        newFaces.Add(edgeIndex2);
                        newFaces.Add(polytope.Count);
                    }

                    polytope.Add(support);

                    var (newNormals, newMinFace) = GetFaceNormals(polytope, newFaces);
                    SoftFloat oldMinDistance = SoftFloat.MaxValue;
                    for (int i = 0; i < normals.Count; i++)
                    {
                        if (normals[i].Distance < oldMinDistance)
                        {
                            oldMinDistance = normals[i].Distance;
                            minFace = i;
                        }
                    }

                    if (newNormals[newMinFace].Distance < oldMinDistance)
                    {
                        minFace = newMinFace + normals.Count;
                    }

                    faces.AddRange(newFaces);
                    normals.AddRange(newNormals);
                }
            }

            Collision collision = new Collision(); //new Collision(true, minNormal);

            //collision.Normal = minNormal;
            //collision.PenetrationDepth = minDistance + 0.001f;
            //collision.HasCollision = true;

            return collision;
        }

        private static (List<(SoftVector3 Normal, SoftFloat Distance)>, int) GetFaceNormals(List<SoftVector3> polytope,
            List<int> faces)
        {
            List<(SoftVector3 Normal, SoftFloat Distance)> normals =
                new List<(SoftVector3 Normal, SoftFloat Distance)>();
            int minTriangle = 0;
            SoftFloat minDistance = SoftFloat.MaxValue;

            for (int i = 0; i < faces.Count; i += 3)
            {
                SoftVector3 a = polytope[faces[i]];
                SoftVector3 b = polytope[faces[i + 1]];
                SoftVector3 c = polytope[faces[i + 2]];

                SoftVector3 normal = SoftVector3.Normalize(SoftVector3.Cross(b - a, c - a));
                SoftFloat distance = SoftVector3.Dot(normal, a);

                if (distance < SoftFloat.Zero)
                {
                    normal *= SoftFloat.MinusOne;
                    distance *= SoftFloat.MinusOne;
                }

                normals.Add((normal, distance));

                if (distance < minDistance)
                {
                    minTriangle = i / 3;
                    minDistance = distance;
                }
            }

            return (normals, minTriangle);
        }

        private static void AddIfUniqueEdge(List<(int, int)> edges, List<int> faces, int a, int b)
        {
            int reverseEdgeIndex = edges.FindIndex(pair => pair.Item1 == faces[b] && pair.Item2 == faces[a]);

            if (reverseEdgeIndex != -1)
            {
                edges.RemoveAt(reverseEdgeIndex);
            }
            else
            {
                edges.Add((faces[a], faces[b]));
            }
        }
    }
}
