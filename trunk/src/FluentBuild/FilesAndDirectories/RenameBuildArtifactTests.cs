using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.FilesAndDirectories
{
    [TestFixture]
    public class RenameBuildArtifactTests
    {
        [Test]
        public void ShouldCallWrapperMoveFile()
        {
            string origin = "c:\\nonexistant.txt";
            string destination = "nonexistant2.txt";

            var buildArtifact = new BuildArtifact(origin);
            var fileSystemWrapper = MockRepository.GenerateMock<IFileSystemWrapper>();
            var subject = new RenameBuildArtifact(fileSystemWrapper, buildArtifact);
            
            subject.To(destination);
            fileSystemWrapper.AssertWasCalled(x=>x.MoveFile(origin, "c:\\" + destination));
        }
    }
}