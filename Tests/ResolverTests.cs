using DumbDI;
using NUnit.Framework;

namespace Tests
{
    public class ResolverTests
    {
        private class TestClassA { }
        private class TestClassB
        {
            public TestClassB(TestClassA a)
            {

            }
        }

        private class TestClassC
        {
            public TestClassC(TestClassD d)
            {

            }
        }

        private class TestClassD
        {
            public TestClassD(TestClassC c)
            {

            }
        }

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

            TestClassA instance = service.Resolve<TestClassA>();

            Assert.NotNull(instance);
            Assert.IsInstanceOf<TestClassA>(instance);
        }

        [Test]
        public void ResolveRegisteredDependency_WithDependency_ReturnsInstantiatedObject()
        {
            var service = new DependencyService();
            service.Register<TestClassA>();
            service.Register<TestClassB>();

            TestClassB instance = service.Resolve<TestClassB>();

            Assert.NotNull(instance);
            Assert.IsInstanceOf<TestClassB>(instance);
        }

        [Test]
        public void ResolveRegisteredDependency_WithUnregisteredDependency_ThrowsException()
        {
            var service = new DependencyService();
            service.Register<TestClassB>();

            var exception = Assert.Throws<UnregisteredDependencyException>(() =>
            {
                service.Resolve<TestClassB>();
            });

            Assert.AreEqual(typeof(TestClassB), exception.ResolvingType);
            Assert.AreEqual(typeof(TestClassA), exception.MissingType);
        }

        [Test]
        public void RegisterDependency_AlreadyRegistered_ThrowsException()
        {
            var service = new DependencyService();
            service.Register<TestClassA>();

            Assert.Throws<AlreadyRegisteredDependencyException>(() =>
            {
                service.Register<TestClassA>();
            });
        }

        [Test]
        public void RegisterDependency_HasCircularDependency_ThrowsException()
        {
            var service = new DependencyService();
            service.Register<TestClassC>();
            service.Register<TestClassD>();

            Assert.Throws<CircularDependencyException>(() =>
            {
                service.Resolve<TestClassD>();
            });
        }
    }
}