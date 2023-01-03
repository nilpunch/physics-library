namespace GameLibrary.Physics.Raycast
{
    public interface IDoubleCastCollider : IRaycastCollider
    {
        Collision ColliderCast(IRaycastCollider collider);
    }
}
