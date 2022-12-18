namespace GameLibrary.Physics
{
    public interface IWorld<in TObject>
    {
        void Add(TObject instance);
        void Remove(TObject instance);
    }
}
