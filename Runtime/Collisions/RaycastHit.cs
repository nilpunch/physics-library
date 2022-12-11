namespace GameLibrary.Physics
{
    public readonly struct RaycastHit<THitResult>
    {
        public bool Occure { get; }

        public ContactPoint ContactPoint { get; }

        public THitResult HitResult { get; }
    }
}
