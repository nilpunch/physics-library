using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class BoxCollider : IDoubleCastCollider
    {
        private readonly Box _box;

        public BoxCollider(Box box)
        {
            _box = box;
        }

        public Collision ColliderCast(IRaycastCollider raycastCollider)
        {
            return raycastCollider.BoxCast(_box);
        }

        public Collision BoxCast(Box box)
        {
            return AnalyticCollisionsLibrary.BoxAgainstBox(_box, box);
        }

        public Collision SphereCast(Sphere sphere)
        {
            return AnalyticCollisionsLibrary.SphereAgainstBox(sphere, _box);
        }

        public Collision ConvexHullCast(ConvexHull convexHull)
        {
            return AnalyticCollisionsLibrary.ConvexAgainstBox(convexHull, _box);
        }

        public Collision AABBCast(AABB aabb)
        {
            return AnalyticCollisionsLibrary.AABBAgainstBox(aabb, _box);
        }

        public Collision Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
