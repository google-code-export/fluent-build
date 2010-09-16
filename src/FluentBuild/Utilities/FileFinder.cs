using System;
using System.Collections.Generic;
using System.IO;
using FluentBuild.FilesAndDirectories;

namespace FluentBuild.Utilities
{
    public interface IFileFinder
    {
        string Find(string fileName, string directory);
        string Find(string fileName);
    }

    public class FileFinder : IFileFinder
    {
        private readonly IFileSystemWrapper _fileSystem;

        public FileFinder(IFileSystemWrapper fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public FileFinder() : this(new FileSystemWrapper())
        {
        }

        public string Find(string fileName, string directory)
        {
            IEnumerable<string> filesInDirectory = _fileSystem.GetFilesIn(directory);
            if (filesInDirectory != null)
            {
                foreach (string file in filesInDirectory)
                {
                    if (Path.GetFileName(file) == fileName)
                    {
                        return file;
                    }
                }
            }


            IEnumerable<string> subDirectories = _fileSystem.GetDirectories(directory);
            if (subDirectories != null)
            {
                foreach (string subDirectory in subDirectories)
                {
                    var find = Find(fileName, subDirectory);
                    if (find!= null)
                        return find;
                }
            }
            return null;
        }

        public string Find(string fileName)
        {
            return Find(fileName, Environment.CurrentDirectory);
        }
    }
}