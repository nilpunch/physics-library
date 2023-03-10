using System.Collections.Generic;

namespace GameLibrary.Physics
{
    public class Container<T> : IContainer<T>
    {
        private readonly List<T> _list;

        public Container()
        {
            _list = new List<T>();
        }

        public IReadOnlyList<T> Items => _list;

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Remove(T item)
        {
            _list.Remove(item);
        }

        public void Clear()
        {
            _list.Clear();
        }
    }
}
