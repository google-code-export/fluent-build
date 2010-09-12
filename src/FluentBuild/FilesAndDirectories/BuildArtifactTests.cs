using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class BuildArtifactTests
    {
        [Test]
        public void ToString_Should_Output_Path()
        {
            const string path = @"c:\temp\test.txt";
            var artifact = new BuildArtifact(path);
            Assert.That(artifact.ToString(), Is.EqualTo(path));
        }

        [Test]
        public void Copy_ShouldCreateCopyObject()
        {
            const string path = @"c:\temp\test.txt";
            var artifact = new BuildArtifact(path);
            Assert.That(artifact.Copy, Is.Not.Null);
        }

    }
}