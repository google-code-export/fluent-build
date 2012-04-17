using System.IO;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class ZipTests : TestBase
    {
        [Test]
        public void ShouldZipAndUnzipFile()
        {
            var fileContent = "this is only a test";
            var fileToCompress = rootFolder + "\\test.txt";
            var zipFilePath = rootFolder + "\\test.zip";
            var password = "testpass";


            using (var writer = new StreamWriter(fileToCompress))
            {
                writer.Write(fileContent);
                writer.Close();
            }
            
            Task.Run.Zip(x=>x.Compress.SourceFile(fileToCompress).UsingCompressionLevel.Five.UsingPassword(password).To(zipFilePath));
            File.Delete(fileToCompress);
            Task.Run.Zip(x=>x.Decompress(zipFilePath).UsingPassword(password).To(rootFolder));

            Assert.That(File.Exists(fileToCompress), "File did not exist");
            Assert.That(File.ReadAllText(fileToCompress), Is.EqualTo(fileContent));
        }

        [Test]
        public void ShouldZipAndUnzipFolder()
        {
            var folderToCompress = rootFolder + "\\TestCompression";
            Directory.CreateDirectory(folderToCompress);
            var zipFilePath = rootFolder + "\\test.zip";
            var password = "testpass";

            for (int i = 0; i < 5; i++)
            {
                var fileStream = File.Create(folderToCompress + "\\" + i + ".txt");
                fileStream.Close();
            }

            Task.Run.Zip(x=>x.Compress.SourceFolder(folderToCompress).UsingCompressionLevel.Five.UsingPassword(password).To(zipFilePath));
            Directory.Delete(folderToCompress, true);
            Task.Run.Zip(x=>x.Decompress(zipFilePath).UsingPassword(password).To(rootFolder));

//            Assert.That(Directory.Exists(folderToCompress), "Directory did not exist");

            for (int i = 0; i < 5; i++)
            {
              Assert.That(File.Exists(rootFolder + "\\" + i + ".txt"));
            }
        }
    }
}