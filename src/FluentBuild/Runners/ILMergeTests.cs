using System.IO;
using FluentBuild.Core;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Runners
{
    [TestFixture]
    public class ILMergeTests
    {
        private ILMerge _subject;
        private IFileFinder _fileFinder;

        [SetUp]
        public void Setup()
        {
            _fileFinder = MockRepository.GenerateMock<IFileFinder>();
            _subject = new ILMerge(_fileFinder);
        }

        [Test]
        public void FindExecuteable_ShouldUseSetArg()
        {
            var path = "c:\\temp\\ilmerge.exe";
            _subject.ExecuteableLocatedAt(path);
            Assert.That(_subject.FindExecuteable(), Is.EqualTo(path));
        }

        [Test]
        public void FindExecuteable_ShouldAutoFindIfNotSet()
        {
            _fileFinder.Stub(x => x.Find("ILMerge.exe")).Return("c:\\ilmerge.exe");
            _subject.FindExecuteable();
            _fileFinder.AssertWasCalled(x=>x.Find("ILMerge.exe"));
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void FindExecuteable_ShouldThrowExecptionIfItCantBeFound()
        {
            _fileFinder.Stub(x => x.Find("ILMerge.exe")).Return(null);
            _subject.FindExecuteable();

        }

        [Test]
        public void BuildArgs_ShouldBuildArgs()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET2_0;
            
            var args = _subject.AddSource("input.dll").OutputTo("c:\\test.dll").BuildArgs();
            Assert.That(args[0], Is.EqualTo("input.dll"));
            Assert.That(args[1], Is.EqualTo("/OUT:c:\\test.dll"));
            Assert.That(args[2], Is.EqualTo("/ndebug"));
        }

        [Test]
        public void BuildArgs_ShouldSetFrameworkTypeIfFrameworkIsDotNet4()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET4_0.Full;
            var args = _subject.AddSource("input.dll").OutputTo("c:\\test.dll").BuildArgs();
            Assert.That(args[2], Is.StringStarting("/targetplatform:v4,"));
        }

        [Test]
        public void AddSource_ShouldSetSource()
        {
            var source = "c:\\test.dll";
            _subject.AddSource(source);
            Assert.That(_subject.Sources[0], Is.EqualTo(source));
        }

        [Test]
        public void AddBuildArtifactSource_ShouldSetSource()
        {
            var source = new BuildArtifact("c:\\test.dll");
            _subject.AddSource(source);
            Assert.That(_subject.Sources[0], Is.EqualTo(source.ToString()));
        }

        [Test]
        public void OutputToWithBuildArtifact_ShouldOutpt()
        {
            var source = new BuildArtifact("c:\\test.dll");
            _subject.OutputTo(source);
            Assert.That(_subject.Destination, Is.EqualTo(source.ToString()));
        }

        [Test]
        public void OutputTo_ShouldOutpt()
        {
            var source = "c:\\test.dll";
            _subject.OutputTo(source);
            Assert.That(_subject.Destination, Is.EqualTo(source));
        }
    }
}