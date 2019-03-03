using System;

namespace DumbDI
{
    public sealed class AlreadyRegisteredDependencyException : Exception
    {
        public AlreadyRegisteredDependencyException()
        {
        }
    }
}
