namespace GameLibrary.Physics
{
    public readonly struct ConcreteCastHit<THitResult>
    {
        public bool Occure { get; }

        public ContactPoint[] ContactPoint { get; }

        public THitResult HitResult { get; }
    }
}
