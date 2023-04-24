using System;
using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class RaycastWriteOnlySubContainer<TNumber> : IRaycastWriteOnlyContainer<TNumber> where TNumber : struct, INumber<TNumber>
    {
        private readonly IRaycastWriteOnlyContainer<TNumber> _parent;
        private readonly List<IRaycastCollider<TNumber>> _localColliders;

        public RaycastWriteOnlySubContainer(IRaycastWriteOnlyContainer<TNumber> parent)
        {
            _parent = parent;
            _localColliders = new List<IRaycastCollider<TNumber>>();
        }

        public void Add(IRaycastCollider<TNumber> item)
        {
            _parent.Add(item);
            _localColliders.Add(item);
        }

        public void Remove(IRaycastCollider<TNumber> item)
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

        public ConcreteRaycastHit<TNumber, IRaycastCollider<TNumber>> Raycast(Vector3<TNumber> from, Vector3<TNumber> direction)
        {
            throw new NotImplementedException();
        }
    }
}
