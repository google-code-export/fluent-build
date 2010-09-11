using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild.Compilation
{
    [TestFixture]
    public class Build_Tests
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
    }
}
