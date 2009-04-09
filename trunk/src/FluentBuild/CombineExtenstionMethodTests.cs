using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class CombineExtenstionMethodTests
    {
        //dir_compile.And(dir_tests).WithNameOf(name_commons);
        [Test]
        public void ShouldCombinePaths()
        {
            Assert.That(@"c:\temp".SubFolder("src").FileName("test.cs"), Is.EqualTo(@"c:\temp\src\test.cs"));
        }

        [Test]
        public void ShouldCombinePathsWithWildcardFilename()
        {
            Assert.That(@"c:\temp".SubFolder("src").FileName("test*.cs"), Is.EqualTo(@"c:\temp\src\test*.cs"));
        }

        [Test]
        public void ShouldAddRecursor()
        {
            Assert.That(@"c:\temp".RecurseAllSubFolders().FileName("test.cs"), Is.EqualTo(@"c:\temp\**\test.cs"));
        }

        [Test]
        public void ShouldAddRecursor_Without_Using_Explicit_Extension()
        {
            Assert.That(@"c:\temp".SubFolder("**").FileName("test.cs"), Is.EqualTo(@"c:\temp\**\test.cs"));
        }

        [Test]
        public void ShouldAddRecursorWithNoDoubleSlash()
        {
            Assert.That(@"c:\temp\".RecurseAllSubFolders().FileName("test.cs"), Is.EqualTo(@"c:\temp\**\test.cs"));
        }



    }
}