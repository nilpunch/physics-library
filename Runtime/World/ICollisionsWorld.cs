using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface ICollisionsWorld<TBody>
    {
        RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction);
    }
}
