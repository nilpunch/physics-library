namespace GameLibrary.Physics.Raycast
{
    public class DoubleCastCollidersWorld<TConcrete> : AnalyticCollidersWorld<TConcrete, IDoubleCastCollider>
    {
        protected override Collision CalculateCollision(IDoubleCastCollider first, IDoubleCastCollider second)
        {
            return first.ColliderCast(second);
        }
    }
}
