using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.FilesAndDirectories
{
    ///<summary>
    /// Moves a build artifact to a destination
    ///</summary>
    public class MoveBuildArtifact : Failable<MoveBuildArtifact>
    {
        private readonly BuildArtifact _buildArtifact;
        private readonly IFileSystemWrapper _fileSystemWrapper;


        internal MoveBuildArtifact(IFileSystemWrapper fileSystemWrapper, BuildArtifact buildArtifact)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _buildArtifact = buildArtifact;
        }

        internal MoveBuildArtifact(BuildArtifact buildArtifact)
            : this(new FileSystemWrapper(), buildArtifact)
        {
        }

        ///<summary>
        /// Moves a file to a destination
        ///</summary>
        ///<param name="destination">the new location of the file</param>
        public BuildArtifact To(string destination)
        {
            if (_fileSystemWrapper.DirectoryExists(destination))
            {
                destination = Path.Combine(destination, _buildArtifact.FileName());
            }

            FailableActionExecutor.DoAction(OnError, _fileSystemWrapper.MoveFile, _buildArtifact.ToString(), destination);
            _buildArtifact.Path = destination;
            return _buildArtifact;
        }

        ///<summary>
        /// Moves a file to a destination
        ///</summary>
        ///<param name="destination">the new location of the file</param>
        public BuildArtifact To(BuildArtifact destination)
        {
            return To(destination.ToString());
        }

        ///<summary>
        /// Moves a file to a destination
        ///</summary>
        ///<param name="destination">the new location of the file</param>
        public BuildArtifact To(BuildFolder destination)
        {
            return To(destination.ToString());
        }
    }
}