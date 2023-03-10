using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class ConcreteRaycastSubWorld<TConcrete> : IConcreteRaycastWorld<TConcrete>
    {
        private readonly IRaycastWriteOnlyContainer _parent;
        private readonly Dictionary<IRaycastCollider, TConcrete> _localColliders;

        public ConcreteRaycastSubWorld(IRaycastWriteOnlyContainer parent)
        {
            _parent = parent;
            _localColliders = new Dictionary<IRaycastCollider, TConcrete>();
        }

        public void Add(IRaycastCollider collider, TConcrete concrete)
        {
            _parent.Add(collider);
            _localColliders.Add(collider, concrete);
        }

        public void Remove(IRaycastCollider collider)
        {
            _parent.Remove(collider);
            _localColliders.Remove(collider);
        }

        public ConcreteRaycastHit<TConcrete> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
