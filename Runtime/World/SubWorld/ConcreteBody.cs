namespace GameLibrary.Physics
{
    public readonly struct ConcreteBody<TBody, TConcrete>
    {
        public ConcreteBody(TBody body, TConcrete concrete)
        {
            Body = body;
            Concrete = concrete;
        }

        public TConcrete Concrete { get; }
        public TBody Body { get; }
    }
}
