using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;
using GameLibrary.Physics.SupportMapping;


namespace GameLibrary.Physics
{
    public class PhysicSimulation<TBody> : ISimulate
    {
        private readonly ICollisions<TBody> _collisions;
        private readonly ICollisionsSolver<TBody> _solver;

        public PhysicSimulation(ICollisions<TBody> collisions, ICollisionsSolver<TBody> solver)
        {
            _collisions = collisions;
            _solver = solver;
        }

        public void Step(long stepMilliseconds)
        {
            ICollisionManifold<TBody>[] collisionManifolds = _collisions.FindCollisions();
            _solver.Solve(collisionManifolds, stepMilliseconds);
        }
    }
}
