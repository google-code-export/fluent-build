using System;
using System.Collections.Generic;
using System.Linq;
using FluentBuild.Core;
using NUnit.Framework;

using Rhino.Mocks;

namespace FluentBuild.FilesAndDirectories.FileSet
{
    [TestFixture]
    public class FileSetTests
    {
        [Test]
        public void BuildFileSet()
        {
            string fileName = "test.txt";
            var fileset = new Core.FileSet(null).Include(fileName);
            Assert.That(fileset.Files.Count(), Is.EqualTo(1));
            Assert.That(fileset.Files[0], Is.EqualTo(fileName));
        }

        [Test]
        public void CopyShouldNotBeNull()
        {
            var fileset = new Core.FileSet(null);
            Assert.That(fileset.Copy, Is.Not.Null);
        }

        [Test]
        public void BuildFileSetFromArtifact()
        {
            string fileName = "test.txt";
            var artifact = new BuildArtifact(fileName);
            var fileset = new Core.FileSet(null).Include(artifact);
            Assert.That(fileset.Files.Count(), Is.EqualTo(1));
            Assert.That(fileset.Files[0], Is.EqualTo(fileName));
        }


        [Test]
        public void AddAllCsFiles()
        {
            string path = "c:\\temp\\*.cs";
            IFileSystemUtility utility = MockRepository.GenerateStub<IFileSystemUtility>();
            var results = new List<String>();
            results.Add("c:\\temp\\test1.cs");
            results.Add("c:\\temp\\test2.cs");
            utility.Stub(x => x.GetAllFilesMatching(path)).Return(results); 
            
            var fileset = new Core.FileSet(utility).Include(path);
            Assert.That(fileset.Files.Count, Is.EqualTo(2));
            Assert.That(fileset.Files[0], Is.EqualTo("c:\\temp\\test1.cs"));
        }

        [Test]
        public void AddAllCsFilesAndExcludeSpecificFileName()
        {
            string path = "c:\\temp\\*.cs";
            IFileSystemUtility utility = MockRepository.GenerateStub<IFileSystemUtility>();
            var results = new List<String>();
            results.Add("c:\\temp\\test1.cs");
            results.Add("c:\\temp\\test2.cs");
            utility.Stub(x => x.GetAllFilesMatching(path)).Return(results);

            var fileset = new Core.FileSet(utility).Include(path);
            fileset.Exclude("c:\\temp\\test2.cs");
            Assert.That(fileset.Files.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddAllCsFilesAndExcludeGenericFileName()
        {
            string includeFilter = @"c:\temp\**\*.cs";
            string exclusionFilter = @"c:\temp\**\test1.cs";
            var includedFiles = new List<String>();
            includedFiles.Add(@"c:\temp\dir1\test1.cs");
            includedFiles.Add(@"c:\temp\dir1\test2.cs");
            includedFiles.Add(@"c:\temp\dir2\test1.cs");

            IFileSystemUtility utility = MockRepository.GenerateStub<IFileSystemUtility>();
            utility.Stub(x => x.GetAllFilesMatching(includeFilter)).Return(includedFiles);
            utility.Stub(x => x.GetAllFilesMatching(exclusionFilter)).Return(new List<string>());
            
            var fileset = new Core.FileSet(utility).Include(includeFilter);
            
            fileset.Exclude(exclusionFilter);
            utility.AssertWasCalled(x => x.GetAllFilesMatching(exclusionFilter));
        }
        
    }
}