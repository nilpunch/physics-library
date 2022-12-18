using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;
using GameLibrary.Physics.SupportMapping;


namespace GameLibrary.Physics
{
    public class PhysicSimulation<TBody> : ISimulate
    {
        private readonly IManifoldFinder<TBody> _manifoldFinder;
        private readonly ICollisionSolver<TBody> _solver;

        public PhysicSimulation(IManifoldFinder<TBody> manifoldFinder, ICollisionSolver<TBody> solver)
        {
            _manifoldFinder = manifoldFinder;
            _solver = solver;
        }

        public void Step(long stepMilliseconds)
        {
            ICollisionManifold<TBody>[] collisionManifolds = _manifoldFinder.FindCollisions();
            _solver.Solve(collisionManifolds, stepMilliseconds);
        }
    }
}
