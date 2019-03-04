using System;
using System.Collections.Generic;

namespace DumbDI
{
    public sealed class DependencyService
    {
        private RegisteredDependencies registeredTypes = new RegisteredDependencies();

        private object Resolve(Type type)
        {
            if (!registeredTypes.HasRegisteredType(type))
            {
                throw new UnregisteredDependencyException(type, type);
            }

            if (HasCircularDependencies(type))
            {
                throw new CircularDependencyException();
            }

            DependencyCollection registeredTypeCollection = registeredTypes.GetCollection(type);
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

        public void Register<T>()
        {
            if (registeredTypes.HasRegisteredType(typeof(T)))
            {
                throw new AlreadyRegisteredDependencyException();
            }

            Type type = typeof(T);
            DependencyCollection collection = registeredTypes.GetOrAddCollection(type, true);

            System.Reflection.ConstructorInfo constructor = type.GetConstructors()[0];
            foreach (System.Reflection.ParameterInfo t in constructor.GetParameters())
            {
                DependencyCollection typeCollection = registeredTypes.GetOrAddCollection(t.ParameterType, false);
                collection.AddNode(t.ParameterType, typeCollection);
            }
        }

        private IList<Type> GetAllTypesInChain(Type initialType, Type t, IList<Type> typeCollection)
        {
            if (!registeredTypes.HasRegisteredType(t))
            {
                throw new UnregisteredDependencyException(initialType, t);
            }

            DependencyCollection collection = registeredTypes.GetCollection(t);

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
    }
}
