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
            DependencyCollection registeredTypeCollection = registeredTypes[type];
            var paramaters = new List<object>();

            foreach (Type t in registeredTypeCollection)
            {
                try
                {
                    paramaters.Add(Resolve(t));
                }
                catch (UnregisteredDependencyException)
                {
                    throw new UnregisteredDependencyException(type, t);
                }
            }

            return Activator.CreateInstance(type, paramaters.ToArray());
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
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
