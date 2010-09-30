using System.Collections.Generic;
using FluentBuild.FilesAndDirectories.FileSet;

namespace FluentBuild.Core
{
    ///<summary>
    /// Represents a FileSet
    ///</summary>
    public interface IFileSet
    {

        ///<summary>
        /// 
        ///</summary>
        IList<string> Files { get; }
        CopyFileset Copy { get; }
        FileSet Include(BuildArtifact path);
        FileSet Include(string path);
        FileSet Exclude(string path);
    }

    public class FileSet : IFileSet
    {
        private readonly List<string> _exclusions = new List<string>();
        private readonly List<string> _files = new List<string>();
        private readonly IFileSystemUtility _utility;

        public FileSet() : this(new FileSystemUtility())
        {
        }

        internal FileSet(IFileSystemUtility utility)
        {
            _utility = utility;
        }

        //TODO: Should this not be a read only collection?

        #region IFileSet Members

        public IList<string> Files
        {
            get
            {
                foreach (string exclusion in _exclusions)
                {
                    _files.Remove(exclusion);
                }
                return _files;
            }
        }

        public FileSet Include(BuildArtifact path)
        {
            return Include(path.ToString());
        }

        public FileSet Include(string path)
        {
            if (path.IndexOf('*') == -1)
                _files.Add(path);
            else
                _files.AddRange(_utility.GetAllFilesMatching(path));
            return this;
        }

        //TODO: should take a buildArtifact/Folder
        public FileSet Exclude(string path)
        {
            if (path.IndexOf('*') == -1)
                _exclusions.Add(path);
            else
                _exclusions.AddRange(_utility.GetAllFilesMatching(path));
            return this;
        }

        public CopyFileset Copy
        {
            get { return new CopyFileset(this); }
        }

        #endregion
    }
}