using System;
using System.Collections.Generic;
using System.IO;

namespace FluentBuild
{
    public interface IFileSystemWrapper
    {
        void Copy(string source, string destination);
        bool FileExists(string path);
        void WriteAllText(string destination, string input);
        string ReadAllText(string path);
        IEnumerable<string> GetDirectories(string directory);
    }

    public class FileSystemWrapper : IFileSystemWrapper
    {
        public void Copy(string source, string destination)
        {
            File.Copy(source,destination);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void WriteAllText(string destination, string input)
        {
            File.WriteAllText(destination, input);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public IEnumerable<string> GetDirectories(string directory)
        {
            return Directory.GetDirectories(directory);
        }
    }
}