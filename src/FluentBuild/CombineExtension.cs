using System.IO;

namespace FluentBuild
{
    public static class CombineExtension
    {
        public static string SubFolder(this string s1, string s2)
        {
            return Path.Combine(s1, s2);
        }

        public static string FileName(this string s1, string s2)
        {
            return Path.Combine(s1, s2);
        }

        public static string AllSubFolders(this string s1)
        {
            //pull off the trailing slash
            if (s1.Substring(s1.Length - 1, 1) == "\\")
            {
                s1 = s1.Substring(0, s1.Length - 1);
            }
            return s1 + @"\**\";
        }
    }
}