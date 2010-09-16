using System;
using System.IO;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    public class CopyFileset : Failable<CopyFileset>
    {
        private readonly FluentBuild.FileSet _fileSet;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public CopyFileset(FluentBuild.FileSet fileSet, IFileSystemWrapper fileSystemWrapper)
        {
            _fileSet = fileSet;
            _fileSystemWrapper = fileSystemWrapper;
        }

        public CopyFileset(FluentBuild.FileSet fileSet): this(fileSet, new FileSystemWrapper())
        {
        }

        public FluentBuild.FileSet To(BuildFolder destination)
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

        protected override CopyFileset GetSelf
        {
            get { return this; }
        }
    }
}
