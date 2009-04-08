using System.IO;

namespace FluentBuild
{
    public class DirectoryUtility
    {
        public static void RecreateDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        public static void CopyAllFiles(string from, string to)
        {
            foreach (string file in Directory.GetFiles(from))
            {
                File.Copy(file, Path.Combine(to, Path.GetFileName(file)));
            }
        }

        public static void CopyAllFiles(FileSet from, string to)
        {
            foreach (string file in from.Files)
            {
                File.Copy(file, Path.Combine(to, Path.GetFileName(file)));
            }
        }
    }
}