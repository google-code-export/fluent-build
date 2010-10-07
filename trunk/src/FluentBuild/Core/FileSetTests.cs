using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentBuild.FilesAndDirectories.FileSet;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Core
{
    ///<summary />
    [TestFixture]
    public class FileSetTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _mockFs = MockRepository.GenerateStub<IFileSystemUtility>();
            _subject = new FileSet(_mockFs);
        }

        #endregion

        private FileSet _subject;
        private IFileSystemUtility _mockFs;


        ///<summary />
        [Test]
        public void AddAllCsFiles()
        {
            string path = "c:\\temp\\*.cs";
            var utility = MockRepository.GenerateStub<IFileSystemUtility>();
            var results = new List<String>();
            results.Add("c:\\temp\\test1.cs");
            results.Add("c:\\temp\\test2.cs");
            utility.Stub(x => x.GetAllFilesMatching(path)).Return(results);

            FileSet fileset = new FileSet(utility).Include(path);
            Assert.That(fileset.Files.Count, Is.EqualTo(2));
            Assert.That(fileset.Files[0], Is.EqualTo("c:\\temp\\test1.cs"));
        }

        ///<summary />
        [Test]
        public void AddAllCsFilesAndExcludeSpecificFileName()
        {
            string path = "c:\\temp\\*.cs";

            var results = new List<String>();
            results.Add("c:\\temp\\test1.cs");
            results.Add("c:\\temp\\test2.cs");
            _mockFs.Stub(x => x.GetAllFilesMatching(path)).Return(results);

            FileSet fileset = _subject.Include(path);
            fileset.Exclude("c:\\temp\\test2.cs");
            Assert.That(fileset.Files.Count, Is.EqualTo(1));
        }

        ///<summary />
        [Test]
        public void BuildFileSetFromArtifact()
        {
            string fileName = "test.txt";
            var artifact = new BuildArtifact(fileName);
            FileSet fileset = _subject.Include(artifact);
            Assert.That(fileset.Inclusions.Count(), Is.EqualTo(1));
            Assert.That(fileset.Inclusions[0], Is.EqualTo(fileName));
        }

        ///<summary />
        [Test]
        public void CopyShouldNotBeNull()
        {
            Assert.That(_subject.Copy, Is.Not.Null);
        }

        [Test]
        public void Exclude_RecurseAllSubfolders_ShouldSetPending()
        {
            var folder = new BuildFolder("c:\\windows");

            BuildFolderChoices x = _subject.Exclude(folder).RecurseAllSubDirectories;
            Assert.That(x.PendingExclude, Is.EqualTo("c:\\windows\\**\\"));
        }

        [Test]
        public void Include_RecurseAllSubfolders_ShouldSetPending()
        {
            var folder = new BuildFolder("c:\\windows");
            BuildFolderChoices x = _subject.Include(folder).RecurseAllSubDirectories;
            Assert.That(x.PendingInclude, Is.EqualTo("c:\\windows\\**\\"));
        }

        ///<summary />
        [Test]
        public void Include_ShouldLoadCollection()
        {
            string fileName = "test.txt";
            var fileset = _subject.Include(fileName);
            Assert.That(fileset.Inclusions.Count(), Is.EqualTo(1));
            Assert.That(fileset.Inclusions[0], Is.EqualTo(fileName));
        }

        [Test]
        public void ProcessPending_ShouldClearPendings()
        {
            string includePath = "c:\\temp";
            _subject.PendingInclude = includePath;
            string excludePath = "c:\\windows\\";
            _subject.PendingExclude = excludePath;
            _subject.ProcessPendings();
            Assert.That(_subject.PendingInclude, Is.EqualTo(string.Empty));
            Assert.That(_subject.PendingExclude, Is.EqualTo(string.Empty));
            Assert.That(_subject.Files[0], Is.EqualTo(includePath));
            Assert.That(_subject.Exclusions[0], Is.EqualTo(excludePath));
        }

        [Test]
        public void RecurseAllSubfoldersShouldWork()
        {
            var folder = new BuildFolder("c:\\windows");
            ReadOnlyCollection<string> files = _subject.Include(folder).RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(folder).RecurseAllSubDirectories.Filter("AssemblyInfo.*")
                .Include("c:\\temp\test.txt")
                .Files;
        }
    }
}