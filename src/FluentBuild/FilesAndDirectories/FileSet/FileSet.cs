using System.Collections.Generic;

namespace FluentBuild
{
    public class FileSet
    {
        private readonly IFileSystemUtility utility;
        private readonly List<string> files = new List<string>();
        private readonly List<string> exclusions = new List<string>();

        public FileSet() : this(new FileSystemUtility())
        {
        }

        internal FileSet(IFileSystemUtility utility)
        {
            this.utility = utility;
        }

        public FileSet Include(string path)
        {
            if(path.IndexOf('*') == -1)
                files.Add(path);
            else
                files.AddRange(utility.GetAllFilesMatching(path));
            return this;
        }

        public IList<string> Files
        {
            get
            {
                foreach (var exclusion in exclusions)
                {
                    files.Remove(exclusion);
                }
                return files;
            }
        }

        public FileSet Exclude(string path)
        {
            if (path.IndexOf('*') == -1)
                exclusions.Add(path);
            else
                exclusions.AddRange(utility.GetAllFilesMatching(path));
            return this;
        }
    }
}