using System;
using System.IO;

namespace FluentBuild
{
    public class CopyFrom
    {
        private readonly string _from;

        internal CopyFrom(string path)
        {
            this._from = path;
        }

        public void To(string path)
        {
            MessageLogger.Write("copy", String.Format("Copying all files from '{0}' to '{1}'", _from, path));
            foreach (string file in Directory.GetFiles(_from))
            {
                File.Copy(file, Path.Combine(path, Path.GetFileName(file)));
            }
        }
    }
}