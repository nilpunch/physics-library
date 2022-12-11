using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.GJK
{
    public static class GjkAlgorithm
    {
        public struct Result
        {
            public Result(bool collisionHappened, List<SoftVector3> simplex)
            {
                CollisionHappened = collisionHappened;
                Simplex = simplex;
            }

            public bool CollisionHappened { get; }
            public List<SoftVector3> Simplex { get; }
        }

        public static Result Calculate(ISupportMappingCollider shapeA, ISupportMappingCollider shapeB, int maxIterations)
        {
            List<SoftVector3> simplex = new List<SoftVector3>();
            SoftVector3 direction = new SoftVector3();

            // do the actual test
            EvolveResult result = EvolveResult.StillEvolving;
            int iterations = 0;
            while (iterations < maxIterations && result == EvolveResult.StillEvolving)
            {
                result = EvolveSimplex(simplex, shapeA, shapeB, direction);
                iterations++;
            }

            return new Result(result == EvolveResult.FoundIntersection, simplex);
        }

        public static SoftVector3 MinkowskiDifference(ISupportMappingCollider shapeA, ISupportMappingCollider shapeB, SoftVector3 direction)
        {
            SoftVector3 oppositeDirection = direction * SoftFloat.MinusOne;
            SoftVector3 minkowskiDifference = shapeA.SupportPoint(direction) - shapeB.SupportPoint(oppositeDirection);
            return minkowskiDifference;
        }

        private static SoftVector3 TripleProduct(SoftVector3 a, SoftVector3 b, SoftVector3 c)
        {
            return SoftVector3.Cross(SoftVector3.Cross(a, b), c);
            ;
        }

        private static EvolveResult EvolveSimplex(List<SoftVector3> simplex, ISupportMappingCollider shapeA, ISupportMappingCollider shapeB,
            SoftVector3 direction)
        {
            switch (simplex.Count)
            {
                case 0:
                    {
                        direction = shapeB.Centre - shapeA.Centre;
                        break;
                    }
                case 1:
                    {
                        // flip the direction
                        direction *= SoftFloat.MinusOne;
                        break;
                    }
                case 2:
                    {
                        // line ab is the line formed by the first two vertices
                        SoftVector3 ab = simplex[1] - simplex[0];
                        // line a0 is the line from the first vertex to the origin
                        SoftVector3 a0 = simplex[0] * SoftFloat.MinusOne;

                        // use the triple-cross-product to calculate a direction perpendicular
                        // to line ab in the direction of the origin
                        direction = TripleProduct(ab, a0, direction);
                        break;
                    }
                case 3:
                    {
                        SoftVector3 ac = simplex[2] - simplex[0];
                        SoftVector3 ab = simplex[1] - simplex[0];
                        direction = SoftVector3.Cross(ac, ab);

                        // ensure it points toward the origin
                        SoftVector3 a0 = simplex[0] * SoftFloat.MinusOne;
                        if (SoftVector3.Dot(direction, a0) < SoftFloat.Zero)
                            direction *= SoftFloat.MinusOne;
                        break;
                    }
                case 4:
                    {
                        // ascii representation of our simplex at this point
                        /*
                                                   [D]
                                                  ,|,
                                                ,7``\'VA,
                                              ,7`   |, `'VA,
                                            ,7`     `\    `'VA,
                                          ,7`        |,      `'VA,
                                        ,7`          `\         `'VA,
                                      ,7`             |,           `'VA,
                                    ,7`               `\       ,..ooOOTK` [C]
                                  ,7`                  |,.ooOOT''`    AV
                                ,7`            ,..ooOOT`\`           /7
                              ,7`      ,..ooOOT''`      |,          AV
                             ,T,..ooOOT''`              `\         /7
                        [A] `'TTs.,                      |,       AV
                                 `'TTs.,                 `\      /7
                                      `'TTs.,             |,    AV
                                           `'TTs.,        `\   /7
                                                `'TTs.,    |, AV
                                                     `'TTs.,\/7
                                                          `'T`
                                                            [B]
                        */

                        // calculate the three edges of interest
                        SoftVector3 da = simplex[3] - simplex[0];
                        SoftVector3 db = simplex[3] - simplex[1];
                        SoftVector3 dc = simplex[3] - simplex[2];

                        // and the direction to the origin
                        SoftVector3 d0 = simplex[3] * SoftFloat.MinusOne;

                        // check triangles a-b-d, b-c-d, and c-a-d
                        var abdNorm = SoftVector3.Cross(da, db);
                        var bcdNorm = SoftVector3.Cross(db, dc);
                        var cadNorm = SoftVector3.Cross(dc, da);

                        if (SoftVector3.Dot(abdNorm, d0) > SoftFloat.Zero)
                        {
                            // the origin is on the outside of triangle a-b-d
                            // eliminate c!
                            simplex.Remove(simplex[2]);
                            direction = abdNorm;
                        }
                        else if (SoftVector3.Dot(bcdNorm, d0) > SoftFloat.Zero)
                        {
                            // the origin is on the outside of triangle bcd
                            // eliminate a!
                            simplex.Remove(simplex[0]);
                            direction = bcdNorm;
                        }
                        else if (SoftVector3.Dot(cadNorm, d0) > SoftFloat.Zero)
                        {
                            // the origin is on the outside of triangle cad
                            // eliminate b!
                            simplex.Remove(simplex[1]);
                            direction = cadNorm;
                        }
                        else
                        {
                            // the origin is inside all of the triangles!
                            return EvolveResult.FoundIntersection;
                        }

                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(simplex.Count),
                        "Can\'t have simplex with ${vertices.length} verts!");
            }

            SoftVector3 newVertex = MinkowskiDifference(shapeA, shapeB, direction);
            simplex.Add(newVertex);

            return SoftVector3.Dot(direction, newVertex) >= SoftFloat.Zero - SoftMath.CalculationsEpsilon
                ? EvolveResult.StillEvolving
                : EvolveResult.NoIntersection;
        }


        private enum EvolveResult
        {
            NoIntersection,
            FoundIntersection,
            StillEvolving
        }
    }
}
