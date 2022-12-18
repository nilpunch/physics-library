using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.SupportMapping
{
    public static class GjkAlgorithm
    {
        public struct Result
        {
            public Result(bool collisionHappened, List<SoftVector3> simplex, int iterations, SoftVector3 direction)
            {
                CollisionHappened = collisionHappened;
                Simplex = simplex;
                Iterations = iterations;
                Direction = direction;
            }

            public bool CollisionHappened { get; }
            public List<SoftVector3> Simplex { get; }

            public int Iterations { get; }
            public SoftVector3 Direction { get; }
        }

        public static Result Calculate(ISMCollider shapeA, ISMCollider shapeB, int maxIterations)
        {
            List<SoftVector3> simplex = new List<SoftVector3>();

            SoftVector3 direction = SoftVector3.NormalizeSafe(shapeB.Centre - shapeA.Centre, SoftVector3.Up);

            bool colliding = false;
            int iterations = 0;
            while (iterations < maxIterations)
            {
                iterations++;

                SoftVector3 supportPoint = MinkowskiDifference(shapeA, shapeB, direction);
                simplex.Add(supportPoint);

                if(SoftVector3.Dot(supportPoint, direction) <= SoftFloat.Zero)
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

        public static SoftVector3 MinkowskiDifference(ISMCollider shapeA, ISMCollider shapeB, SoftVector3 direction)
        {
            return shapeA.SupportPoint(direction) - shapeB.SupportPoint(-direction);
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


        private static (bool encloseOrigin, SoftVector3 nextDirection) TryEncloseOrigin(List<SoftVector3> simplex,
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
                        SoftVector3 ab = simplex[1] - simplex[0];
                        // line a0 is the line from the first vertex to the origin
                        SoftVector3 a0 = -simplex[0];

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
                        SoftVector3 ab = simplex[1] - simplex[0];
                        SoftVector3 ac = simplex[2] - simplex[0];
                        direction = SoftVector3.Cross(ab, ac);

                        // ensure it points toward the origin
                        SoftVector3 a0 = -simplex[0];
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
                        SoftVector3 ab = simplex[1] - simplex[0];
                        SoftVector3 ac = simplex[2] - simplex[0];
                        SoftVector3 ad = simplex[3] - simplex[0];

                        SoftVector3 bc = simplex[2] - simplex[1];
                        SoftVector3 bd = simplex[3] - simplex[1];
                        SoftVector3 ba = -ab;

                        // ABC
                        direction = SoftVector3.Normalize(SoftVector3.Cross(ab, ac));
                        if(SoftVector3.Dot(ad, direction) > SoftFloat.Zero)
                        {
                            direction = -direction;
                        }
                        if(SoftVector3.Dot(simplex[0], direction) < SoftFloat.Zero)
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
                        if(SoftVector3.Dot(simplex[0], direction) < SoftFloat.Zero)
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
                        if(SoftVector3.Dot(simplex[0], direction) < SoftFloat.Zero)
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
                        if(SoftVector3.Dot(simplex[1], direction) < SoftFloat.Zero)
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

        private readonly struct EvolveResult
        {
            public EvolveResult(EvolutionState state, SoftVector3 direction)
            {
                State = state;
                Direction = direction;
            }

            public EvolutionState State { get; }
            public SoftVector3 Direction { get; }
        }

        private enum EvolutionState
        {
            NoIntersection,
            FoundIntersection,
            StillEvolving
        }
    }
}
