using FluentBuild.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.FilesAndDirectories
{
    [TestFixture]
    public class MoveBuildArtifactTests
    {
        private MoveBuildArtifact _subject;
        private IFileSystemWrapper _fileSystemWrapper;
        private BuildArtifact _buildArtifact;
        private string _destination;
        private string _source;

        [SetUp]
        public void SetUp()
        {
            _fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            _source = @"c:\temp\test.txt";
            _buildArtifact = new BuildArtifact(_source);
            _destination = @"c:\test.txt";
            _subject = new MoveBuildArtifact(_fileSystemWrapper, _buildArtifact);
        }


        [Test]
        public void ToShouldCallWrapper()
        {
            _subject.To(_destination);
            _fileSystemWrapper.AssertWasCalled(x=>x.MoveFile(_source, _destination));
        }

        [Test]
        public void ToShouldCallWrapperUsingBuildArtifact()
        {
            _subject.To(new BuildArtifact(_destination));
            _fileSystemWrapper.AssertWasCalled(x => x.MoveFile(_source, _destination));
        }

        [Test]
        public void ShouldCopyToDestinationAndFileNameWhenOnlyDestinationFolderUsed()
        {
            var folderDestination = "c:\\";
            _fileSystemWrapper.Stub(x => x.DirectoryExists(folderDestination)).Return(true);
            _subject.To(folderDestination);
            _fileSystemWrapper.AssertWasCalled(x => x.MoveFile(_source, _destination));
        }
    }
}