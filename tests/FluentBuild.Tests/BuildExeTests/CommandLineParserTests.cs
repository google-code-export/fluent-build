using FluentBuild.BuildExe;

using NUnit.Framework;

namespace FluentBuild.Tests.BuildExeTexts
{
    [TestFixture]
    public class CommandLineParserTests
    {
        [Test]
        public void ShouldParseBuildClass()
        {
            var subject = new CommandLineParser(new[] {"SourceFolder", "-c:NotDefault"});
            Assert.That(subject.ClassToRun, Is.EqualTo("NotDefault"));
        }

        [Test]
        public void ShouldParseDllProperly()
        {
            var subject = new CommandLineParser(new[] {"test.dll"});
            Assert.That(subject.PathToBuildDll, Is.StringContaining("test.dll"));
        }

        [Test]
        public void ShouldParseMultipleProperties()
        {
            var subject = new CommandLineParser(new[] {"SourceFolder", "-p:Test=32", "-p:Test2=Sam"});
            Assert.That(Properties.CommandLineProperties.GetProperty("Test"), Is.EqualTo("32"));
            Assert.That(Properties.CommandLineProperties.GetProperty("Test2"), Is.EqualTo("Sam"));
        }

        [Test]
        public void ShouldParseProperty()
        {
            var subject = new CommandLineParser(new[] {"SourceFolder", "-p:Test3=32"});
            Assert.That(Properties.CommandLineProperties.GetProperty("Test3"), Is.EqualTo("32"));
        }

        [Test]
        public void ShouldParseSourceProperly()
        {
            var subject = new CommandLineParser(new[] {"SourceFolder"});
            Assert.That(subject.PathToBuildSources, Is.StringContaining("SourceFolder"));
        }

        [Test]
        public void ShouldParseMethodsToRunProperly()
        {
            var subject = new CommandLineParser(new[] { "SourceFolder", "-m:Method1", "-m:Method2" });
            Assert.That(subject.MethodsToRun[0], Is.EqualTo("Method1"));
            Assert.That(subject.MethodsToRun[1], Is.EqualTo("Method2"));
        }

        

    }
}