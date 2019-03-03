using System;
using System.Collections;
using System.Collections.Generic;

namespace DumbDI
{
    internal sealed class DependencyCollection : IEnumerable<Type>
    {
        private List<Type> nodes = new List<Type>();

        public void AddNode(Type type)
        {
            nodes.Add(type);
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
