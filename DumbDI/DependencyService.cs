using System;
using System.Collections;
using System.Collections.Generic;

namespace DumbDI
{
    public class DependencyCollection : IEnumerable<Type>
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

    public sealed class DependencyService
    {
        private Dictionary<Type, DependencyCollection> registeredTypes 
            = new Dictionary<Type, DependencyCollection>();

        private object Resolve(Type type)
        {
            if (!registeredTypes.ContainsKey(type))
            {
                throw new UnregisteredDependencyException();
            }
            var registeredTypeCollection = registeredTypes[type];
            var paramaters = new List<Object>();

            foreach (var t in registeredTypeCollection)
            {
                paramaters.Add(Resolve(t));
            }

            return Activator.CreateInstance(type, paramaters.ToArray());
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));  
        }

        public void Register<T>()
        {
            var collection = new DependencyCollection();
            var type = typeof(T);

            var constructor = type.GetConstructors()[0];
            foreach (var t in constructor.GetParameters())
            {
                collection.AddNode(t.ParameterType);
            }

            registeredTypes.Add(type, collection);
        }
    }
}
