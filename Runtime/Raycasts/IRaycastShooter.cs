using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRaycastShooter<TRaycastTarget>
    {
        ConcreteRaycastHit<TRaycastTarget> Raycast(SoftVector3 from, SoftVector3 direction);
    }
}
