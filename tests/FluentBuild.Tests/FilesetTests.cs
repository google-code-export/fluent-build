using System.IO;
using FluentBuild.Core;
using FluentFs.Core;
using NUnit.Framework;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class FilesetTests : TestBase
    {
        private FileSet _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new FileSet();
            File.Create(rootFolder + "\\" + "temp.txt").Dispose();
            _subject.Include(rootFolder + "\\*.txt");
        }

        [Test]
        public void ShouldCopyFile()
        {
            string destination = rootFolder + "\\destination";
            Directory.CreateDirectory(destination);
            _subject.Copy.To(destination);
            Assert.That(File.Exists(destination + "\\temp.txt"));
        }

        [Test, ExpectedException(typeof(DirectoryNotFoundException))]
        public void ShouldFailIfDestinationDoesNotExist()
        {
            string destination = rootFolder + "\\destination";
            _subject.Copy.FailOnError.To(destination);
            Assert.That(File.Exists(destination + "\\temp.txt"));
        }

        [Test]
        public void ShouldContinueIfErrorOccurs()
        {
            string destination = rootFolder + "\\destination";
            _subject.Copy.ContinueOnError.To(destination);
            Assert.That(!File.Exists(destination + "\\temp.txt"));
        }
    }
}