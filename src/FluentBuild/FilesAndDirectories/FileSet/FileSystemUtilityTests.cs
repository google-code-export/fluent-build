using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild
{
    [TestFixture]
    public class FileSystemUtilityTests
    {
        private ISearchPatternParser _parser;
        private IFileSystemWrapper _fileSystemWrapper;
        private FileSystemUtility _util;

        [SetUp]
        public void Setup()
        {
            _parser = MockRepository.GenerateStub<ISearchPatternParser>();
            _fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            _util = new FileSystemUtility(_parser, _fileSystemWrapper);
        }
        
        [Test]
        public void Should_Call_Parser_For_Path_Containing_Wildcard()
        {
            const string fileName = @"c:\temp\file*.txt";            
            //setup expectations
            _parser.Folder = @"c:\temp\";
            _parser.SearchPattern = "file*.txt";
            _parser.Recursive = false;
            
            _fileSystemWrapper.Stub(x => x.FileExists(fileName)).Return(true);
            _util.GetAllFilesMatching(fileName);
            _parser.AssertWasCalled(x => x.Parse(fileName));
        }

        [Test]
        public void Should_Not_Call_Parser_For_Path_Containing_No_Wildcards()
        {
            const string fileName = @"c:\temp\file1.txt";
            _util.GetAllFilesMatching(fileName);
            _parser.AssertWasNotCalled(x => x.Parse(fileName));
        }
    }
}