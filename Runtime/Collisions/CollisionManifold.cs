namespace GameLibrary.Physics
{
    public struct CollisionManifold<TCollider>
    {
        public TCollider First { get; }
        public TCollider Second { get; }
        public Collision Collision { get; }
    }
}
