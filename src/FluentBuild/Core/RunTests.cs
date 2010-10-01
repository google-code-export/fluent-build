using FluentBuild.Runners.Zip;
using NUnit.Framework;

namespace FluentBuild.Core
{
    ///<summary />	[TestFixture]
    public class RunTests
    {
        ///<summary />	[Test]
        public void Run_Should_Populate_Exe()
        {
            const string exe = "temp.exe";
            var executeable = Run.Executeable(exe);
            Assert.That(executeable.ExecuteablePath, Is.EqualTo(exe));
        }

        ///<summary />	[Test]
        public void Run_Should_Populate_Exe_When_Using_Build_Artifact()
        {
            var exe = new BuildArtifact("temp.exe");
            var executeable = Run.Executeable(exe);
            Assert.That(executeable.ExecuteablePath, Is.EqualTo(exe.ToString()));
        }

        ///<summary />	[Test]
        public void UnitTestFramework_ShouldCreateObject()
        {
            var framework = Run.UnitTestFramework;
            Assert.That(framework, Is.Not.Null);
        }

        ///<summary />	[Test]
        public void Zip_ShouldCreateObject()
        {
            var zip = Run.Zip;
            Assert.That(zip, Is.TypeOf<Zip>());
        }

    }
}