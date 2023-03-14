using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public struct MinkowskiDifference
    {
        public MinkowskiDifference(SoftVector3 supportA, SoftVector3 supportB, SoftVector3 difference)
        {
            SupportA = supportA;
            SupportB = supportB;
            Difference = difference;
        }

        public SoftVector3 SupportA { get; }
        public SoftVector3 SupportB { get; }
        public SoftVector3 Difference { get; }

        public static MinkowskiDifference Calculate(ISMCollider shapeA, ISMCollider shapeB, SoftVector3 direction)
        {
            SoftVector3 supportA = shapeA.SupportPoint(direction);
            SoftVector3 supportB = shapeB.SupportPoint(-direction);
            SoftVector3 difference = supportA - supportB;

            return new MinkowskiDifference(supportA, supportB, difference);
        }
    }

    public static class GjkAlgorithm
    {
        private static SoftFloat Tolerance => (SoftFloat)0.0001f;

        private static List<MinkowskiDifference> SimplexShared { get; } = new List<MinkowskiDifference>(4);

        public struct Result
        {
            public Result(bool collisionHappened, IReadOnlyList<MinkowskiDifference> simplex, int iterations, SoftVector3 direction)
            {
                CollisionHappened = collisionHappened;
                Simplex = simplex;
                Iterations = iterations;
                Direction = direction;
            }

            public bool CollisionHappened { get; }
            public IReadOnlyList<MinkowskiDifference> Simplex { get; }

            public int Iterations { get; }
            public SoftVector3 Direction { get; }
        }

        public static Result Calculate(ISMCollider shapeA, ISMCollider shapeB, int maxIterations)
        {
            SimplexShared.Clear();
            var simplex = SimplexShared;

            SoftVector3 direction = SoftVector3.NormalizeSafe(shapeB.Centre - shapeA.Centre, SoftVector3.Up);

            bool colliding = false;
            int iterations = 1;
            while (iterations < maxIterations)
            {
                iterations++;

                MinkowskiDifference supportPoint = MinkowskiDifference.Calculate(shapeA, shapeB, direction);
                simplex.Add(supportPoint);

                if(SoftVector3.Dot(supportPoint.Difference, direction) <= SoftFloat.Zero)
                {
                    break;
                }

                var encloseResult = TryEncloseOrigin(simplex, shapeA, shapeB, direction);
                if(encloseResult.encloseOrigin)
                {
                    colliding = true;
                    break;
                }

                direction = encloseResult.nextDirection;
            }

            return new Result(colliding, simplex, iterations, direction);
        }

        private static SoftVector3 TripleProduct(SoftVector3 a, SoftVector3 b, SoftVector3 c)
        {
            return SoftVector3.Cross(SoftVector3.Cross(a, b), c);
        }

        private static SoftVector3 CrossOrOrthogonal(SoftVector3 a, SoftVector3 b)
        {
            SoftVector3 cross = SoftVector3.Cross(a, b);

            if (SoftVector3.ApproximatelyEqual(cross, SoftVector3.Zero))
                return SoftVector3.Orthogonal(a);

            return cross;
        }

        private static (bool encloseOrigin, SoftVector3 nextDirection) TryEncloseOrigin(List<MinkowskiDifference> simplex,
            ISMCollider shapeA, ISMCollider shapeB, SoftVector3 direction)
        {
            switch (simplex.Count)
            {
                case 0:
                    {
                        direction = SoftVector3.NormalizeSafe(shapeB.Centre - shapeA.Centre, SoftVector3.Up);
                        break;
                    }
                case 1:
                    {
                        // flip the direction
                        direction = -direction;
                        break;
                    }
                case 2:
                    {
                        // line ab is the line formed by the first two vertices
                        SoftVector3 ab = simplex[1].Difference - simplex[0].Difference;
                        // line a0 is the line from the first vertex to the origin
                        SoftVector3 a0 = -simplex[0].Difference;

                        if (SoftVector3.ApproximatelyEqual(SoftVector3.Cross(ab, a0), SoftVector3.Zero))
                            direction = SoftVector3.Orthogonal(ab);
                        else
                            // use the triple-cross-product to calculate a direction perpendicular
                            // to line ab in the direction of the origin
                            direction = TripleProduct(ab, a0, ab);
                        break;
                    }
                case 3:
                    {
                        SoftVector3 ab = simplex[1].Difference - simplex[0].Difference;
                        SoftVector3 ac = simplex[2].Difference - simplex[0].Difference;
                        direction = SoftVector3.Cross(ab, ac);

                        // ensure it points toward the origin
                        SoftVector3 a0 = -simplex[0].Difference;
                        if (SoftVector3.Dot(direction, a0) < SoftFloat.Zero)
                            direction = -direction;
                        break;
                    }
                case 4:
                    {
                        // ascii representation of our simplex at this point
                        //
                        //                            [D]
                        //                           ,|,
                        //                         ,7``\'VA,
                        //                       ,7`   |, `'VA,
                        //                     ,7`     `\    `'VA,
                        //                   ,7`        |,      `'VA,
                        //                 ,7`          `\         `'VA,
                        //               ,7`             |,           `'VA,
                        //             ,7`               `\       ,..ooOOTK` [C]
                        //           ,7`                  |,.ooOOT''`    AV
                        //         ,7`            ,..ooOOT`\`           /7
                        //       ,7`      ,..ooOOT''`      |,          AV
                        //      ,T,..ooOOT''`              `\         /7
                        // [A] `'TTs.,                      |,       AV
                        //          `'TTs.,                 `\      /7
                        //               `'TTs.,             |,    AV
                        //                    `'TTs.,        `\   /7
                        //                         `'TTs.,    |, AV
                        //                              `'TTs.,\/7
                        //                                   `'T`
                        //                                     [B]
                        //

                        // calculate edges of interest
                        SoftVector3 ab = simplex[1].Difference - simplex[0].Difference;
                        SoftVector3 ac = simplex[2].Difference - simplex[0].Difference;
                        SoftVector3 ad = simplex[3].Difference - simplex[0].Difference;

                        SoftVector3 bc = simplex[2].Difference - simplex[1].Difference;
                        SoftVector3 bd = simplex[3].Difference - simplex[1].Difference;
                        SoftVector3 ba = -ab;

                        // ABC
                        direction = SoftVector3.Normalize(SoftVector3.Cross(ab, ac));
                        if(SoftVector3.Dot(ad, direction) > SoftFloat.Zero)
                        {
                            direction = -direction;
                        }
                        if(SoftVector3.Dot(simplex[0].Difference, direction) < SoftFloat.Zero - Tolerance)
                        {
                            // remove d
                            simplex.RemoveAt(3);
                            return (false, direction);
                        }

                        // ADB
                        direction = SoftVector3.Normalize(SoftVector3.Cross(ab, ad));
                        if(SoftVector3.Dot(ac, direction) > SoftFloat.Zero)
                        {
                            direction = -direction;
                        }
                        if(SoftVector3.Dot(simplex[0].Difference, direction) < SoftFloat.Zero - Tolerance)
                        {
                            // remove c
                            simplex.RemoveAt(2);
                            return (false, direction);
                        }

                        // ACD
                        direction = SoftVector3.Normalize(SoftVector3.Cross(ac, ad));
                        if(SoftVector3.Dot(ab, direction) > SoftFloat.Zero)
                        {
                            direction = -direction;
                        }
                        if(SoftVector3.Dot(simplex[0].Difference, direction) < SoftFloat.Zero - Tolerance)
                        {
                            // remove b
                            simplex.RemoveAt(1);
                            return (false, direction);
                        }

                        // BCD
                        direction = SoftVector3.Normalize(SoftVector3.Cross(bc, bd));
                        if(SoftVector3.Dot(ba, direction) > SoftFloat.Zero)
                        {
                            direction = -direction;
                        }
                        if(SoftVector3.Dot(simplex[1].Difference, direction) < SoftFloat.Zero - Tolerance)
                        {
                            // remove a
                            simplex.RemoveAt(0);
                            return (false, direction);
                        }

                        // origin is in center
                        return (true, direction);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(simplex.Count),
                        "Can\'t have simplex with ${vertices.length} verts!");
            }

            return (false, direction);
        }
    }
}
