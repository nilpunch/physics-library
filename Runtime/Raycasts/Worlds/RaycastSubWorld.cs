using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class RaycastSubWorld : IRaycastWorld
    {
        private readonly IRaycastWorld _parent;
        private readonly List<IRaycastCollider> _localColliders;

        public RaycastSubWorld(IRaycastWorld parent)
        {
            _parent = parent;
            _localColliders = new List<IRaycastCollider>();
        }

        public void Add(IRaycastCollider collider)
        {
            _parent.Add(collider);
            _localColliders.Add(collider);
        }

        public void Remove(IRaycastCollider collider)
        {
            _parent.Remove(collider);
            _localColliders.Remove(collider);
        }

        public ConcreteRaycastHit<IRaycastCollider> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
