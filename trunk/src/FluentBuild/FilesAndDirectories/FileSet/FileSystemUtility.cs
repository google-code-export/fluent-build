using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    public interface IFileSystemUtility
    {
        IList<string> GetAllFilesMatching(string filter);
    }

    internal class FileSystemUtility : IFileSystemUtility
    {
        private readonly ISearchPatternParser parser;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public FileSystemUtility(ISearchPatternParser parser, IFileSystemWrapper fileSystemWrapper)
        {
            this.parser = parser;
            _fileSystemWrapper = fileSystemWrapper;
        }

        public FileSystemUtility() : this(new SearchPatternParser(), new FileSystemWrapper())
        {
        }

        #region IFileSystemUtility Members

        public IList<string> GetAllFilesMatching(string filter)
        {
            //a full file i.e. c:\temp\file1.txt
            if ((filter.LastIndexOf("*") == -1) && (Path.HasExtension(filter)))
            {
                var list = new List<String>();
                if (_fileSystemWrapper.FileExists(filter))
                    list.Add(filter);
                return list;
            }
            parser.Parse(filter);
            return GetAllFilesMatching(parser.Folder, parser.SearchPattern, parser.Recursive);
        }

        #endregion

        private IList<String> GetAllFilesMatching(string directory, string filter, bool recursive)
        {
            string[] files = Directory.GetFiles(directory, filter);
            List<string> matching = files.ToList();
            if (recursive)
            {
                foreach (string subDirectory in _fileSystemWrapper.GetDirectories(directory))
                {
                    matching.AddRange(GetAllFilesMatching(subDirectory, filter, true));
                }
            }
            return matching;
        }
    }
}