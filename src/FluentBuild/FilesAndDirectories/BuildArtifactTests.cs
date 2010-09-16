using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild.FilesAndDirectories
{
    [TestFixture]
    public class BuildArtifactTests
    {
        [Test]
        public void Delete_ShouldCallToFileSystemWrapper()
        {
            var fs = MockRepository.GenerateMock<IFileSystemWrapper>();
            string path = @"c:\temp\nonexistant.txt";
            var subject = new BuildArtifact(fs, path);
            subject.Delete();

            fs.AssertWasCalled(x=>x.DeleteFile(path));
        }

        [Test]
        public void Rename_ShouldBuildRenameObject()
        {
            var fs = MockRepository.GenerateMock<IFileSystemWrapper>();
            string path = @"c:\temp\nonexistant.txt";
            var subject = new BuildArtifact(fs, path);
            Assert.That(subject.Rename, Is.Not.Null);
        }

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