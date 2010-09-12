using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild
{
    [TestFixture]
    public class CopyFilesetTests
    {
        [Test]
        public void To_ShouldPerformCopy()
        {
            MessageLogger.WindowWidth = 80;
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            var fileSet = new FileSet();
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

    public class CopyFileset
    {
        private readonly FluentBuild.FileSet _fileSet;
        private readonly IFileSystemWrapper _fileSystemWrapper;

        public CopyFileset(FileSet fileSet, IFileSystemWrapper fileSystemWrapper)
        {
            _fileSet = fileSet;
            _fileSystemWrapper = fileSystemWrapper;
        }

        public CopyFileset(FluentBuild.FileSet fileSet): this(fileSet, new FileSystemWrapper())
        {
        }

        public FluentBuild.FileSet To(BuildFolder destination)
        {
            MessageLogger.Write("copy", String.Format("Copying {0} files to '{1}'", _fileSet.Files.Count, destination));
            MessageLogger.BlankLine();
            foreach (string file in _fileSet.Files)
            {
                string destinationPath = Path.Combine(destination.ToString(), Path.GetFileName(file));
                _fileSystemWrapper.Copy(file, destinationPath);
            }
            return _fileSet;
        }
    }
}
