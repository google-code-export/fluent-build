using System;
using FluentBuild.Core;
using FluentBuild.FilesAndDirectories;
using NUnit.Framework;

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
            mockFilesystem.Stub(x => x.GetFilesIn(Properties.CurrentDirectory)).Return(new[]
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
            mockFilesystem.Stub(x => x.GetFilesIn(Properties.CurrentDirectory)).Return(new[]
                                                                                            {
                                                                                                "test.txt",
                                                                                            });
            mockFilesystem.Stub(x => x.GetDirectories(Properties.CurrentDirectory)).Return(new[] {"src", "tools"});
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