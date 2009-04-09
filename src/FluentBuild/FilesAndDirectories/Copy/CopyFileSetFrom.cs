using System;
using System.IO;

namespace FluentBuild
{
    public class CopyFileSetFrom
    {
        private readonly FileSet set;

        internal CopyFileSetFrom(FileSet set)
        {
            this.set = set;
        }

        public void To(string path)
        {
            MessageLogger.Write("copy", String.Format("Copying {0} files to '{1}'", set.Files.Count, path));
            foreach (string file in set.Files)
            {
                File.Copy(file, Path.Combine(path, Path.GetFileName(file)));
            }
        }
    }
}