using System.IO;
using FluentBuild.FilesAndDirectories;
using FluentBuild.Utilities;

namespace FluentBuild.Core
{

    /// <summary>
    /// Represents a folder used in the build process.
    /// </summary>
    public class BuildFolder
    {
        private readonly IFileSystemWrapper _fileSystemWrapper;
        private readonly string _path;

        public BuildFolder(IFileSystemWrapper fileSystemWrapper, string path)
        {
            _fileSystemWrapper = fileSystemWrapper;
            _path = path;
        }

        public BuildFolder(string path) : this(new FileSystemWrapper(), path)
        {            
        }

        /// <summary>
        /// Creates a new BuildFolder object for a subdirectory
        /// </summary>
        /// <param name="path">The subfolder below the current BuildFolder</param>
        /// <returns>a new BuildFolder object</returns>
        /// <remarks>The folder does not need to exist to use this method</remarks>
        public BuildFolder SubFolder(string path)
        {
            return new BuildFolder(Path.Combine(_path, path));
        }


        /// <summary>
        /// Creates a new BuildFolder that encompases the current folder and all of its subdirectories
        /// </summary>
        /// <returns>A buildfolder that represents the current folder and all its subdirectories</returns>
        public BuildFolder RecurseAllSubFolders()
        {
            return new BuildFolder(_path + "\\**\\");
        }


        /// <summary>
        /// Deletes the folder. If the the folder can not be deleted, or does not exist then an exception is thrown.
        /// </summary>
        /// <returns></returns>
        public BuildFolder Delete()
        {
            return Delete(Defaults.OnError);
        }

        public BuildFolder Delete(OnError onError)
        {
            MessageLogger.WriteDebugMessage("Deleting " + _path);
            OnErrorActionExecutor.DoAction(onError, _fileSystemWrapper.DeleteDirectory, _path, true);
            return this;
        }

        public BuildFolder Create()
        {
            return Create(Defaults.OnError);
        }
       
        public BuildFolder Create(OnError onError)
        {
            MessageLogger.WriteDebugMessage("Create Direcotry " + _path);
            OnErrorActionExecutor.DoAction(onError, _fileSystemWrapper.CreateDirectory, _path);
            return this;
        }

        /// <summary>
        /// Provides the current path internal to the BuildFolder object
        /// </summary>
        /// <returns>The path of the BuildFolder</returns>
        public override string ToString()
        {
            return _path;
        }

        /// <summary>
        /// Appends a filename onto the BuildFolder
        /// </summary>
        /// <param name="name">The name (or filter) of the file</param>
        /// <returns>A BuildArtifact that represents the full path to the file</returns>
        public BuildArtifact File(string name)
        {
            return new BuildArtifact(Path.Combine(_path, name));
        }

        public FileSet Files(string filter)
        {
            var fileSet = new FileSet();
            return fileSet.Include(Path.Combine(_path, filter));
        }
    }
}