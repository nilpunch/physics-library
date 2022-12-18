namespace GameLibrary.Physics
{
    public readonly struct ConcreteCollider<TCollider, TConcrete>
    {
        public ConcreteCollider(TCollider collider, TConcrete concrete)
        {
            Collider = collider;
            Concrete = concrete;
        }

        public TCollider Collider { get; }
        public TConcrete Concrete { get; }
    }
}
