using System;
using System.IO;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class NunitTests : TestBase
    {
        [Test]
        public void ShouldRunProperly()
        {
            var outputFile = rootFolder + "\\sample.xml";
            var pathToProjectRoot = Environment.CurrentDirectory + "\\..\\..\\..\\..\\";
            Core.Run.UnitTestFramework.NUnit
                .FileToTest(Environment.CurrentDirectory + @"..\..\..\Run\Samples\SimpleTestAssembly\Sample.Test.dll")
                .XmlOutputTo(outputFile)
                .PathToNunitConsoleRunner(pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe")
                .Execute();
            Assert.That(File.Exists(outputFile));
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void ShouldFailIfErrorOccurs()
        {
            //test fail/continue on error
            var outputFile = rootFolder + "\\sample.xml";
            var pathToProjectRoot = Environment.CurrentDirectory + "\\..\\..\\..\\..\\";
            Core.Run.UnitTestFramework.NUnit
                .FileToTest("nonexistant.dll")
                .XmlOutputTo(outputFile)
                .PathToNunitConsoleRunner(pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe")
                .Execute();
            Assert.That(File.Exists(outputFile));
        }

        [Test]
        public void ShouldSucceedIfFailOnErrorIsContinue()
        {
            //test fail/continue on error
            var outputFile = rootFolder + "\\sample.xml";
            var pathToProjectRoot = Environment.CurrentDirectory + "\\..\\..\\..\\..\\";
            Core.Run.UnitTestFramework.NUnit
                .FileToTest("nonexistant.dll")
                .XmlOutputTo(outputFile)
                .PathToNunitConsoleRunner(pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe")
                .ContinueOnError
                .Execute();
        }

    }
}