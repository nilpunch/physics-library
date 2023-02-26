﻿using GameLibrary.Mathematics;

namespace GameLibrary.Physics
{
    public interface IRigidbody
    {
        SoftVector3 Velocity { get; set; }

        SoftVector3 Position { get; set; }
        SoftUnitQuaternion Rotation { get; set; }
        SoftFloat Mass { get; set; }

        bool IsStatic { get; set; }
    }
}
