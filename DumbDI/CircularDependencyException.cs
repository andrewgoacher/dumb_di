using System;

namespace DumbDI
{
    public sealed class CircularDependencyException : Exception
    {
        public CircularDependencyException()
        {
        }
    }
}
