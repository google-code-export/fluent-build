using System;
using System.IO;

namespace FluentBuild
{

    /// <summary>
    /// Represents a folder used in the build process.
    /// </summary>
    public class BuildFolder
    {
        private readonly string _path;

        public BuildFolder(string path)
        {
            _path = path;
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
        /// Deletes the folder if it exists. If it does not exist then no action is taken
        /// </summary>
        /// <returns>The current BuildFolder</returns>
        public BuildFolder Delete()
        {
            MessageLogger.Write("delete", _path);
            MessageLogger.BlankLine();

            if (Directory.Exists(_path))
                Directory.Delete(_path, true);
            return this;
        }

        /// <summary>
        /// Created a folder based on the BuildFolder path
        /// </summary>
        /// <returns>The current BuildFolder object</returns>
        public BuildFolder Create()
        {
            MessageLogger.Write("mkdir", _path);
            MessageLogger.BlankLine();
            Directory.CreateDirectory(_path);
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