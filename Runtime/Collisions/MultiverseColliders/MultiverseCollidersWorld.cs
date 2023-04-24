using GameLibrary.Physics.Raycast;
using GameLibrary.Physics.SupportMapping;
using PluggableMath;

namespace GameLibrary.Physics
{
    public class MultiverseCollidersWorld<TNumber, TCollidingBody> : IConcreteCollidersWorld<IMultiverseCollider<TNumber>, TCollidingBody> where TNumber : struct, INumber<TNumber>
    {
        private readonly IConcreteCollidersWorld<ISMCollider<TNumber>, TCollidingBody> _smCollidersWorld;
        private readonly IConcreteCollidersWorld<IRaycastCollider<TNumber>, TCollidingBody> _raycastCollidersWorld;

        public MultiverseCollidersWorld(IConcreteCollidersWorld<ISMCollider<TNumber>, TCollidingBody> smCollidersWorld, IConcreteCollidersWorld<IRaycastCollider<TNumber>, TCollidingBody> raycastCollidersWorld)
        {
            _smCollidersWorld = smCollidersWorld;
            _raycastCollidersWorld = raycastCollidersWorld;
        }

        public void Add(IMultiverseCollider<TNumber> collider, TCollidingBody concrete)
        {
            _smCollidersWorld.Add(collider, concrete);
            _raycastCollidersWorld.Add(collider, concrete);
        }

        public void Remove(IMultiverseCollider<TNumber> collider)
        {
            _smCollidersWorld.Remove(collider);
            _raycastCollidersWorld.Remove(collider);
        }
    }
}
