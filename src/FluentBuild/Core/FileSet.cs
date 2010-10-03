using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentBuild.FilesAndDirectories.FileSet;

namespace FluentBuild.Core
{
    ///<summary>
    /// Represents a FileSet
    ///</summary>
    public interface IFileSet
    {
        ///<summary>
        /// Returns a list of files that is contained within the fileset. 
        ///</summary>
        ReadOnlyCollection<string> Files { get; }

        ///<summary>
        /// Copies the fileset
        ///</summary>
        CopyFileset Copy { get; }

        ///<summary>
        /// Includes a path in the fileset.
        ///</summary>
        ///<param name="path">path to files to include</param>
        FileSet Include(BuildArtifact path);

        ///<summary>
        /// Includes a path in the fileset.
        ///</summary>
        ///<param name="path">path to files to include</param>
        FileSet Include(string path);

        ///<summary>
        /// Adds an exclude filter
        ///</summary>
        ///<param name="path">exclude path</param>
        FileSet Exclude(string path);
    }

    ///<summary>
    /// Represents a set of files with include and exclude filters
    ///</summary>
    public class FileSet : IFileSet
    {
        private readonly List<string> _exclusions = new List<string>();
        private readonly List<string> _files = new List<string>();
        private readonly IFileSystemUtility _utility;

        ///<summary>
        /// Creates a new fileset
        ///</summary>
        public FileSet() : this(new FileSystemUtility())
        {
        }

        internal FileSet(IFileSystemUtility utility)
        {
            _utility = utility;
        }



        #region IFileSet Members
        public ReadOnlyCollection<string> Files
        {
            get
            {
                foreach (string exclusion in _exclusions)
                {
                    _files.Remove(exclusion);
                }
                return _files.AsReadOnly();
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