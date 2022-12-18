namespace GameLibrary.Physics
{
    public readonly struct ConcreteRaycastHit<THitResult>
    {
        public bool Occure { get; }

        public ContactPoint ContactPoint { get; }

        public THitResult HitResult { get; }
    }
}
