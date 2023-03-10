using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics.Raycast
{
    public class RaycastContainer : Container<IRaycastCollider>, IRaycastWriteOnlyContainer
    {
        public ConcreteRaycastHit<IRaycastCollider> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
