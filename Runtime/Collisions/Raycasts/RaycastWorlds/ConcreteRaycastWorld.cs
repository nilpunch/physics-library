using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class ConcreteRaycastWorld<TConcrete> : IConcreteRaycastWorld<TConcrete>
    {
        private readonly Dictionary<IRaycastCollider, TConcrete> _colliders;

        public ConcreteRaycastWorld()
        {
            _colliders = new Dictionary<IRaycastCollider, TConcrete>();
        }

        public void Add(IRaycastCollider collider, TConcrete concrete)
        {
            _colliders.Add(collider, concrete);
        }

        public void Remove(IRaycastCollider collider)
        {
            _colliders.Remove(collider);
        }

        public ConcreteRaycastHit<TConcrete> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
