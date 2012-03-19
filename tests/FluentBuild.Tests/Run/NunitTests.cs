using System;
using System.Configuration;
using System.IO;
using FluentBuild.Core;
using FluentBuild.Runners;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class NunitTests : TestBase
    {
        private string _outputFile;
        private string _pathToProjectRoot;

        [SetUp]
        public void Setup()
        {
            _outputFile = rootFolder + "\\sample.xml";
            _pathToProjectRoot = Settings.PathToRootFolder;
            Console.WriteLine(_pathToProjectRoot);
        }

        [Test]
        public void ShouldRunProperly()
        {
            Core.Run.UnitTestFramework.NUnit
                .FileToTest(Settings.PathToSamplesFolder  + @"\\Run\SimpleTestAssembly\Sample.Test.dll")
                .XmlOutputTo(_outputFile)
                .PathToNunitConsoleRunner(_pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe")
                .Execute();
            Assert.That(File.Exists(_outputFile));
        }

        [Test, ExpectedException(typeof(ExecutableFailedException))]
        public void ShouldFailIfErrorOccurs()
        {   
            //test fail/continue on error
            Core.Run.UnitTestFramework.NUnit
                .FileToTest("nonexistant.dll")
                .XmlOutputTo(_outputFile)
                .PathToNunitConsoleRunner(_pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe")
                .Execute();
            Assert.That(File.Exists(_outputFile));
        }

        [Test]
        public void ShouldSucceedIfFailOnErrorIsContinue()
        {
            //test fail/continue on error
            Core.Run.UnitTestFramework.NUnit
                .FileToTest("nonexistant.dll")
                .XmlOutputTo(_outputFile)
                .PathToNunitConsoleRunner(_pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe")
                .ContinueOnError
                .Execute();
        }

    }
}