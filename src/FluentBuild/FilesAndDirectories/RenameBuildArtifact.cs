using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.FilesAndDirectories
{
    ///<summary>
    /// Renames a build artifact
    ///</summary>
    public class RenameBuildArtifact : Failable<RenameBuildArtifact>
    {
        private readonly BuildArtifact _buildArtifact;
        private readonly IFileSystemWrapper _fileSystemWrapper;


        internal RenameBuildArtifact(IFileSystemWrapper fileSystemWrapper, BuildArtifact buildArtifact)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _buildArtifact = buildArtifact;
        }

        internal RenameBuildArtifact(BuildArtifact buildArtifact) : this(new FileSystemWrapper(), buildArtifact)
        {
        }

        ///<summary>
        /// Renames a file to a destination
        ///</summary>
        ///<param name="newName">the new name of the file</param>
        public BuildArtifact To(string newName)
        {
            var newPath = Path.GetDirectoryName(_buildArtifact.ToString()) + "\\" + newName;
            FailableActionExecutor.DoAction(OnError, _fileSystemWrapper.MoveFile, _buildArtifact.ToString(), newPath);
            _buildArtifact.Path = newPath;
            return _buildArtifact;
        }
    }
}