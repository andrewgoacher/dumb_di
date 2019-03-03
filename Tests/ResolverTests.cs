using DumbDI;
using NUnit.Framework;

namespace Tests
{
    public class ResolverTests
    {
        private class TestClassA { }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ResolveUnregisteredDependency_ThrowsException()
        {
            var service = new DependencyService();

            Assert.Throws<UnregisteredDependencyException>(() =>
            {
                service.Resolve<TestClassA>();
            });
        }

        [Test]
        public void ResolveRegisteredDependency_NoSubdependencies_ReturnsInstantiatedDependency()
        {
            var service = new DependencyService();
            service.Register<TestClassA>();

            var instance = service.Resolve<TestClassA>();

            Assert.NotNull(instance);
            Assert.IsInstanceOf<TestClassA>(instance);
        }
    }
}