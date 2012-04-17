using System;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class BuildFolderTests : TestBase
    {
        private FluentFs.Core.Directory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new FluentFs.Core.Directory(rootFolder).SubFolder("Test");
        }
        
        [Test]
        public void Create_ShouldCreateSubFolder()
        {
            _subject.Create();
            Assert.That(Directory.Exists(_subject.ToString()));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Create_ShouldFailIfImproperCharactersInPath()
        {
            _subject.SubFolder("invalidzzz@#$&&@$#%*").Create(FluentFs.Core.OnError.Fail);
        }

        [Test]
        public void Create_ShouldContinueIfContinueOnErrorIsSet()
        {
            _subject.SubFolder("invalidzzz@#$&&@$#%*").Create(FluentFs.Core.OnError.Continue);
        }

        [Test]
        public void Delete_ShouldDeleteSubFolder()
        {
            _subject.Create();
            Assert.That(Directory.Exists(_subject.ToString()));
            _subject.Delete();
            Assert.That(!Directory.Exists(_subject.ToString()));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Delete_ShouldFailIfImproperCharactersInPath()
        {
            _subject.Create();
            Assert.That(Directory.Exists(_subject.ToString()));
            _subject.SubFolder("invalidzzz@#$&&@$#%*").Delete(FluentFs.Core.OnError.Fail);
        }

        [Test]
        public void Delete_ShouldContinueIfContinueOnErrorIsSet()
        {
            _subject.Create();
            Assert.That(Directory.Exists(_subject.ToString()));
            _subject.SubFolder("invalidzzz@#$&&@$#%*").Delete(FluentFs.Core.OnError.Continue);
        }
    }
}