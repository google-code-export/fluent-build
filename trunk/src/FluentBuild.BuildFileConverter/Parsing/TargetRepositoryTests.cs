using FluentBuild.BuildFileConverter.Structure;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class TargetRepositoryTests
    {
        private TargetRepository _subject;

        [SetUp]
        public void SetUp()
        {
            TargetRepository.ClearKnownTargets();
            _subject = new TargetRepository();
            var target = new Target { Name = "test" };
            _subject.Register(target);

        }

        [Test]
        public void ShouldFindByName()
        {
            var target = _subject.Resolve("test");
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.TypeOf<Target>());
        }

        [Test]
        public void ShouldReturnProxyToTarget()
        {
            var target = _subject.Resolve("unregisteredTarget");
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.TypeOf<UnregisteredTarget>());
        }

        [Test, Ignore("need a checkin")]
        public void ShouldHaveUnregisteredDependancyThenShouldHaveRegisteredDepenancy()
        {
            //create a "compile" target that depends on a "clean" target that has yet to be parsed
            var compileTarget = new Target() {Name ="compile"};
            compileTarget.DependsOn.Add(_subject.Resolve("clean"));
            _subject.Register(compileTarget);
            
            //ensure that we get a proxy to unregisteredTarget
            Assert.That(compileTarget.DependsOn[0], Is.TypeOf<UnregisteredTarget>());
            
            var cleanTarget = new Target() {Name = "clean"};
            _subject.Register(cleanTarget);
            Assert.That(compileTarget.DependsOn[0], Is.TypeOf<Target>());
        }
    }
}