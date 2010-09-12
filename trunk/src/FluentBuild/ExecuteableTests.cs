using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class ExecuteableTests
    {
        [Test]
        public void ShouldConstructProperly()
        {
            const string executeablePath = @"c:\temp\nothing.exe";
            const string workingDirectory = @"c:\";
            var executeable = new Executeable(executeablePath).InWorkingDirectory(workingDirectory).WithArguments(new[] { "one", "two", "three"});
            Assert.That(executeable.CreateArgumentString(), Is.EqualTo(" one two three"));
            Assert.That(executeable._executeablePath, Is.EqualTo(executeablePath));
            Assert.That(executeable._workingDirectory, Is.EqualTo(workingDirectory));
        }

        [Test]
        public void ShouldPopulateWorkingDirectoryViaArtifact()
        {
            const string executeablePath = @"c:\temp\nothing.exe";
            const string workingDirectory = @"c:\";
            var workingFolder = new BuildFolder(workingDirectory);
            var executeable = new Executeable(executeablePath).InWorkingDirectory(workingFolder);
            Assert.That(executeable._workingDirectory, Is.EqualTo(workingDirectory));
        }

    }
}