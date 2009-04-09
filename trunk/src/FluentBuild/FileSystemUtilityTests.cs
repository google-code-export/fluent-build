using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild
{
    [TestFixture]
    public class FileSystemUtilityTests
    {
        [Test]
        public void Should_Call_Parser_For_Path_Containing_Wildcard()
        {
            const string fileName = @"c:\temp\file*.txt";

            var parser = MockRepository.GenerateStub<ISearchPatternParser>();
            //setup expectations
            parser.Folder = @"c:\temp\";
            parser.SearchPattern = "file*.txt";
            parser.Recursive = false;

            var util = new FileSystemUtility(parser);
            
            util.GetAllFilesMatching(fileName);

            parser.AssertWasCalled(x => x.Parse(fileName));
        }

        [Test]
        public void Should_Not_Call_Parser_For_Path_Containing_No_Wildcards()
        {
            const string fileName = @"c:\temp\file1.txt";

            var parser = MockRepository.GenerateStub<ISearchPatternParser>();
            var util = new FileSystemUtility(parser);
            util.GetAllFilesMatching(fileName);

            parser.AssertWasNotCalled(x => x.Parse(fileName));
        }
    }
}