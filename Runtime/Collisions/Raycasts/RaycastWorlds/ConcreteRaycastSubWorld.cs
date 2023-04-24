using System;
using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class ConcreteRaycastSubWorld<TNumber, TConcrete> : IConcreteRaycastWorld<TNumber, TConcrete> where TNumber : struct, INumber<TNumber>
    {
        private readonly IRaycastWriteOnlyContainer<TNumber> _parent;
        private readonly Dictionary<IRaycastCollider<TNumber>, TConcrete> _localColliders;

        public ConcreteRaycastSubWorld(IRaycastWriteOnlyContainer<TNumber> parent)
        {
            _parent = parent;
            _localColliders = new Dictionary<IRaycastCollider<TNumber>, TConcrete>();
        }

        public void Add(IRaycastCollider<TNumber> collider, TConcrete concrete)
        {
            _parent.Add(collider);
            _localColliders.Add(collider, concrete);
        }

        public void Remove(IRaycastCollider<TNumber> collider)
        {
            _parent.Remove(collider);
            _localColliders.Remove(collider);
        }

        public ConcreteRaycastHit<TNumber, TConcrete> Raycast(Vector3<TNumber> from, Vector3<TNumber> direction)
        {
            throw new NotImplementedException();
        }
    }
}
