using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.FilesAndDirectories
{
    public class RenameBuildArtifact
    {
        private readonly BuildArtifact _buildArtifact;
        private readonly IFileSystemWrapper _fileSystemWrapper;
        private OnError _onError;

        public RenameBuildArtifact(IFileSystemWrapper fileSystemWrapper, BuildArtifact buildArtifact)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _buildArtifact = buildArtifact;
        }

        public RenameBuildArtifact(BuildArtifact buildArtifact) : this(new FileSystemWrapper(), buildArtifact)
        {
        }

        //TODO: this may need to be centralized into a OnErrorBase<T> class
        public RenameBuildArtifact FailOnError
        {
            get
            {
                _onError = OnError.Continue;
                return this;
            }
        }

        public RenameBuildArtifact ContinueOnError
        {
            get
            {
                _onError = OnError.Continue;
                return this;
            }
        }

        public BuildArtifact To(string newName)
        {
            OnErrorActionExecutor.DoAction(_onError, _fileSystemWrapper.MoveFile, _buildArtifact.ToString(), Path.GetDirectoryName(_buildArtifact.ToString()) + "\\" + newName);
            return _buildArtifact;
        }
    }
}