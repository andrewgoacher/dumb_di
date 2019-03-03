using System;

namespace DumbDI
{
    public sealed class UnregisteredDependencyException : Exception
    {
        public Type ResolvingType { get; }
        public Type MissingType { get; }

        public UnregisteredDependencyException(Type resolving, Type missing)
        {
            ResolvingType = resolving;
            MissingType = missing;
        }
    }
}
