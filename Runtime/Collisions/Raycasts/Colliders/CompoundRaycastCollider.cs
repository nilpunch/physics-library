using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class CompoundRaycastCollider<TNumber> : IDoubleCastCollider<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IDoubleCastCollider<TNumber>[] _collidingShells;

        public CompoundRaycastCollider(IDoubleCastCollider<TNumber>[] collidingShells)
        {
            _collidingShells = collidingShells;
        }

        public Collision<TNumber> ColliderCast(IRaycastCollider<TNumber> raycastCollider)
        {
            Collision<TNumber> collision = new Collision<TNumber>();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.ColliderCast(raycastCollider));

            return collision;
        }

        public Collision<TNumber> BoxCast(Box<TNumber> box)
        {
            Collision<TNumber> collision = new Collision<TNumber>();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.BoxCast(box));

            return collision;
        }

        public Collision<TNumber> SphereCast(Sphere<TNumber> sphere)
        {
            Collision<TNumber> collision = new Collision<TNumber>();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.SphereCast(sphere));

            return collision;
        }

        public Collision<TNumber> ConvexHullCast(ConvexHull<TNumber> convexHull)
        {
            Collision<TNumber> collision = new Collision<TNumber>();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.ConvexHullCast(convexHull));

            return collision;
        }

        public Collision<TNumber> AABBCast(AABB<TNumber> aabb)
        {
            Collision<TNumber> collision = new Collision<TNumber>();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.AABBCast(aabb));

            return collision;
        }

        public Collision<TNumber> Raycast(Vector3<TNumber> @from, Vector3<TNumber> direction)
        {
            Collision<TNumber> collision = new Collision<TNumber>();

            foreach (var shell in _collidingShells)
                collision = collision.Merge(shell.Raycast(from, direction));

            return collision;
        }
    }
}
