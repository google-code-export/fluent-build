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
        internal List<string> Exclusions = new List<string>();
        internal List<string> Inclusions = new List<string>();
        
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
                var files = new List<string>();
                files.AddRange(DetermineActualFiles(Inclusions));
                foreach (var exclusion in DetermineActualFiles(Exclusions))
                {
                    files.Remove(exclusion);
                }
                return files.AsReadOnly();
            }
        }

        internal IEnumerable<string> DetermineActualFiles(List<string> input)
        {
            foreach (var path in input)
            {
                if (path.IndexOf('*') == -1)
                    yield return path;
                else
                {
                    var allFilesMatching = _utility.GetAllFilesMatching(path);
                    if (allFilesMatching != null)
                    {
                        foreach (var match in allFilesMatching)
                        {
                            yield return match;
                        }
                    }
                }
            }
        }

        protected internal string PendingInclude;
        protected internal string PendingExclude;

        public BuildFolderChoices Include(BuildFolder path)
        {
            ProcessPendings();
            PendingInclude = path.ToString();
            return new BuildFolderChoices(this, _utility, true);
        }

        public BuildFolderChoices Exclude(BuildFolder path)
        {
            ProcessPendings();
            PendingExclude = path.ToString();
            return new BuildFolderChoices(this, _utility, false);
        }


        public FileSet Include(BuildArtifact path)
        {
            return Include(path.ToString());
        }

        public FileSet Include(string path)
        {
            ProcessPendings();
            Inclusions.Add(path);
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
            Exclusions.Add(path);
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