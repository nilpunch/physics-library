using GameLibrary.Physics.Raycast;
using GameLibrary.Physics.SupportMapping;

namespace GameLibrary.Physics
{
    public class MultiverseCollidersWorld<TCollidingBody> : IConcreteCollidersWorld<IMultiverseCollider, TCollidingBody>
    {
        private readonly IConcreteCollidersWorld<ISMCollider, TCollidingBody> _smCollidersWorld;
        private readonly IConcreteCollidersWorld<IRaycastCollider, TCollidingBody> _raycastCollidersWorld;

        public MultiverseCollidersWorld(IConcreteCollidersWorld<ISMCollider, TCollidingBody> smCollidersWorld, IConcreteCollidersWorld<IRaycastCollider, TCollidingBody> raycastCollidersWorld)
        {
            _smCollidersWorld = smCollidersWorld;
            _raycastCollidersWorld = raycastCollidersWorld;
        }

        public void Add(IMultiverseCollider collider, TCollidingBody concrete)
        {
            _smCollidersWorld.Add(collider, concrete);
            _raycastCollidersWorld.Add(collider, concrete);
        }

        public void Remove(IMultiverseCollider collider)
        {
            _smCollidersWorld.Remove(collider);
            _raycastCollidersWorld.Remove(collider);
        }
    }
}
