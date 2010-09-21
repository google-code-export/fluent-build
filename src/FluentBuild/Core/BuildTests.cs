using FluentBuild.Compilation;
using NUnit.Framework;

namespace FluentBuild.Core
{
    [TestFixture]
    public class BuildTests
    {
        [Test]
        public void ShouldCreateTaskWithVbCompiler()
        {
            var build = Build.UsingVbc;
            Assert.That(build.compiler, Is.EqualTo("vbc.exe"));
        }

        [Test]
        public void ShouldCreateTaskWithCSCCompiler()
        {
            var build = Build.UsingCsc;
            Assert.That(build.compiler, Is.EqualTo("csc.exe"));
        }

        [Test]
        public void ShouldCreateMsBuildTask()
        {
            var build = Build.UsingMsBuild("c:\\mysln.sln");
            Assert.That(build, Is.TypeOf(typeof(MsBuildTask)));
        }
    }
}
