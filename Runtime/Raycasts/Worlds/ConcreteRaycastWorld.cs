using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class ConcreteRaycastWorld<TConcrete> : IConcreteRaycastWorld<TConcrete>
    {
        private readonly List<ConcreteCollider<IRaycastCollider, TConcrete>> _colliders;

        public ConcreteRaycastWorld()
        {
            _colliders = new List<ConcreteCollider<IRaycastCollider, TConcrete>>();
        }

        public void Add(ConcreteCollider<IRaycastCollider, TConcrete> collider)
        {
            _colliders.Add(collider);
        }

        public void Remove(ConcreteCollider<IRaycastCollider, TConcrete> collider)
        {
            _colliders.Remove(collider);
        }

        public ConcreteRaycastHit<TConcrete> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
