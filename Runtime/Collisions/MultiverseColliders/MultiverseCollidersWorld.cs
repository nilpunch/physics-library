using GameLibrary.Physics.Raycast;
using GameLibrary.Physics.SupportMapping;

namespace GameLibrary.Physics
{
    public class MultiverseCollidersWorld : IRigidbodyCollidersWorld<IMultiverseCollider>
    {
        private readonly IRigidbodyCollidersWorld<ISMCollider> _smCollidersWorld;
        private readonly IRigidbodyCollidersWorld<IRaycastCollider> _raycastCollidersWorld;

        public MultiverseCollidersWorld(IRigidbodyCollidersWorld<ISMCollider> smCollidersWorld, IRigidbodyCollidersWorld<IRaycastCollider> raycastCollidersWorld)
        {
            _smCollidersWorld = smCollidersWorld;
            _raycastCollidersWorld = raycastCollidersWorld;
        }

        public void Add(IMultiverseCollider collider, IRigidbody concrete)
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
