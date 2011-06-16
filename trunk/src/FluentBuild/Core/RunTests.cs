using FluentBuild.Runners;
using FluentBuild.Runners.UnitTesting;
using FluentBuild.Runners.Zip;
using NUnit.Framework;

namespace FluentBuild.Core
{
    ///<summary />
    [TestFixture]
    public class RunTests
    {
        [Test]
        public void Run_ShouldCreateObject()
        {
            ILMerge merge = Run.ILMerge;
            Assert.That(merge, Is.TypeOf<ILMerge>());
        }

        ///<summary />
        [Test]
        public void Run_Should_Populate_Exe()
        {
            const string exe = "temp.exe";
            Executable executable = Run.Executable(exe);
            Assert.That(executable.Path, Is.EqualTo(exe));
        }

        ///<summary />
        [Test]
        public void Run_Should_Populate_Exe_When_Using_Build_Artifact()
        {
            var exe = new BuildArtifact("temp.exe");
            Executable executable = Run.Executable(exe);
            Assert.That(executable.Path, Is.EqualTo(exe.ToString()));
        }

        ///<summary />
        [Test]
        public void UnitTestFramework_ShouldCreateObject()
        {
            UnitTestFrameworkRun framework = Run.UnitTestFramework;
            Assert.That(framework, Is.Not.Null);
        }

        ///<summary />
        [Test]
        public void Zip_ShouldCreateObject()
        {
            Zip zip = Run.Zip;
            Assert.That(zip, Is.TypeOf<Zip>());
        }
    }
}