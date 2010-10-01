using FluentBuild.Core;
using FluentBuild.Runners;
using NUnit.Framework;


namespace FluentBuild.Utilities
{
    ///<summary />
	[TestFixture]
    public class ExecuteableTests
    {
        private const string _executablePath = @"c:\temp\nothing.exe";

        ///<summary />
	[Test]
        public void Executable_ShouldSetProperly()
        {
            var subject = new Executeable();
            Assert.That(subject.Executable(_executablePath), Is.EqualTo(subject));
            Assert.That(subject.ExecuteablePath, Is.EqualTo(_executablePath));
        }

        ///<summary />
	[Test]
        public void ShouldConstructProperly()
        {
            const string workingDirectory = @"c:\";
            var executeable = (Executeable)new Executeable(_executablePath).InWorkingDirectory(workingDirectory).WithArguments(new[] { "one", "two", "three" });
            Assert.That(executeable.CreateArgumentString(), Is.EqualTo(" one two three"));
            Assert.That(executeable.ExecuteablePath, Is.EqualTo(_executablePath));
            Assert.That(executeable.WorkingDirectory, Is.EqualTo(workingDirectory));
        }

        ///<summary />
	[Test]
        public void ShouldPopulateWorkingDirectoryViaArtifact()
        {
            const string workingDirectory = @"c:\";
            var workingFolder = new BuildFolder(workingDirectory);
            var executeable = (Executeable) new Executeable(_executablePath).InWorkingDirectory(workingFolder);
            Assert.That(executeable.WorkingDirectory, Is.EqualTo(workingDirectory));
        }

        ///<summary />
	[Test]
        public void CreateProcess_Should_Build_Process()
        {
            var subject = new Executeable(_executablePath);
            subject.InWorkingDirectory("c:\\temp");
            var processWrapper = subject.CreateProcess();
            Assert.That(processWrapper, Is.Not.Null);
        }

    }
}