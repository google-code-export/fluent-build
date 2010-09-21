using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentBuild.Runners.Zip
{
    public class ZipDecompress
    {
        private readonly string _pathToArchive;
        private string _password;

        public ZipDecompress(string pathToArchive)
        {
            _pathToArchive = pathToArchive;
        }

        public ZipDecompress UsingPassword(string password)
        {
            _password = password;
            return this;
        }

        public void To(string outputPath)
        {
            using (var zipInputStream = new ZipInputStream(File.OpenRead(_pathToArchive)))
            {
                zipInputStream.Password = _password;

                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    FileStream streamWriter = File.Create(Path.Combine(outputPath + "\\", entry.Name));
                    long size = entry.Size;
                    var data = new byte[size];
                    while (true)
                    {
                        size = zipInputStream.Read(data, 0, data.Length);
                        if (size > 0) streamWriter.Write(data, 0, (int) size);
                        else break;
                    }
                    streamWriter.Close();
                }
            }
        }
    }
}