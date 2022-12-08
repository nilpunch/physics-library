using System;
using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public class Rigidbody<TCollider> : IRigidbody<TCollider>
    {
        public Rigidbody(TCollider collider)
        {
            Collider = collider;
        }

        public SoftVector3 Velocity { get; set; }
        public SoftVector3 Position { get; set; }

        public TCollider Collider { get; }
    }
}
