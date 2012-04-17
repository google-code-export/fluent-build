using FluentBuild.Runners.Zip;

namespace FluentBuild
{
    public interface IZipOptions
    {
        IZipCompress Compress { get; }
        ZipDecompress Decompress(string path);
    }

    public class ZipOptions : IZipOptions
    {
        public IZipCompress Compress
        {
            get { return new ZipCompress(); }
        }

        public ZipDecompress Decompress(string path)
        {
            return new ZipDecompress(path);
        }
    }
}