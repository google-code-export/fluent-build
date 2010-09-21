using NUnit.Framework;

namespace FluentBuild.Core
{
    [TestFixture]
    public class RunTests
    {
        [Test]
        public void Run_Should_Populate_Exe()
        {
            const string exe = "temp.exe";
            var executeable = Run.Executeable(exe);
            Assert.That(executeable._executeablePath, Is.EqualTo(exe));
        }

        [Test]
        public void Run_Should_Populate_Exe_When_Using_Build_Artifact()
        {
            var exe = new BuildArtifact("temp.exe");
            var executeable = Run.Executeable(exe);
            Assert.That(executeable._executeablePath, Is.EqualTo(exe.ToString()));
        }

        [Test]
        public void UnitTestFramework_ShouldCreateObject()
        {
            var framework = Run.UnitTestFramework;
            Assert.That(framework, Is.Not.Null);
        }

    }
}