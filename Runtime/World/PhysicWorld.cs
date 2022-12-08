using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;


namespace GameLibrary.Physics
{
    public class PhysicWorld<TBody, TCollider> : IPhysicWorld<TBody>, IPhysicStep
        where TBody : IPhysicalBody<TCollider>
    {
        private readonly ICollisionCollector<TCollider> _collector;
        private readonly ICollisionSolver<TBody> _solver;

        private readonly List<TBody> _bodies;

        public PhysicWorld(ICollisionCollector<TCollider> collector, ICollisionSolver<TBody> solver)
        {
            _collector = collector;
            _solver = solver;
            _bodies = new List<TBody>();
        }

        public void Step(long stepMilliseconds)
        {
            // TODO:
            // 1. Broad colliders
            // 2. Collect collisions
            // 3. Solve collisions

            Dictionary<TCollider, TBody> collidersBodies =
                new Dictionary<TCollider, TBody>(_bodies.Select(body =>
                    new KeyValuePair<TCollider, TBody>(body.Collider, body)));

            CollisionManifold<TCollider>[] collisionManifolds =
                _collector.CollectManifolds(collidersBodies.Keys.ToArray());

            _solver.Solve(collisionManifolds.Select(manifold =>
                new BodiesCollision<TBody>(
                    collidersBodies[manifold.First],
                    collidersBodies[manifold.Second],
                    manifold.Collision)).ToArray(), stepMilliseconds);
        }

        public void Add(TBody body)
        {
            _bodies.Add(body);
        }

        public void Remove(TBody body)
        {
            _bodies.Remove(body);
        }

        public CollisionManifold<TBody>[] CollisionsWith(TBody body)
        {
            Collision collision = new Collision();

            // foreach (var simulatedObject in bodies.Where(obj => !obj.Equals(body)))
            //     collision = collision.Merge(simulatedObject.Collider.Collide(body.Collider));

            return Array.Empty<CollisionManifold<TBody>>();
        }

        public RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
