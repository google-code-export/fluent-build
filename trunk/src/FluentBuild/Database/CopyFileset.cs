using System;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    public class CopyFileset : Failable<CopyFileset>
    {
        private readonly Core.FileSet _fileSet;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public CopyFileset(Core.FileSet fileSet, IFileSystemWrapper fileSystemWrapper)
        {
            _fileSet = fileSet;
            _fileSystemWrapper = fileSystemWrapper;
        }

        public CopyFileset(Core.FileSet fileSet): this(fileSet, new FileSystemWrapper())
        {
        }

        public Core.FileSet To(BuildFolder destination)
        {
            return To(destination.ToString());
        }

        public Core.FileSet To(string destination)
        {
            MessageLogger.Write("copy", String.Format("Copying {0} files to '{1}'", _fileSet.Files.Count, destination));
            MessageLogger.BlankLine();
            foreach (string file in _fileSet.Files)
            {
                string destinationPath = Path.Combine(destination.ToString(), Path.GetFileName(file));
                OnErrorActionExecutor.DoAction<string, string>(this.OnError, _fileSystemWrapper.Copy, file, destinationPath);
            }
            return _fileSet;
        }

    }
}
