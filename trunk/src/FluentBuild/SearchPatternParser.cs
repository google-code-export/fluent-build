using System.IO;
using System.Text.RegularExpressions;

namespace FluentBuild
{
    public class SearchPatternParser
    {
        private string searchPattern = "*.*";
        public SearchPatternParser(string pattern)
        {
            //no wildcards so just a folder i.e. c:\temp
            if (pattern.IndexOf("*") == -1)
            {
                Folder = pattern;
                return;
            }

            // c:\temp\auto*.cs
            var regex = new Regex(@"[a-zA-Z0-9]\*.");
            if (regex.IsMatch(pattern))
            {
                searchPattern = Path.GetFileName(pattern);
                Folder = pattern.Substring(0, pattern.LastIndexOf("\\")+1);
                if (Folder.IndexOf("\\**\\") >=0)
                {
                    Recursive = true;
                    Folder = Folder.Replace("\\**\\", "\\");
                }
            }
            else
            {
                searchPattern = pattern.Substring(pattern.IndexOf("*"));
                Folder = pattern.Substring(0, pattern.IndexOf("*"));

                if (searchPattern.IndexOf("**") >= 0)
                {
                    Recursive = true;
                    searchPattern = searchPattern.Substring(searchPattern.IndexOf("**") + 3);
                }
            }
        }

        public string SearchPattern
        {
            get { return searchPattern; }
            set { searchPattern = value; }
        }

        public string Folder { get; set; }

        public bool Recursive { get; set; }
    }
}