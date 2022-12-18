using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRaycastCollider
    {
        RaycastHit Raycast(SoftVector3 from, SoftVector3 direction);
    }
}
