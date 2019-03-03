using System;

namespace DumbDI
{
    public sealed class UnregisteredDependencyException : Exception
    {
        public UnregisteredDependencyException()
        {
        }
    }
}
