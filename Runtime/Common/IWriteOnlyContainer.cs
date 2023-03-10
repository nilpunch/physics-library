namespace GameLibrary.Physics
{
    public interface IWriteOnlyContainer<in T>
    {
        void Add(T item);
        void Remove(T item);

        void Clear();
    }
}
