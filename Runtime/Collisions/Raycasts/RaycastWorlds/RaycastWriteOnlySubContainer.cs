using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class RaycastWriteOnlySubContainer : IRaycastWriteOnlyContainer
    {
        private readonly IRaycastWriteOnlyContainer _parent;
        private readonly List<IRaycastCollider> _localColliders;

        public RaycastWriteOnlySubContainer(IRaycastWriteOnlyContainer parent)
        {
            _parent = parent;
            _localColliders = new List<IRaycastCollider>();
        }

        public void Add(IRaycastCollider item)
        {
            _parent.Add(item);
            _localColliders.Add(item);
        }

        public void Remove(IRaycastCollider item)
        {
            _parent.Remove(item);
            _localColliders.Remove(item);
        }

        public void Clear()
        {
            foreach (var item in _localColliders)
            {
                _parent.Remove(item);
            }
            _localColliders.Clear();
        }

        public ConcreteRaycastHit<IRaycastCollider> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
