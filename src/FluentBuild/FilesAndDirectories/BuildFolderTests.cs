using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild
{
    [TestFixture]
    public class BuildFolderTests
    {
        [Test]
        public void CreateDirecory_ShouldCallWrapper()
        {
            MessageLogger.WindowWidth = 80;
            var expected = "c:\\temp";
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            var folder = new BuildFolder(fs, expected);
            folder.Create();
            fs.AssertWasCalled(x=>x.CreateDirectory(expected));
        }

        [Test]
        public void DeleteDirecory_ShouldCheckExistanceOfFolder()
        {
            MessageLogger.WindowWidth = 80;
            var expected = "c:\\temp";
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            var folder = new BuildFolder(fs, expected);
            folder.Delete();
            fs.AssertWasCalled(x => x.DirectoryExists(expected));
        }

        [Test]
        public void DeleteDirecory_ShouldDeleteIfFolderExists()
        {
            MessageLogger.WindowWidth = 80;
            var expected = "c:\\temp";
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            var folder = new BuildFolder(fs, expected);
            fs.Stub(x => x.DirectoryExists(expected)).Return(true);
            folder.Delete();
            fs.AssertWasCalled(x => x.DeleteDirectory(expected, true));
        }

        
        [Test]
        public void Create_Should_Have_Path()
        {
            var expected = "c:\\temp";
            var folder = new BuildFolder(expected);
            Assert.That(folder.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void Create_Should_Build_SubFolder()
        {
            var folder = new BuildFolder("c:\\temp").SubFolder("tmp");
            Assert.That(folder.ToString(), Is.EqualTo("c:\\temp\\tmp"));
        }

        [Test]
        public void Create_Should_Build_Recursive_And_SubFolder()
        {
            var folder = new BuildFolder("c:\\temp").RecurseAllSubFolders().SubFolder("tmp");
            Assert.That(folder.ToString(), Is.EqualTo(@"c:\temp\**\tmp"));
        }

        [Test]
        public void Create_Should_Build_File()
        {
            var file = new BuildFolder("c:\\temp").File("test.txt");
            Assert.That(file.ToString(), Is.EqualTo(@"c:\temp\test.txt"));
        }
    }
}