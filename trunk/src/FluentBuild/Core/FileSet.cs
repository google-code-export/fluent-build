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

        ///<summary>
        /// Sets the include to be a folder and opens additional options
        ///</summary>
        ///<param name="path">The buildfolder representing the path to be used</param>
        BuildFolderChoices Include(BuildFolder path);
        
        ///<summary>
        /// Sets the exclude to be a folder and opens additional options
        ///</summary>
        ///<param name="path">The buildfolder representing the path to be used</param>
        BuildFolderChoices Exclude(BuildFolder path);
    }

    ///<summary>
    /// Represents a set of files with include and exclude filters
    ///</summary>
    public class FileSet : IFileSet
    {
        internal readonly List<string> Exclusions = new List<string>();
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
                ProcessPendings();
                foreach (string exclusion in Exclusions)
                {
                    _files.Remove(exclusion);
                }
                return _files.AsReadOnly();
            }
        }

        
        protected internal string PendingInclude;
        protected internal string PendingExclude;

        public BuildFolderChoices Include(BuildFolder path)
        {
            PendingInclude = path.ToString();
            return new BuildFolderChoices(this);
        }

        public BuildFolderChoices Exclude(BuildFolder path)
        {
            PendingExclude = path.ToString();
            return new BuildFolderChoices(this);
        }


        public FileSet Include(BuildArtifact path)
        {
            return Include(path.ToString());
        }

        public FileSet Include(string path)
        {
            ProcessPendings();

            if (path.IndexOf('*') == -1)
                _files.Add(path);
            else
                _files.AddRange(_utility.GetAllFilesMatching(path));
            return this;
        }

        
        internal void ProcessPendings()
        {
            if (!string.IsNullOrEmpty(PendingExclude))
            {
                var tmp = PendingExclude;
                PendingExclude = string.Empty;
                Exclude(tmp);
            }

            if (!string.IsNullOrEmpty(PendingInclude))
            {
                var tmp = PendingInclude;
                PendingInclude = string.Empty;
                Include(tmp);
            }
        }

        public FileSet Exclude(string path)
        {
            ProcessPendings();

            if (path.IndexOf('*') == -1)
                Exclusions.Add(path);
            else
                Exclusions.AddRange(_utility.GetAllFilesMatching(path));
            return this;
        }

        public CopyFileset Copy
        {
            get
            {
                ProcessPendings();
                return new CopyFileset(this);
            }
        }

        #endregion
    }
}