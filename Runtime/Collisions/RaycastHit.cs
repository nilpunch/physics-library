namespace GameLibrary.Physics
{
    public struct RaycastHit<THitResult>
    {
        public bool Occure { get; }

        public ContactPoint ContactPoint { get; }

        public THitResult HitResult { get; }
    }
}
