using System.IO;

namespace FluentBuild
{
    public class RenameBuildArtifact
    {
        private readonly BuildArtifact _buildArtifact;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public RenameBuildArtifact(IFileSystemWrapper fileSystemWrapper, BuildArtifact buildArtifact)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _buildArtifact = buildArtifact;
        }

        public RenameBuildArtifact(BuildArtifact buildArtifact) : this(new FileSystemWrapper(), buildArtifact)
        {
        }

        public BuildArtifact To(string newName)
        {
            //TODO: may have to determine folder of original build artifact for this to work
            
            _fileSystemWrapper.MoveFile(_buildArtifact.ToString(), Path.GetDirectoryName(_buildArtifact.ToString()) + "\\" + newName);
            return _buildArtifact;
        }
    }
}