using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;
using GameLibrary.Physics.SupportMapping;


namespace GameLibrary.Physics
{
    public class PhysicWorld<TBody> : IPhysicWorld<TBody>, IPhysicStep where TBody : IUnique
    {
        private readonly ICollisionDetector _collisionDetector;
        private readonly ICollisionSolver<TBody> _solver;

        private readonly Dictionary<Guid, TBody> _bodies;

        public PhysicWorld(ICollisionDetector collisionDetector, ICollisionSolver<TBody> solver)
        {
            _collisionDetector = collisionDetector;
            _solver = solver;
            _bodies = new Dictionary<Guid, TBody>();
        }

        public void Step(long stepMilliseconds)
        {
            // TODO:
            // 1. Broad colliders
            // 2. Collect collisions
            // 3. Solve collisions

            CollisionManifold[] collisionManifolds = _collisionDetector.FindCollisions();

            BodiesCollision<TBody>[] bodiesCollisions = collisionManifolds.Select(manifold =>
                new BodiesCollision<TBody>(_bodies[manifold.First], _bodies[manifold.Second], manifold.Collision)).ToArray();

            _solver.Solve(bodiesCollisions, stepMilliseconds);
        }

        public void Add(TBody body)
        {
            _bodies.Add(body.Id, body);
        }

        public void Remove(TBody body)
        {
            _bodies.Remove(body.Id);
        }

        public CollisionManifold[] CollisionsWith(TBody body)
        {
            Collision collision = new Collision();

            // foreach (var simulatedObject in bodies.Where(obj => !obj.Equals(body)))
            //     collision = collision.Merge(simulatedObject.Collider.Collide(body.Collider));

            return Array.Empty<CollisionManifold>();
        }

        public RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
