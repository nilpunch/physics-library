using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class SubPhysicWorld<TBody, TCollider> : IPhysicWorld<TBody> where TBody : IPhysicalBody<TCollider>
    {
        private readonly IPhysicWorld<TBody> _parent;
        private readonly List<TBody> _localBodies;

        public SubPhysicWorld(IPhysicWorld<TBody> parent)
        {
            _parent = parent;
            _localBodies = new List<TBody>();
        }

        public RaycastHit<TBody> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
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
    }

    public class SubPhysicWorld<TBody, TCollider, TConcrete> : IPhysicWorld<ConcreteBody<TBody, TConcrete>>,
        ICollisionsWorld<TConcrete> where TBody : IPhysicalBody<TCollider>
    {
        private readonly List<ConcreteBody<TBody, TConcrete>> _concreteBodies = new();

        private readonly IPhysicWorld<TBody> _parent;

        public SubPhysicWorld(IPhysicWorld<TBody> parent)
        {
            _parent = parent;
        }

        public void Add(ConcreteBody<TBody, TConcrete> concreteBody)
        {
            _parent.Add(concreteBody.Body);
            _concreteBodies.Add(concreteBody);
        }

        public void Remove(ConcreteBody<TBody, TConcrete> concreteBody)
        {
            _parent.Remove(concreteBody.Body);
            _concreteBodies.RemoveAll(physicalLink => ReferenceEquals(physicalLink.Body, concreteBody.Body));
        }

        public RaycastHit<ConcreteBody<TBody, TConcrete>> Raycast(SoftVector3 from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }

        RaycastHit<TConcrete> ICollisionsWorld<TConcrete>.Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
