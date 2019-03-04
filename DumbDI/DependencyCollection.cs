using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DumbDI
{

    internal sealed class DependencyCollection : IEnumerable<Type>
    {
        private List<DependencyNode> nodes = new List<DependencyNode>();

        public void AddNode(Type type, DependencyCollection collection)
        {
            nodes.Add(new DependencyNode(type, collection));
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return nodes.Select(x => x.Type).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
