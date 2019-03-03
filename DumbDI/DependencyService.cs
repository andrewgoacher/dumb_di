using System;
using System.Collections.Generic;

namespace DumbDI
{
    public sealed class DependencyService
    {
        private List<Type> registeredTypes = new List<Type>();

        public T Resolve<T>()
        {
            if(!registeredTypes.Contains(typeof(T)))
            {
                throw new UnregisteredDependencyException();
            }
            return Activator.CreateInstance<T>();
        }

        public void Register<T>()
        {
            registeredTypes.Add(typeof(T));
        }
    }
}
