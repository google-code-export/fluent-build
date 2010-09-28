using FluentBuild.Compilation;
using NUnit.Framework;

namespace FluentBuild.Core
{
    [TestFixture]
    public class Build_Tests
    {
        [Test]
        public void ShouldCreateTaskWithVbCompiler()
        {
            var build = Build.UsingVbc;
            Assert.That(build.Compiler, Is.EqualTo("vbc.exe"));
        }

        [Test]
        public void ShouldCreateTaskWithCSCCompiler()
        {
            var build = Build.UsingCsc;
            Assert.That(build.Compiler, Is.EqualTo("csc.exe"));
        }

        [Test]
        public void ShouldCreateMsBuildTask()
        {
            var build = Build.UsingMsBuild("c:\\mysln.sln");
            Assert.That(build, Is.TypeOf(typeof(MsBuildTask)));
        }
    }
}
