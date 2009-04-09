namespace FluentBuild
{
    public class Copy
    {
        public static CopyFrom From(string path)
        {
            return new CopyFrom(path);
        }

        public static CopyFileSetFrom From(FileSet set)
        {
            return new CopyFileSetFrom(set);   
        }
    }
}