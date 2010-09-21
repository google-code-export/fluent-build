using FluentBuild.Core;
using FluentBuild.Tokenization;
using NUnit.Framework;

using Rhino.Mocks;

namespace FluentBuild.FilesAndDirectories
{
    [TestFixture]
    public class CopyBuildArtifactTests
    {
        private BuildArtifact _artifact;
        private IFileSystemWrapper _fileSystemWrapper;
        private CopyBuildArtifcat _copyEngine;
        private string _source;
        private string _destination;

        [SetUp]
        public void Setup()
        {
            _source = @"c:\temp\nonexistant.txt";
            _destination = @"c:\temp\nonexistant2.txt";

            _artifact = new BuildArtifact(_source);
            _fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            _copyEngine = new CopyBuildArtifcat(_fileSystemWrapper, _artifact);
        }

        [Test]
        public void StringCopyShouldRenameFile()
        {
            _copyEngine.To(_destination);
            _fileSystemWrapper.AssertWasCalled(x=>x.Copy(_source, _destination));
        }

        [Test]
        public void ArtifactCopyShouldRenameFile()
        {
            var destinationArtifact = new BuildArtifact(_destination);
            _copyEngine.To(destinationArtifact);
            _fileSystemWrapper.AssertWasCalled(x => x.Copy(_source, _destination));
        }

        [Test]
        public void BuildFolderCopyShouldMoveToNewFolder()
        {
            var buildFolder = new BuildFolder(@"c:\sample");
            _copyEngine.To(buildFolder);
            _fileSystemWrapper.AssertWasCalled(x => x.Copy(_source, buildFolder.ToString() + "\\nonexistant.txt"));
        }

        [Test]
        public void PerformTokenReplacement()
        {
            _fileSystemWrapper.Stub(x => x.ReadAllText(_artifact.ToString())).Return("hello @Bills@ ya'll");
            TokenReplacer tokenReplacer = _copyEngine.ReplaceToken("Bills").With("dolla dolla bills");
            Assert.That(tokenReplacer.ToString(), Is.EqualTo("hello dolla dolla bills ya'll"));
        }

    }
}