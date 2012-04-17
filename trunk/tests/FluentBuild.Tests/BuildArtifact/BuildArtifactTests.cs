using System.IO;
using FluentBuild.Utilities;
using NUnit.Framework;
using File = FluentFs.Core.File;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class BuildArtifactTests : TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _artifact = new File(rootFolder + "\\test.txt");
            System.IO.File.Create(_artifact.ToString()).Dispose();
        }

        #endregion

        private File _artifact;


        [Test]
        public void ShouldCopyFile()
        {
            _artifact.Copy.To(rootFolder + "\\test2.txt");
            Assert.That(System.IO.File.Exists(rootFolder + "\\test.txt"));
            Assert.That(System.IO.File.Exists(rootFolder + "\\test2.txt"));
        }

        [Test]
        public void ShouldDeleteFile()
        {
            _artifact.Delete();
            Assert.That(!System.IO.File.Exists(rootFolder + "\\test.txt"));
        }

        [Test, ExpectedException(typeof (FileNotFoundException))]
        public void ShouldFailToCopyFileIfFileDoesNotExist()
        {
            System.IO.File.Delete(_artifact.ToString());
            _artifact.Copy.FailOnError.To(rootFolder + "\\test2.txt");
        }

        [Test, ExpectedException(typeof (IOException))]
        public void ShouldFailToDeleteFileIfFileInUse()
        {
            using (var fs = new StreamReader(_artifact.ToString()))
            {
                _artifact.Delete(FluentFs.Core.OnError.Fail);
            }
        }

        [Test, ExpectedException(typeof (FileNotFoundException))]
        public void ShouldFailToRenameIfFileDoesNotExist()
        {
            System.IO.File.Delete(_artifact.ToString());
            _artifact.Rename.FailOnError.To("test2.txt");
            Assert.That(System.IO.File.Exists(rootFolder + "\\test2.txt"));
        }

        [Test]
        public void ShouldNotThrowErrorIfFileDoesNotExist()
        {
            System.IO.File.Delete(_artifact.ToString());
            _artifact.Copy.ContinueOnError.To(rootFolder + "\\test2.txt");
        }

        [Test]
        public void ShouldNotThrowErrorIfFileInUse()
        {
            using (var fs = new StreamReader(_artifact.ToString()))
            {
                _artifact.Delete(FluentFs.Core.OnError.Continue);
            }
        }

        [Test]
        public void ShouldRenameFile()
        {
            _artifact.Rename.To("test2.txt");
            Assert.That(System.IO.File.Exists(rootFolder + "\\test2.txt"));
        }

        [Test]
        public void ShouldSucceedIfFileDoesNotExist()
        {
            System.IO.File.Delete(_artifact.ToString());
            _artifact.Rename.ContinueOnError.To("test2.txt");
        }
    }
}