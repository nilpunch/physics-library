using System;
using System.Collections.Generic;
using System.Linq;
using GameLibrary.Mathematics;
using GameLibrary.Physics.SupportMapping;

namespace GameLibrary.Physics
{
    public class ConcreteSubPhysicWorld<TBody, TConcrete> : IPhysicWorld<ConcreteBody<TBody, TConcrete>>, IRaycastWorld<TConcrete>
    {
        private readonly List<ConcreteBody<TBody, TConcrete>> _concreteBodies = new();

        private readonly IPhysicWorld<TBody> _parent;

        public ConcreteSubPhysicWorld(IPhysicWorld<TBody> parent)
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

        RaycastHit<TConcrete> IRaycastWorld<TConcrete>.Raycast(SoftVector3 @from, SoftVector3 direction)
        {
            throw new NotImplementedException();
        }
    }
}
