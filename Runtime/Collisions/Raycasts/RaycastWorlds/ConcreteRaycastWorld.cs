using System;
using System.Collections.Generic;
using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class ConcreteRaycastWorld<TNumber, TConcrete> : IConcreteRaycastWorld<TNumber, TConcrete> where TNumber : struct, INumber<TNumber>
    {
        private readonly Dictionary<IRaycastCollider<TNumber>, TConcrete> _colliders;

        public ConcreteRaycastWorld()
        {
            _colliders = new Dictionary<IRaycastCollider<TNumber>, TConcrete>();
        }

        public void Add(IRaycastCollider<TNumber> collider, TConcrete concrete)
        {
            _colliders.Add(collider, concrete);
        }

        public void Remove(IRaycastCollider<TNumber> collider)
        {
            _colliders.Remove(collider);
        }

        public ConcreteRaycastHit<TNumber, TConcrete> Raycast(Vector3<TNumber> from, Vector3<TNumber> direction)
        {
            throw new NotImplementedException();
        }
    }
}
