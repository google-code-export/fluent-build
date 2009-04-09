using System;
using System.Collections.Generic;
using System.IO;

namespace FluentBuild
{
    public class FileSet
    {
        private readonly List<string> exclusions = new List<string>();
        private readonly List<string> files = new List<string>();
        private readonly IFileSystemUtility utility;

        public FileSet() : this(new FileSystemUtility())
        {
        }

        internal FileSet(IFileSystemUtility utility)
        {
            this.utility = utility;
        }

        public IList<string> Files
        {
            get
            {
                foreach (string exclusion in exclusions)
                {
                    files.Remove(exclusion);
                }
                return files;
            }
        }

        public FileSet Include(BuildArtifact path)
        {
            return Include(path.ToString());
        }

        public FileSet Include(string path)
        {
            if (path.IndexOf('*') == -1)
                files.Add(path);
            else
                files.AddRange(utility.GetAllFilesMatching(path));
            return this;
        }

        public FileSet Exclude(string path)
        {
            if (path.IndexOf('*') == -1)
                exclusions.Add(path);
            else
                exclusions.AddRange(utility.GetAllFilesMatching(path));
            return this;
        }

        public FileSet CopyTo(BuildFolder destination)
        {
            MessageLogger.Write("copy", String.Format("Copying {0} files to '{1}'", Files.Count, destination.ToString()));
            foreach (string file in Files)
            {
                File.Copy(file, Path.Combine(destination.ToString(), Path.GetFileName(file)));
            }
            return this;
        }
    }
}