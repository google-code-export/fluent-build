using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace FluentBuild.Runners.Zip
{
    ///<summary>
    /// Zip decompresses an archive
    ///</summary>
    public class ZipDecompress
    {
        private readonly string _pathToArchive;
        private string _password;

        internal ZipDecompress(string pathToArchive)
        {
            _pathToArchive = pathToArchive;
        }

        ///<summary>
        /// Sets the password to decompress a file
        ///</summary>
        ///<param name="password">the password to use</param>
        public ZipDecompress UsingPassword(string password)
        {
            _password = password;
            return this;
        }

        ///<summary>
        /// Sets the output path
        ///</summary>
        ///<param name="outputPath">The path you would like the file(s) outputed to</param>
        public void To(string outputPath)
        {
            using (var zipInputStream = new ZipInputStream(System.IO.File.OpenRead(_pathToArchive)))
            {
                zipInputStream.Password = _password;

                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    FileStream streamWriter = System.IO.File.Create(Path.Combine(outputPath + "\\", entry.Name));
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