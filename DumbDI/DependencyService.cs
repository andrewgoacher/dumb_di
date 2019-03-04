using System;
using System.Collections.Generic;

namespace DumbDI
{
    public sealed class DependencyService
    {
        private Dictionary<Type, DependencyCollection> registeredTypes
            = new Dictionary<Type, DependencyCollection>();

        private object Resolve(Type type)
        {
            if (!registeredTypes.ContainsKey(type))
            {
                throw new UnregisteredDependencyException(type, type);
            }

            if (HasCircularDependencies(type))
            {
                throw new CircularDependencyException();
            }

            DependencyCollection registeredTypeCollection = registeredTypes[type];
            var paramaters = new List<object>();

            foreach (Type t in registeredTypeCollection)
            {
                paramaters.Add(Resolve(t));
            }

            return Activator.CreateInstance(type, paramaters.ToArray());
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private IList<Type> GetAllTypesInChain(Type initialType, Type t, IList<Type> typeCollection)
        {
            if (!registeredTypes.TryGetValue(t, out DependencyCollection collection))
            {
                throw new UnregisteredDependencyException(initialType, t);
            }

            var itemsToAdd = new List<Type>();
            foreach (Type type in collection)
            {
                if (!typeCollection.Contains(type))
                {
                    typeCollection.Add(type);
                    IList<Type> items = GetAllTypesInChain(initialType, type, typeCollection);
                    itemsToAdd.AddRange(items);
                }
            }

            foreach (Type i in itemsToAdd)
            {
                typeCollection.Add(i);
            }
            return typeCollection;
        }

        private bool HasCircularDependencies(Type t)
        {
            IList<Type> types = GetAllTypesInChain(t, t, new List<Type>());
            return types.Contains(t);
        }

        public void Register<T>()
        {
            if (registeredTypes.ContainsKey(typeof(T)))
            {
                throw new AlreadyRegisteredDependencyException();
            }

            var collection = new DependencyCollection();
            Type type = typeof(T);

            System.Reflection.ConstructorInfo constructor = type.GetConstructors()[0];
            foreach (System.Reflection.ParameterInfo t in constructor.GetParameters())
            {
                collection.AddNode(t.ParameterType);
            }

            registeredTypes.Add(type, collection);
        }
    }
}
