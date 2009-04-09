using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class BuildFolderTests
    {
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
            var file = new BuildFolder("c:\\temp").FileName("test.txt");
            Assert.That(file.ToString(), Is.EqualTo(@"c:\temp\test.txt"));
        }
    }
}