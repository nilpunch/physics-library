namespace GameLibrary.Physics
{
    public struct BodiesCollision<TBody>
    {
        public BodiesCollision(TBody first, TBody second, Collision collision)
        {
            First = first;
            Second = second;
            Collision = collision;
        }

        public TBody First { get; }
        public TBody Second { get; }
        public Collision Collision { get; }
    }
}
