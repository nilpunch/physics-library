using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class RaycastWorld : IRaycastWorld
    {
        private readonly List<IRaycastCollider> _colliders;

        public RaycastWorld()
        {
            _colliders = new List<IRaycastCollider>();
        }

        public void Add(IRaycastCollider collider)
        {
            _colliders.Add(collider);
        }

        public void Remove(IRaycastCollider collider)
        {
            _colliders.Remove(collider);
        }

        public ConcreteRaycastHit<IRaycastCollider> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
