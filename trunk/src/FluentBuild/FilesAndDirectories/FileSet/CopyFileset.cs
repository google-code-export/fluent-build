using System;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    ///<summary>
    /// Copies the fileset
    ///</summary>
    public class CopyFileset : Failable<CopyFileset>
    {
        private readonly Core.FileSet _fileSet;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        internal CopyFileset(Core.FileSet fileSet, IFileSystemWrapper fileSystemWrapper)
        {
            _fileSet = fileSet;
            _fileSystemWrapper = fileSystemWrapper;
        }

        internal CopyFileset(Core.FileSet fileSet): this(fileSet, new FileSystemWrapper())
        {
        }

        ///<summary>
        /// Copies the fileset to the destination
        ///</summary>
        ///<param name="destination">A BuildFolder that will recieve the files</param>
        ///<returns></returns>
        public Core.FileSet To(BuildFolder destination)
        {
            return To(destination.ToString());
        }

        ///<summary>
        /// Copies the fileset to the destination
        ///</summary>
        ///<param name="destination">Path to a folder that will recieve the files</param>
        ///<returns></returns>
        public Core.FileSet To(string destination)
        {
            MessageLogger.Write("copy", String.Format("Copying {0} files to '{1}'", _fileSet.Files.Count, destination));
            foreach (string file in _fileSet.Files)
            {
                string destinationPath = Path.Combine(destination.ToString(), Path.GetFileName(file));
                FailableActionExecutor.DoAction<string, string>(this.OnError, _fileSystemWrapper.Copy, file, destinationPath);
            }
            return _fileSet;
        }

    }
}
