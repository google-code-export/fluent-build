using System;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using FluentFs.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    ///<summary />
    [TestFixture]
    public class MsBuildTaskTests
    {
        #region Setup/Teardown

        ///<summary>
        ///</summary>
        ///<summary />
        [SetUp]
        public void Setup()
        {
            _projectOrSolutionFilePath = "c:\\temp.sln";
            _mockExecutor = MockRepository.GenerateStub<IActionExcecutor>();
            _subject = new MsBuildTask(_mockExecutor).ProjectOrSolutionFilePath(_projectOrSolutionFilePath);
        }

        #endregion

        private MsBuildTask _subject;
        private string _projectOrSolutionFilePath;
        private IActionExcecutor _mockExecutor;


        ///<summary />
        [Test]
        public void BuildArgs_ShouldAddConfigurationIfSpecified()
        {
            string[] buildArgs = _subject.Configuration("DEBUG").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:Configuration=DEBUG"));
        }

        [Test]
        public void BuildArgs_ShouldHandleArgs()
        {
            var output = _subject.WithArguments("arg1");
            Assert.That(output, Is.TypeOf<MsBuildTask>());
            Assert.That(output._args, Contains.Item("arg1"));

        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldHandleFileType()
        {
            string[] buildArgs = new MsBuildTask().ProjectOrSolutionFilePath(new File(_projectOrSolutionFilePath)).BuildArgs();
            Assert.That(buildArgs[0], Is.EqualTo(_projectOrSolutionFilePath));
        }

        [Test,ExpectedException(typeof(ArgumentException))]
        public void ShouldFailIfNoProjectOrSolutionFile()
        {
            new MsBuildTask(_mockExecutor).InternalExecute();
        }
        

        ///<summary />
        [Test]
        public void BuildArgs_ShouldHandleProperties()
        {
            var args = _subject.SetProperty("prop", "value").BuildArgs();
            Assert.That(args[1], Is.EqualTo("/p:prop=value"));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldHaveFirstArgAsProjectOrSolution()
        {
            string[] buildArgs = _subject.BuildArgs();
            Assert.That(buildArgs[0], Is.EqualTo(_projectOrSolutionFilePath));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldHaveTarget()
        {
            string[] buildArgs = _subject.AddTarget("mytarget").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/target:mytarget"));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldNotHaveConfigurationIfNoneSpecified()
        {
            string[] buildArgs = _subject.BuildArgs();
            Assert.That(buildArgs.Length, Is.EqualTo(1));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsNotSet()
        {
            string[] buildArgs = _subject.OutputDirectory("c:\\temp").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:OutDir=c:\\temp\\"));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsSet()
        {
            string[] buildArgs = _subject.OutputDirectory("c:\\temp\\").BuildArgs();
            Assert.That(buildArgs[1], Is.EqualTo("/p:OutDir=c:\\temp\\"));
        }

        [Test]
        public void OutputDirectory_ShouldSetDir()
        {
            string path = "c:\\temp";
            var folder = new Directory(path);
            _subject.OutputDirectory(folder);
            Assert.That(_subject.Outdir, Is.EqualTo(path));
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
            string property = _subject.Properties["name"];
            Assert.That(property, Is.EqualTo("value"));
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

        [Test]
        public void ShouldExecute()
        {
            _subject.InternalExecute();
            _mockExecutor.AssertWasCalled(x=>x.Execute(Arg<Action<Executable>>.Is.Anything));
        }
    }
}