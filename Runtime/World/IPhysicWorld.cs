namespace GameLibrary.Physics
{
    public interface IPhysicWorld<TBody> : IRaycastWorld<TBody>
    {
        void Add(TBody body);
        void Remove(TBody body);
    }
}
