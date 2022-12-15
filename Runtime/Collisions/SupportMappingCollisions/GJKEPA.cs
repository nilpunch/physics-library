using System;
using GameLibrary.Mathematics;
using GameLibrary.Physics.SupportMapping;

namespace GameLibrary.Physics.SupportPoint
{
    public sealed class Gjkepa
    {
        private const int MaxIter = 10;

        public struct Statistics
        {
            public SoftFloat Accuracy;
            public int Iterations;
        }

        public struct MinkowskiDifference
        {
            public ISupportMappingCollider SupportA, SupportB;

            public void Support(in SoftVector3 direction, out SoftVector3 vA, out SoftVector3 vB, out SoftVector3 v)
            {
                vA = SupportA.SupportPoint(-direction);
                vB = SupportB.SupportPoint(direction);
                v = vA - vB;
            }

            public void SupportCenter(out SoftVector3 center)
            {
                center = SupportA.Centre - SupportB.Centre;
            }
        }

        public class GjkepaSolver
        {
            public struct Triangle
            {
                public short A, B, C;
                public bool FacingOrigin;

                public short this[int i]
                {
                    get
                    {
                        switch (i)
                        {
                            case 0:
                                return A;
                            case 1:
                                return B;
                            default:
                                return C;
                        }
                    }
                }

                public SoftVector3 Normal;
                public SoftVector3 ClosestToOrigin;

                public SoftFloat NormalSq;
                public SoftFloat ClosestToOriginSq;
            }

            public struct Edge
            {
                public short A;
                public short B;

                public Edge(short a, short b)
                {
                    this.A = a;
                    this.B = b;
                }

                public static bool Equals(in Edge a, in Edge b)
                {
                    return ((a.A == b.A && a.B == b.B) || (a.A == b.B && a.B == b.A));
                }
            }

            public Statistics Statistics;
            public MinkowskiDifference Mkd;

            private const int MaxVertices = MaxIter + 4;
            private const int MaxTriangles = 3 * MaxVertices;

            private readonly Triangle[] _triangles = new Triangle[MaxTriangles];
            private readonly SoftVector3[] _vertices = new SoftVector3[MaxVertices];
            private readonly SoftVector3[] _verticesA = new SoftVector3[MaxVertices];
            private readonly SoftVector3[] _verticesB = new SoftVector3[MaxVertices];

            private readonly Edge[] _edges = new Edge[256];

            private short _tPointer = 0;
            private short _vPointer = 0;

            private bool _originEnclosed = false;
            private SoftVector3 _center;

            public SoftVector3 CalcBarycentric(in Triangle tri, bool clamp = false)
            {
                SoftVector3 a = _vertices[tri.A];
                SoftVector3 b = _vertices[tri.B];
                SoftVector3 c = _vertices[tri.C];

                // Calculate the barycentric coordinates of the origin (0,0,0) projected
                // onto the plane of the triangle.
                //
                // [W. Heidrich, Journal of Graphics, GPU, and Game Tools,Volume 10, Issue 3, 2005.]

                SoftVector3 u = a - b;
                SoftVector3 v = a - c;

                SoftFloat t = tri.NormalSq;
                SoftVector3 tmp = SoftVector3.Cross(u, a);
                SoftFloat gamma = SoftVector3.Dot(tmp, tri.Normal) / t;
                tmp = SoftVector3.Cross(a, v);
                SoftFloat beta = SoftVector3.Dot(tmp, tri.Normal) / t;
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
                    else
                    {
                        SoftVector3 w;
                        if (beta >= SoftFloat.Zero && gamma < SoftFloat.Zero)
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
                }

                return new SoftVector3(alpha, beta, gamma);
            }

            private SoftFloat _scale = (SoftFloat)1e-4;

            private SoftVector3 V0 =>
                _scale * new SoftVector3(SoftMath.Sqrt((SoftFloat)8.0 / (SoftFloat)9.0), SoftFloat.Zero,
                    -SoftFloat.One / (SoftFloat)3.0);

