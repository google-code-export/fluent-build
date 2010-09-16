using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    [TestFixture]
    public class SearchPatternParserTests
    {
        /*
     *     c:\temp\*.cs        
     *     c:\temp\
     *     c:\temp\*.*
     *     c:\temp\**\*.cs
     *     c:\temp\auto*.cs
     *     c:\temp\**\auto*.cs
     */

        [Test]
        public void GetAllFilesMatching_Name_And_WildCard()
        {
            var parser = new SearchPatternParser();
                parser.Parse(@"c:\temp\auto*.cs");
            Assert.That(parser.Folder, Is.EqualTo(@"c:\temp\"));
            Assert.That(parser.SearchPattern, Is.EqualTo("auto*.cs"));
            Assert.That(parser.Recursive, Is.EqualTo(false));
        }

        [Test]
        public void GetAllFilesMatching_Recursive_Name_And_WildCard()
        {
            var parser = new SearchPatternParser();
            parser.Parse(@"c:\temp\**\auto*.cs");
            Assert.That(parser.Folder, Is.EqualTo(@"c:\temp\"));
            Assert.That(parser.SearchPattern, Is.EqualTo("auto*.cs"));
            Assert.That(parser.Recursive, Is.EqualTo(true));
        }
        
        [Test]
        public void GetAllFilesMatching_JustDirectory()
        {
            var parser = new SearchPatternParser();
            parser.Parse(@"c:\temp\");
            Assert.That(parser.Folder, Is.EqualTo(@"c:\temp\"));
            Assert.That(parser.SearchPattern, Is.EqualTo("*.*"));
            Assert.That(parser.Recursive, Is.EqualTo(false));
        }

        [Test]
        public void GetAllFilesMatching_Start_Dot_Start_Filter()
        {
            var parser = new SearchPatternParser();
            parser.Parse(@"c:\temp\*.*");
            Assert.That(parser.Folder, Is.EqualTo(@"c:\temp\"));
            Assert.That(parser.SearchPattern, Is.EqualTo("*.*"));
            Assert.That(parser.Recursive, Is.EqualTo(false));
        }

        [Test]
        public void GetAllFilesMatching_Start_Dot_CS_Filter()
        {
            var parser = new SearchPatternParser();
            parser.Parse(@"c:\temp\*.cs");
            Assert.That(parser.Folder, Is.EqualTo(@"c:\temp\"));
            Assert.That(parser.SearchPattern, Is.EqualTo("*.cs"));
            Assert.That(parser.Recursive, Is.EqualTo(false));
        }

        [Test]
        public void GetAllFilesMatching_Recursive_Start_Dot_CS_Filter()
        {
            var parser = new SearchPatternParser();
            parser.Parse(@"c:\temp\**\*.cs");
            Assert.That(parser.Folder, Is.EqualTo(@"c:\temp\"));
            Assert.That(parser.SearchPattern, Is.EqualTo("*.cs"));
            Assert.That(parser.Recursive, Is.EqualTo(true));
        }
    }
}