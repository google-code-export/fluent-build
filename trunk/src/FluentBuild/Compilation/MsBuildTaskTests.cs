
using FluentBuild.Core;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    ///<summary />
	[TestFixture]
    public class MsBuildTaskTests
    {
        private MsBuildTask _subject;
        private string _projectOrSolutionFilePath;
        private IExecutable _executable;


        ///<summary>
        ///</summary>
        ///<summary />
	[SetUp]
        public void Setup()
        {
            _projectOrSolutionFilePath = "c:\\temp.sln";
            _executable = MockRepository.GenerateStub<IExecutable>();
            _subject = new MsBuildTask(_executable).ProjectOrSolutionFilePath(_projectOrSolutionFilePath);            
        }

        ///<summary />
	[Test]
        public void ShouldSetSolutionPath()
        {
            Assert.That(_subject._projectOrSolutionFilePath, Is.EqualTo(_projectOrSolutionFilePath));
        }

        ///<summary />
	[Test]
        public void ShouldSetTarget()
        {
            _subject.AddTarget("target");
            Assert.That(_subject.Targets.Contains("target"));
        }

        ///<summary />
	[Test]
        public void ShouldSetConfiguration()
        {
            _subject.Configuration("config");
            Assert.That(_subject.ConfigurationToUse, Is.EqualTo("config"));
        }


        ///<summary />
	[Test]
        public void ShouldSetOutDir()
        {
            _subject.OutputDirectory("outdir");
            Assert.That(_subject.Outdir, Is.EqualTo("outdir"));
        }

        ///<summary />
	[Test]
        public void ShouldSetProperty()
        {
            _subject.SetProperty("name", "value");
            var property = _subject.Properties["name"];
            Assert.That(property, Is.EqualTo("value"));
        }
        
        ///<summary />
	[Test]
        public void BuildArgs_ShouldHaveTarget()
        {
            var buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(_projectOrSolutionFilePath).AddTarget("mytarget").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/target:mytarget"));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldAddConfigurationIfSpecified()
        {
            var buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(_projectOrSolutionFilePath).Configuration("DEBUG").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:Configuration=DEBUG"));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldNotHaveConfigurationIfNoneSpecified()
        {
            var buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(_projectOrSolutionFilePath).BuildArgs();
            Assert.That(buildArgs.Length, Is.EqualTo(1));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsNotSet()
        {
            var buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(_projectOrSolutionFilePath).OutputDirectory("c:\\temp").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:OutDir=c:\\temp\\"));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsSet()
        {
            var buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(_projectOrSolutionFilePath).OutputDirectory("c:\\temp\\").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:OutDir=c:\\temp\\"));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldHaveFirstArgAsProjectOrSolution()
        {
            var buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(_projectOrSolutionFilePath).BuildArgs();
            Assert.That(buildArgs[0], Is.EqualTo(_projectOrSolutionFilePath));
        }

        [Test]
        public void OutputDirectory_ShouldSetDir()
        {
            var path = "c:\\temp";
            var folder = new FluentFs.Core.Directory(path);
            _subject.OutputDirectory(folder);
            Assert.That(_subject.Outdir, Is.EqualTo(path));
        }
    }
}