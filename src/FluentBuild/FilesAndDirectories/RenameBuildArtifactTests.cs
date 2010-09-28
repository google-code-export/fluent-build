using System.IO;
using FluentBuild.Core;
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
            fileSystemWrapper.AssertWasCalled(x=>x.MoveFile(origin, "c:\\\\" + destination));
        }

        [Test, ExpectedException(typeof(IOException))]
        public void ShouldFailOnError()
        {
            var buildArtifact = new BuildArtifact("c:\\nonexistant.txt");
            
            var fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            var subject = new RenameBuildArtifact(fileSystemWrapper, buildArtifact);

            fileSystemWrapper.Stub(x => x.MoveFile("", "")).IgnoreArguments().Throw(new IOException("Could not do that"));
            subject.FailOnError.To("nonexistant2.txt");
            
        }

        [Test]
        public void ShouildContinueOnError()
        {
            var buildArtifact = new BuildArtifact("c:\\nonexistant.txt");

            var fileSystemWrapper = MockRepository.GenerateMock<IFileSystemWrapper>();
            var subject = new RenameBuildArtifact(fileSystemWrapper, buildArtifact);

            fileSystemWrapper.Stub(x => x.MoveFile("", "")).IgnoreArguments().Throw(new IOException("Could not do that"));
            subject.ContinueOnError.To("nonexistant2.txt");
        }
    }
}