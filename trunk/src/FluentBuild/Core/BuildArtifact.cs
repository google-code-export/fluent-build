using FluentBuild.FilesAndDirectories;
using FluentBuild.Utilities;

namespace FluentBuild.Core
{

    ///<summary>
    /// Represents a file used or created in a build
    ///</summary>
    public class BuildArtifact
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        internal string Path;
        

        ///<summary>
        ///</summary>
        ///<param name="path">Path to the file</param>
        public BuildArtifact(string path) : this(new FileSystemWrapper(), path)
        {
        }

        internal BuildArtifact(IFileSystemWrapper fileSystemWrapper, string path)
        {
            _fileSystemWrapper = fileSystemWrapper;
            Path = path;
        }

        ///<summary>
        /// Deletes the file 
        ///<remarks>If the file does not exist no error will be thrown (even if OnError is set to fail)</remarks>
        ///</summary>
        public void Delete()
        {
            Delete(Defaults.OnError);
        }


        ///<summary>
        /// Moves a build artifact to a new location
        ///</summary>
        public MoveBuildArtifact Move
        {
            get { return new MoveBuildArtifact(this);}
        }

        ///<summary>
        /// Returns only the file name of the build artifact
        ///</summary>
        ///<returns>The filename of the artifact</returns>
        public string FileName()
        {
            return System.IO.Path.GetFileName(Path);
        }

        ///<summary>
        /// Deletes the file 
        ///<remarks>If the file does not exist no error will be thrown (even if OnError is set to fail)</remarks>
        /// <param name="onError">Sets wether to fail or continue if an error occurs</param>
        ///</summary>
        public void Delete(OnError onError)
        {
            FailableActionExecutor.DoAction(onError, _fileSystemWrapper.DeleteFile, Path);
        }

        
        /// <summary>
        /// Renames the artifact
        /// </summary>
        public RenameBuildArtifact Rename
        {
            get { return new RenameBuildArtifact(this); }
        }

        
        /// <summary>
        /// Copies the artifact
        /// </summary>
        public CopyBuildArtifcat Copy
        {
            get {
                return new CopyBuildArtifcat(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The build artifact path</returns>
        public override string ToString()
        {
            return Path;
        }
    }
}