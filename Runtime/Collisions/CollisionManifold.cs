namespace GameLibrary.Physics
{
    public struct CollisionManifold<TCollider>
    {
        public CollisionManifold(TCollider first, TCollider second, Collision collision)
        {
            First = first;
            Second = second;
            Collision = collision;
        }

        public TCollider First { get; }
        public TCollider Second { get; }
        public Collision Collision { get; }
    }
}
