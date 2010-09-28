using FluentBuild.Runners;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    [TestFixture]
    public class MsBuildTaskTests
    {
        private MsBuildTask _subject;
        private string _projectOrSolutionFilePath;
        private IExecuteable _executable;


        [SetUp]
        public void Setup()
        {
            _projectOrSolutionFilePath = "c:\\temp.sln";
            _executable = MockRepository.GenerateStub<IExecuteable>();
            _subject = new MsBuildTask(_projectOrSolutionFilePath, _executable);            
        }

        [Test]
        public void ShouldSetSolutionPath()
        {
            Assert.That(_subject.ProjectOrSolutionFilePath, Is.EqualTo(_projectOrSolutionFilePath));
        }

        [Test]
        public void ShouldSetTarget()
        {
            _subject.AddTarget("target");
            Assert.That(_subject.Targets.Contains("target"));
        }

        [Test]
        public void ShouldSetConfiguration()
        {
            _subject.Configuration("config");
            Assert.That(_subject.ConfigurationToUse, Is.EqualTo("config"));
        }


        [Test]
        public void ShouldSetOutDir()
        {
            _subject.OutputDirectory("outdir");
            Assert.That(_subject.Outdir, Is.EqualTo("outdir"));
        }

        [Test]
        public void ShouldSetProperty()
        {
            _subject.SetProperty("name", "value");
            var property = _subject.Properties["name"];
            Assert.That(property, Is.EqualTo("value"));
        }
        
        [Test]
        public void BuildArgs_ShouldHaveTarget()
        {
            var buildArgs = new MsBuildTask(_projectOrSolutionFilePath).AddTarget("mytarget").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/target:mytarget"));
        }

        [Test]
        public void BuildArgs_ShouldAddConfigurationIfSpecified()
        {
            var buildArgs = new MsBuildTask(_projectOrSolutionFilePath).Configuration("DEBUG").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:Configuration=DEBUG"));
        }

        [Test]
        public void BuildArgs_ShouldNotHaveConfigurationIfNoneSpecified()
        {
            var buildArgs = new MsBuildTask(_projectOrSolutionFilePath).BuildArgs();
            Assert.That(buildArgs.Length, Is.EqualTo(1));
        }

        [Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsNotSet()
        {
            var buildArgs = new MsBuildTask(_projectOrSolutionFilePath).OutputDirectory("c:\\temp").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:OutDir=c:\\temp\\"));
        }

        [Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsSet()
        {
            var buildArgs = new MsBuildTask(_projectOrSolutionFilePath).OutputDirectory("c:\\temp\\").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:OutDir=c:\\temp\\"));
        }

        [Test]
        public void BuildArgs_ShouldHaveFirstArgAsProjectOrSolution()
        {
            var buildArgs = new MsBuildTask(_projectOrSolutionFilePath).BuildArgs();
            Assert.That(buildArgs[0], Is.EqualTo(_projectOrSolutionFilePath));
        }

        [Test]
        public void Execute_ShouldExecuteMsBuild()
        {
            //mock calls
            _executable.Stub(x => x.Executable("")).IgnoreArguments().Return(_executable);
            _executable.Stub(x => x.WithArguments("")).IgnoreArguments().Return(_executable);

            _subject.Execute();
            _executable.AssertWasCalled(x=>x.Execute());
        }


    }
}