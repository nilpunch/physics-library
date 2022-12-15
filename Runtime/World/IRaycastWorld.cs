using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRaycastWorld<TBody>
    {
        RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction);
    }
}
