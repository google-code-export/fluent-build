using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FluentBuild
{
    public interface IFileSystemUtility
    {
        IList<string> GetAllFilesMatching(string filter);
    }

    public class FileSystemUtility : IFileSystemUtility
    {
        #region IFileSystemUtility Members

        public IList<string> GetAllFilesMatching(string filter)
        {
            //a full file i.e. c:\temp\file1.txt
            if ((filter.LastIndexOf("*") == -1) && (Path.HasExtension(filter)))
            {
                var list = new List<String>();
                if (File.Exists(filter))
                    list.Add(filter);
                return list;
            }
            //TODO: remove this coupling
            var parser = new SearchPatternParser(filter);
            return GetAllFilesMatching(parser.Folder, parser.SearchPattern, parser.Recursive);
        }

        #endregion

        private IList<String> GetAllFilesMatching(string directory, string filter, bool recursive)
        {
            string[] files = Directory.GetFiles(directory, filter);
            List<string> matching = files.ToList();
            if (recursive)
            {
                foreach (string subDirectory in Directory.GetDirectories(directory))
                {
                    matching.AddRange(GetAllFilesMatching(subDirectory, filter, true));
                }
            }
            return matching;
        }
    }
}