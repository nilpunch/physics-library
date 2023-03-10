using System.Collections.Generic;

namespace GameLibrary.Physics
{
    public interface IReadOnlyContainer<out T>
    {
        IReadOnlyList<T> Items { get; }
    }
}