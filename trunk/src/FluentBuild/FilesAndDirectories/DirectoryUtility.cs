using System.IO;

namespace FluentBuild
{
    public class DirectoryUtility
    {
        public static void RecreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                MessageLogger.Write("delete", path);
                Directory.Delete(path, true);
            }
            MessageLogger.Write("mkdir", path);
            Directory.CreateDirectory(path);
        }
    }
}