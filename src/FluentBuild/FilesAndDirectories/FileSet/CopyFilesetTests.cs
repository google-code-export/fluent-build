using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    [TestFixture]
    public class CopyFilesetTests
    {
        [Test]
        public void To_ShouldPerformCopy()
        {
            MessageLogger.WindowWidth = 80;
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            var fileSet = new FluentBuild.FileSet();
            fileSet.Include(@"c:\temp\test1.txt");
            fileSet.Include(@"c:\temp\test2.txt");
            var subject = new CopyFileset(fileSet, fs);

            string destination = @"c:\destination";
            var dest = new BuildFolder(destination);
            subject.To(dest);

            fs.AssertWasCalled(x=>x.Copy(fileSet.Files[0], destination + "\\test1.txt"));
            fs.AssertWasCalled(x => x.Copy(fileSet.Files[1], destination + "\\test2.txt"));


        }
    }
}