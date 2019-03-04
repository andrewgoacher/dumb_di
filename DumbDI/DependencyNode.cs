using System;

namespace DumbDI
{
    internal sealed class DependencyNode
    {
        public DependencyNode(Type t, DependencyCollection collection)
        {
            Type = t;
            DependencyCollection = collection;
        }

        public Type Type { get; }
        public DependencyCollection DependencyCollection { get; }
    }
}
