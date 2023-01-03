using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class CompoundRaycastCollider : IDoubleCastCollider
    {
        private readonly IDoubleCastCollider[] _collidingShells;

        public CompoundRaycastCollider(IDoubleCastCollider[] collidingShells)
        {
            _collidingShells = collidingShells;
        }

        public Collision ColliderCast(IRaycastCollider raycastCollider)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.ColliderCast(raycastCollider));

            return collision;
        }

        public Collision BoxCast(Box box)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.BoxCast(box));

            return collision;
        }

        public Collision SphereCast(Sphere sphere)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.SphereCast(sphere));

            return collision;
        }

        public Collision ConvexHullCast(ConvexHull convexHull)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.ConvexHullCast(convexHull));

            return collision;
        }

        public Collision AABBCast(AABB aabb)
        {
            Collision collision = new Collision();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.AABBCast(aabb));

            return collision;
        }

        public Collision Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
