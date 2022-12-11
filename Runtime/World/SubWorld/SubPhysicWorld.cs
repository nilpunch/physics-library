using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class SubPhysicWorld<TBody, TCollider> : IPhysicWorld<TBody, TCollider>
    {
        private readonly IPhysicWorld<TBody, TCollider> _parent;
        private readonly List<TBody> _localBodies;

        public SubPhysicWorld(IPhysicWorld<TBody, TCollider> parent)
        {
            _parent = parent;
            _localBodies = new List<TBody>();
        }

        public RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }

        public void Add(TBody body, TCollider collider)
        {
            _parent.Add(body, collider);
            _localBodies.Add(body);
        }

        public void Remove(TBody body)
        {
            _parent.Remove(body);
            _localBodies.Remove(body);
        }
    }
}
