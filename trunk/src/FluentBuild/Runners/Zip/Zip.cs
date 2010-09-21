using FluentBuild.Core;

namespace FluentBuild.Runners.Zip
{
    public class Zip
    {
        internal Zip()
        {
        }

        public ZipCompress Compress { get { return new ZipCompress(); }}

        public ZipDecompress Decompress(string pathToArchive)
        {
            return new ZipDecompress(pathToArchive);
        }
    }
}