using PluggableMath;

namespace GameLibrary.Physics
{
    public interface ICollisions<TNumber, TBody> where TNumber : struct, INumber<TNumber>
    {
        void FindCollisionsNonAlloc(IWriteOnlyContainer<CollisionManifold<TNumber, TBody>> output);
    }
}