            private SoftVector3 V1 =>
                _scale * new SoftVector3(-SoftMath.Sqrt((SoftFloat)2.0 / (SoftFloat)9.0),
                    SoftMath.Sqrt((SoftFloat)2.0 / (SoftFloat)3.0), -SoftFloat.One / (SoftFloat)3.0);

            private SoftVector3 V2 =>
                _scale * new SoftVector3(-SoftMath.Sqrt((SoftFloat)2.0 / (SoftFloat)9.0),
                    -SoftMath.Sqrt((SoftFloat)2.0 / (SoftFloat)3.0), -SoftFloat.One / (SoftFloat)3.0);

            private SoftVector3 V3 => _scale * new SoftVector3(SoftFloat.Zero, SoftFloat.Zero, SoftFloat.One);

            private void ConstructInitialTetrahedron(in SoftVector3 position)
            {
                _vPointer = 3;

                _vertices[0] = V0 + position;
                _vertices[1] = V1 + position;
                _vertices[2] = V2 + position;
                _vertices[3] = V3 + position;

                CreateTriangle(0, 2, 1);
                CreateTriangle(0, 1, 3);
                CreateTriangle(0, 3, 2);
                CreateTriangle(1, 2, 3);
            }

            private bool IsLit(int candidate, int w)
            {
                ref Triangle tr = ref _triangles[candidate];
                SoftVector3 deltaA = _vertices[w] - _vertices[tr.A];
                return SoftVector3.Dot(deltaA, tr.Normal) > SoftFloat.Zero;
            }

            private bool CreateTriangle(short a, short b, short c)
            {
                ref Triangle triangle = ref _triangles[_tPointer];
                triangle.A = a;
                triangle.B = b;
                triangle.C = c;

                SoftVector3 u = _vertices[a] - _vertices[b];
                SoftVector3 v = _vertices[a] - _vertices[c];
                triangle.Normal = SoftVector3.Cross(u, v);
                triangle.NormalSq = SoftVector3.LengthSqr(triangle.Normal);

                // no need to add degenerate triangles
                if (triangle.NormalSq < (SoftFloat)1.0e-24)
                    return false;

                // do we need to flip the triangle? (the origin of the md has to be enclosed)
                SoftFloat delta = SoftVector3.Dot(triangle.Normal, _vertices[a] - _center);

                if (delta < SoftFloat.Zero)
                {
                    (triangle.A, triangle.B) = (triangle.B, triangle.A);
                    triangle.Normal = -triangle.Normal;
                }

                delta = SoftVector3.Dot(triangle.Normal, _vertices[a]);
                triangle.FacingOrigin = delta > SoftFloat.Zero;

                if (_originEnclosed)
                {
                    triangle.ClosestToOrigin = triangle.Normal * (delta / triangle.NormalSq);
                    triangle.ClosestToOriginSq = SoftVector3.LengthSqr(triangle.ClosestToOrigin);
                }
                else
                {
                    SoftVector3 bc = CalcBarycentric(triangle, true);
                    triangle.ClosestToOrigin = bc.X * _vertices[triangle.A] + bc.Y * _vertices[triangle.B] +
                                               bc.Z * _vertices[triangle.C];
                    triangle.ClosestToOriginSq = SoftVector3.LengthSqr(triangle.ClosestToOrigin);
                }

                _tPointer++;
                return true;
            }

