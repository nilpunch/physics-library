using System;
using System.Collections.Generic;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class SubPhysicWorld<TBody> : IPhysicWorld<TBody>
    {
        private readonly IPhysicWorld<TBody> _parent;
        private readonly List<TBody> _localBodies;

        public SubPhysicWorld(IPhysicWorld<TBody> parent)
        {
            _parent = parent;
            _localBodies = new List<TBody>();
        }

        public void Add(TBody body)
        {
            _parent.Add(body);
            _localBodies.Add(body);
        }

        public void Remove(TBody body)
        {
            _parent.Remove(body);
            _localBodies.Remove(body);
        }

        public RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
