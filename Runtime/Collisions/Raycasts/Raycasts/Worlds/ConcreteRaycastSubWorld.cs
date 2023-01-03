using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class ConcreteRaycastSubWorld<TConcrete> : IConcreteRaycastWorld<TConcrete>
    {
        private readonly IRaycastWorld _parent;
        private readonly List<ConcreteCollider<IRaycastCollider, TConcrete>> _localColliders;

        public ConcreteRaycastSubWorld(IRaycastWorld parent)
        {
            _parent = parent;
            _localColliders = new List<ConcreteCollider<IRaycastCollider, TConcrete>>();
        }

        public void Add(ConcreteCollider<IRaycastCollider, TConcrete> collider)
        {
            _parent.Add(collider.Collider);
            _localColliders.Add(collider);
        }

        public void Remove(ConcreteCollider<IRaycastCollider, TConcrete> collider)
        {
            _parent.Remove(collider.Collider);
            _localColliders.Remove(collider);
        }

        public ConcreteRaycastHit<TConcrete> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
