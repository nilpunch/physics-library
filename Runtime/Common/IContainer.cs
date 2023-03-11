namespace GameLibrary.Physics
{
    public interface IContainer<T> : IWriteOnlyContainer<T>, IReadOnlyContainer<T>
    {
    }
}