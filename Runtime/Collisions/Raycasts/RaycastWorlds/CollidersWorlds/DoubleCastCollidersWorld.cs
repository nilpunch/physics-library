namespace GameLibrary.Physics.Raycast
{
    public class DoubleCastCollidersWorld : AnalyticCollidersWorld<IDoubleCastCollider>
    {
        protected override Collision CalculateCollision(IDoubleCastCollider first, IDoubleCastCollider second)
        {
            return first.ColliderCast(second);
        }
    }
}
