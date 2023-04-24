using System;
using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.SupportMapping
{
    public struct MinkowskiDifference<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public MinkowskiDifference(Vector3<TNumber> supportA, Vector3<TNumber> supportB, Vector3<TNumber> difference)
        {
            SupportA = supportA;
            SupportB = supportB;
            Difference = difference;
        }

        public Vector3<TNumber> SupportA { get; }
        public Vector3<TNumber> SupportB { get; }
        public Vector3<TNumber> Difference { get; }

        public static MinkowskiDifference<TNumber> Calculate(ISMCollider<TNumber> shapeA, ISMCollider<TNumber> shapeB, Vector3<TNumber> direction)
        {
            Vector3<TNumber> supportA = shapeA.SupportPoint(direction);
            Vector3<TNumber> supportB = shapeB.SupportPoint(-direction);
            Vector3<TNumber> difference = supportA - supportB;

            return new MinkowskiDifference<TNumber>(supportA, supportB, difference);
        }
    }

    public static class GjkAlgorithm<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private static Operand<TNumber> Tolerance => (Operand<TNumber>)0.0001f;

        private static List<MinkowskiDifference<TNumber>> SimplexShared { get; } = new List<MinkowskiDifference<TNumber>>(4);

        public struct Result
        {
            public Result(bool collisionHappened, IReadOnlyList<MinkowskiDifference<TNumber>> simplex, int iterations, Vector3<TNumber> direction)
            {
                CollisionHappened = collisionHappened;
                Simplex = simplex;
                Iterations = iterations;
                Direction = direction;
            }

            public bool CollisionHappened { get; }
            public IReadOnlyList<MinkowskiDifference<TNumber>> Simplex { get; }

            public int Iterations { get; }
            public Vector3<TNumber> Direction { get; }
        }

        public static Result Calculate(ISMCollider<TNumber> shapeA, ISMCollider<TNumber> shapeB, int maxIterations)
        {
            SimplexShared.Clear();
            var simplex = SimplexShared;

            Vector3<TNumber> direction = Vector3<TNumber>.NormalizeSafe(shapeB.Centre - shapeA.Centre, Vector3<TNumber>.Up);

            bool colliding = false;
            int iterations = 1;
            while (iterations < maxIterations)
            {
                iterations++;

                MinkowskiDifference<TNumber> supportPoint = MinkowskiDifference<TNumber>.Calculate(shapeA, shapeB, direction);
                simplex.Add(supportPoint);

                if(Vector3<TNumber>.Dot(supportPoint.Difference, direction) <= Operand<TNumber>.Zero)
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

        private static Vector3<TNumber> TripleProduct(Vector3<TNumber> a, Vector3<TNumber> b, Vector3<TNumber> c)
        {
            return Vector3<TNumber>.Cross(Vector3<TNumber>.Cross(a, b), c);
        }

        private static Vector3<TNumber> CrossOrOrthogonal(Vector3<TNumber> a, Vector3<TNumber> b)
        {
            Vector3<TNumber> cross = Vector3<TNumber>.Cross(a, b);

            if (Vector3<TNumber>.ApproximatelyEqual(cross, Vector3<TNumber>.Zero))
                return Vector3<TNumber>.Orthogonal(a);

            return cross;
        }

        private static (bool encloseOrigin, Vector3<TNumber> nextDirection) TryEncloseOrigin(List<MinkowskiDifference<TNumber>> simplex,
            ISMCollider<TNumber> shapeA, ISMCollider<TNumber> shapeB, Vector3<TNumber> direction)
        {
            switch (simplex.Count)
            {
                case 0:
                    {
                        direction = Vector3<TNumber>.NormalizeSafe(shapeB.Centre - shapeA.Centre, Vector3<TNumber>.Up);
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
                        Vector3<TNumber> ab = simplex[1].Difference - simplex[0].Difference;
                        // line a0 is the line from the first vertex to the origin
                        Vector3<TNumber> a0 = -simplex[0].Difference;

                        if (Vector3<TNumber>.ApproximatelyEqual(Vector3<TNumber>.Cross(ab, a0), Vector3<TNumber>.Zero))
                            direction = Vector3<TNumber>.Orthogonal(ab);
                        else
                            // use the triple-cross-product to calculate a direction perpendicular
                            // to line ab in the direction of the origin
                            direction = TripleProduct(ab, a0, ab);
                        break;
                    }
                case 3:
                    {
                        Vector3<TNumber> ab = simplex[1].Difference - simplex[0].Difference;
                        Vector3<TNumber> ac = simplex[2].Difference - simplex[0].Difference;
                        direction = Vector3<TNumber>.Cross(ab, ac);

                        // ensure it points toward the origin
                        Vector3<TNumber> a0 = -simplex[0].Difference;
                        if (Vector3<TNumber>.Dot(direction, a0) < Operand<TNumber>.Zero)
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
                        Vector3<TNumber> ab = simplex[1].Difference - simplex[0].Difference;
                        Vector3<TNumber> ac = simplex[2].Difference - simplex[0].Difference;
                        Vector3<TNumber> ad = simplex[3].Difference - simplex[0].Difference;

                        Vector3<TNumber> bc = simplex[2].Difference - simplex[1].Difference;
                        Vector3<TNumber> bd = simplex[3].Difference - simplex[1].Difference;
                        Vector3<TNumber> ba = -ab;

                        // ABC
                        direction = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(ab, ac));
                        if(Vector3<TNumber>.Dot(ad, direction) > Operand<TNumber>.Zero)
                        {
                            direction = -direction;
                        }
                        if(Vector3<TNumber>.Dot(simplex[0].Difference, direction) < Operand<TNumber>.Zero - Tolerance)
                        {
                            // remove d
                            simplex.RemoveAt(3);
                            return (false, direction);
                        }

                        // ADB
                        direction = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(ab, ad));
                        if(Vector3<TNumber>.Dot(ac, direction) > Operand<TNumber>.Zero)
                        {
                            direction = -direction;
                        }
                        if(Vector3<TNumber>.Dot(simplex[0].Difference, direction) < Operand<TNumber>.Zero - Tolerance)
                        {
                            // remove c
                            simplex.RemoveAt(2);
                            return (false, direction);
                        }

                        // ACD
                        direction = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(ac, ad));
                        if(Vector3<TNumber>.Dot(ab, direction) > Operand<TNumber>.Zero)
                        {
                            direction = -direction;
                        }
                        if(Vector3<TNumber>.Dot(simplex[0].Difference, direction) < Operand<TNumber>.Zero - Tolerance)
                        {
                            // remove b
                            simplex.RemoveAt(1);
                            return (false, direction);
                        }

                        // BCD
                        direction = Vector3<TNumber>.Normalize(Vector3<TNumber>.Cross(bc, bd));
                        if(Vector3<TNumber>.Dot(ba, direction) > Operand<TNumber>.Zero)
                        {
                            direction = -direction;
                        }
                        if(Vector3<TNumber>.Dot(simplex[1].Difference, direction) < Operand<TNumber>.Zero - Tolerance)
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
