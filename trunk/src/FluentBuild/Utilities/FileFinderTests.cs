using System;
using FluentBuild.FilesAndDirectories;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild.Utilities
{
    [TestFixture]
    public class FileFinderTests
    {
        [Test]
        public void Find_ShouldSearchFileSystem()
        {
            var mockFilesystem = MockRepository.GenerateStub<IFileSystemWrapper>();
            var finder = new FileFinder(mockFilesystem);

            string fileToSearchFor = "searchForMe.exe";
            mockFilesystem.Stub(x => x.GetFilesIn(Environment.CurrentDirectory)).Return(new[]
                                                                                            {
                                                                                                "test.txt",
                                                                                            });
            finder.Find(fileToSearchFor);
        }

        [Test]
        public void Find_ShouldSearchFileSystemRecursively()
        {
            var mockFilesystem = MockRepository.GenerateStub<IFileSystemWrapper>();
            var finder = new FileFinder(mockFilesystem);

            string fileToSearchFor = "searchForMe.exe";
            mockFilesystem.Stub(x => x.GetFilesIn(Environment.CurrentDirectory)).Return(new[]
                                                                                            {
                                                                                                "test.txt",
                                                                                            });
            mockFilesystem.Stub(x => x.GetDirectories(Environment.CurrentDirectory)).Return(new[] {"src", "tools"});
            mockFilesystem.Stub(x => x.GetFilesIn("src")).Return(new[]
                                                                     {
                                                                         "test.txt",
                                                                     });
            mockFilesystem.Stub(x => x.GetFilesIn("tools")).Return(new[]
                                                                       {
                                                                           "test.txt",
                                                                           fileToSearchFor
                                                                       });
            Assert.That(finder.Find(fileToSearchFor),
                        Is.EqualTo(fileToSearchFor));
        }
    }
}