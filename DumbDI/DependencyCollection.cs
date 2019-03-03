using System;
using System.Collections;
using System.Collections.Generic;

namespace DumbDI
{
    internal sealed class DependencyCollection : IEnumerable<Type>
    {
        private LinkedList<Type> nodes = new LinkedList<Type>();

        public void AddNode(Type type)
        {
            nodes.AddLast(type);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
