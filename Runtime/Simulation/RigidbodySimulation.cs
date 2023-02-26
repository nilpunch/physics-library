using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;
using GameLibrary.Physics.SupportMapping;


namespace GameLibrary.Physics
{
    public class RigidbodySimulation : ISimulate
    {
        private readonly IRigidbodyCollisionsFinder _collisionsFinder;
        private readonly IRigidbodyCollisionsSolver _solver;

        public RigidbodySimulation(IRigidbodyCollisionsFinder collisionsFinder, IRigidbodyCollisionsSolver solver)
        {
            _collisionsFinder = collisionsFinder;
            _solver = solver;
        }

        public void Step(long stepMilliseconds)
        {
            CollisionManifold<IRigidbody>[] collisionManifolds = _collisionsFinder.FindCollisions();
            _solver.Solve(collisionManifolds, stepMilliseconds);
        }
    }
}
