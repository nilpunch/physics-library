using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class DoubleCastCollidersWorld<TNumber, TConcrete> : AnalyticCollidersWorld<TNumber, TConcrete, IDoubleCastCollider<TNumber>> where TNumber : struct, INumber<TNumber>
    {
        protected override Collision<TNumber> CalculateCollision(IDoubleCastCollider<TNumber> first, IDoubleCastCollider<TNumber> second)
        {
            return first.ColliderCast(second);
        }
    }
}
