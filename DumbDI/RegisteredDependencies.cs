using System;
using System.Collections.Generic;

namespace DumbDI
{
    internal sealed class RegisteredDependencies
    {
        private class RegisteredDependency
        {
            public bool Registered { get; set; } = false;
            public DependencyCollection Collection { get; } = new DependencyCollection();
        }

        private Dictionary<Type, RegisteredDependency> registeredTypes = new Dictionary<Type, RegisteredDependency>();

        public bool HasRegisteredType(Type t)
        {
            if (registeredTypes.TryGetValue(t, out RegisteredDependency dependency))
            {
                return dependency.Registered;
            }

            return false;
        }

        public DependencyCollection GetCollection(Type t)
        {
            if(registeredTypes.TryGetValue(t, out var dependency))
            {
                return dependency.Collection;
            }

            return null;
        }

        public DependencyCollection GetOrAddCollection(Type t, bool register)
        {
            if(registeredTypes.TryGetValue(t, out var dependency))
            {
                if(register)
                {
                    dependency.Registered = true;
                }
                return dependency.Collection;
            }

            dependency = new RegisteredDependency()
            {
                Registered = register
            };

            registeredTypes.Add(t, dependency);

            return dependency.Collection;
        }
    }
}
