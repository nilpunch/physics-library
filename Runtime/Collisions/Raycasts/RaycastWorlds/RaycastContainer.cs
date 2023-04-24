using System;
using PluggableMath;

namespace GameLibrary.Physics.Raycast
{
    public class RaycastContainer<TNumber> : Container<IRaycastCollider<TNumber>>, IRaycastWriteOnlyContainer<TNumber> where TNumber : struct, INumber<TNumber>
    {
        public ConcreteRaycastHit<TNumber, IRaycastCollider<TNumber>> Raycast(Vector3<TNumber> from, Vector3<TNumber> direction)
        {
            throw new NotImplementedException();
        }
    }
}
