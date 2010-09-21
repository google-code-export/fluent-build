using FluentBuild.Core;
using FluentBuild.Runners;
using NUnit.Framework;


namespace FluentBuild.Utilities
{
    [TestFixture]
    public class ExecuteableTests
    {
        private const string _executablePath = @"c:\temp\nothing.exe";

        [Test]
        public void GetSelf_ShouldGetSameObject()
        {
            var subject = new Executeable();
            Assert.That(subject.GetSelf, Is.EqualTo(subject));
        }

        [Test]
        public void Executable_ShouldSetProperly()
        {
            var subject = new Executeable();
            Assert.That(subject.Executable(_executablePath), Is.EqualTo(subject));
            Assert.That(subject._executeablePath, Is.EqualTo(_executablePath));
        }

        [Test]
        public void ShouldConstructProperly()
        {
            const string workingDirectory = @"c:\";
            var executeable = (Executeable)new Executeable(_executablePath).InWorkingDirectory(workingDirectory).WithArguments(new[] { "one", "two", "three" });
            Assert.That(executeable.CreateArgumentString(), Is.EqualTo(" one two three"));
            Assert.That(executeable._executeablePath, Is.EqualTo(_executablePath));
            Assert.That(executeable._workingDirectory, Is.EqualTo(workingDirectory));
        }

        [Test]
        public void ShouldPopulateWorkingDirectoryViaArtifact()
        {
            const string workingDirectory = @"c:\";
            var workingFolder = new BuildFolder(workingDirectory);
            var executeable = (Executeable) new Executeable(_executablePath).InWorkingDirectory(workingFolder);
            Assert.That(executeable._workingDirectory, Is.EqualTo(workingDirectory));
        }

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