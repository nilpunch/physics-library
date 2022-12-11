using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class ConcreteSubPhysicWorld<TBody, TCollider, TConcrete> : IPhysicWorld<ConcreteBody<TBody, TConcrete>, TCollider>,
        ICollisionsWorld<TConcrete>
    {
        private readonly List<ConcreteBody<TBody, TConcrete>> _concreteBodies = new();

        private readonly IPhysicWorld<TBody, TCollider> _parent;

        public ConcreteSubPhysicWorld(IPhysicWorld<TBody, TCollider> parent)
        {
            _parent = parent;
        }

        public void Add(ConcreteBody<TBody, TConcrete> concreteBody, TCollider collider)
        {
            _parent.Add(concreteBody.Body, collider);
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