            public bool Solve(out SoftVector3 point1, out SoftVector3 point2, out SoftVector3 normal,
                out SoftFloat separation)
            {
                _tPointer = 0;
                _originEnclosed = false;

                Mkd.SupportCenter(out _center);
                ConstructInitialTetrahedron(_center);

                int iterations = 0;

                while (++iterations < MaxIter)
                {
                    this.Statistics.Iterations = iterations;

                    // search for the closest triangle and check if the origin is enclosed
                    int closestIndex = -1;
                    SoftFloat currentMin = SoftFloat.MaxValue;
                    _originEnclosed = true;

                    for (int i = 0; i < _tPointer; i++)
                    {
                        if (_triangles[i].ClosestToOriginSq < currentMin)
                        {
                            currentMin = _triangles[i].ClosestToOriginSq;
                            closestIndex = i;
                        }

                        if (!_triangles[i].FacingOrigin)
                            _originEnclosed = false;
                    }

                    if (_tPointer == 0)
                        throw new Exception("Something went wrong!");

                    Triangle closestTriangle = _triangles[closestIndex];
                    SoftVector3 searchDir = closestTriangle.ClosestToOrigin;
                    if (_originEnclosed)
                        searchDir = -searchDir;

                    _vPointer++;
                    Mkd.Support(searchDir, out _verticesA[_vPointer], out _verticesB[_vPointer], out _vertices[_vPointer]);

                    // Termination condition
                    //     c = Triangles[Head].ClosestToOrigin (closest point on the polytope)
                    //     v = Vertices[vPointer] (support point)
                    //     e = CollideEpsilon
                    // The termination condition reads:
                    //     abs(dot(normalize(c), v - c)) < e
                    //     <=>  abs(dot(c, v - c))/len(c) < e <=> abs((dot(c, v) - dot(c,c)))/len(c) < e
                    //     <=>  (dot(c, v) - dot(c,c))^2 < e^2*c^2 <=> (dot(c, v) - c^2)^2 < e^2*c^2
                    SoftFloat deltaDist = closestTriangle.ClosestToOriginSq -
                                          SoftVector3.Dot(_vertices[_vPointer], closestTriangle.ClosestToOrigin);

                    if (deltaDist * deltaDist < SoftMath.CalculationsEpsilonSqr * closestTriangle.ClosestToOriginSq)
                    {
                        goto converged;
                    }

                    int ePointer = 0;
                    for (int index = _tPointer; index-- > 0;)
                    {
                        if (!IsLit(index, _vPointer))
                            continue;

                        for (int k = 0; k < 3; k++)
                        {
                            Edge edge = new Edge(_triangles[index][(k + 0) % 3], _triangles[index][(k + 1) % 3]);
                            bool added = true;
                            for (int e = ePointer; e-- > 0;)
                            {
                                if (Edge.Equals(_edges[e], edge))
                                {
                                    _edges[e] = _edges[--ePointer];
                                    added = false;
                                }
                            }

                            if (added)
                                _edges[ePointer++] = edge;
                        }

                        _triangles[index] = _triangles[--_tPointer];
                    }

                    for (int i = 0; i < ePointer; i++)
                    {
                        if (!CreateTriangle(_edges[i].A, _edges[i].B, _vPointer))
                            goto converged;
                    }

                    if (ePointer > 0)
                        continue;

                    converged:
                    separation = SoftMath.Sqrt(closestTriangle.ClosestToOriginSq);
                    if (_originEnclosed)
                        separation *= -SoftFloat.One;

                    this.Statistics.Accuracy = SoftMath.Abs(deltaDist) / separation;

                    SoftVector3 bc = CalcBarycentric(closestTriangle, !_originEnclosed);

                    point1 = bc.X * _verticesA[closestTriangle.A] + bc.Y * _verticesA[closestTriangle.B] +
                             bc.Z * _verticesA[closestTriangle.C];
                    point2 = bc.X * _verticesB[closestTriangle.A] + bc.Y * _verticesB[closestTriangle.B] +
                             bc.Z * _verticesB[closestTriangle.C];

                    normal = closestTriangle.Normal * (SoftFloat.One / SoftMath.Sqrt(closestTriangle.NormalSq));

                    return true;
                }

                point1 = point2 = normal = SoftVector3.Zero;
                separation = SoftFloat.Zero;

                System.Diagnostics.Debug.WriteLine($"EPA: Could not converge within {MaxIter} iterations.");

                return false;
            }
        }

        public static Collision Detect(ISupportMappingCollider supportA, ISupportMappingCollider supportB)
        {
            var epaSolver = new GjkepaSolver();

            epaSolver.Mkd.SupportA = supportA;
            epaSolver.Mkd.SupportB = supportB;

            bool success = epaSolver.Solve(out var pointA, out var pointB, out var normal, out var separation);

            return new Collision(success, new[] {new ContactPoint(pointA)}, normal, separation);
        }
    }
}
