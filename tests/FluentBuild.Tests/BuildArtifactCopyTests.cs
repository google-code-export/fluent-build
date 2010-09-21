using System.IO;
using FluentBuild.Core;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class BuildArtifactCopyTests : TestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            SetupTestFolder();
            Directory.CreateDirectory(Path.Combine(rootFolder, "dir1"));

            File.Create(Path.Combine(rootFolder, "test.txt")).Dispose();
        }

        #endregion

        [Test]
        // c:\temp\test.txt --> c:\temp\dir1\test2.txt
        public void ReplaceToken_Should_Copy_To_New_Directory_And_Change_File_Name()
        {
            var artifact = new BuildArtifact(Path.Combine(rootFolder, "test.txt"));
            string destination = Path.Combine(rootFolder, "dir1");
            artifact.Copy.To(Path.Combine(destination, "test2.txt"));
            Assert.That(File.Exists(Path.Combine(destination, "test2.txt")));
        }

        [Test]
        // c:\temp\test.txt --> c:\temp\dir1
        public void ReplaceToken_Should_Copy_To_New_Directory_And_Perserve_File_Name()
        {
            var artifact = new BuildArtifact(Path.Combine(rootFolder, "test.txt"));
            string destination = Path.Combine(rootFolder, "dir1");
            artifact.Copy.To(destination);
            Assert.That(File.Exists(Path.Combine(destination, "test.txt")));
        }

        [Test]
        // c:\temp\test.txt --> c:\temp\test2.txt
        public void ReplaceToken_Should_Copy_To_New_File()
        {
            var artifact = new BuildArtifact(Path.Combine(rootFolder, "test.txt"));
            string destination = Path.Combine(rootFolder, "test2.txt");
            artifact.Copy.To(destination);
            Assert.That(File.Exists(destination));
        }
    }
}