using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentBuild
{
    public static class DirectoryExtensions
    {
        public static string Delete(this string path)
        {
            MessageLogger.Write("delete", path);
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return path;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
                return path;
            }

            throw new FileNotFoundException("File or directory not found (" + path +")");
        }

        public static string MakeDirectory(this string path)
        {
            MessageLogger.Write("Make Directory", path);
            Directory.CreateDirectory(path);
            return path;
        }
    }
    
}
