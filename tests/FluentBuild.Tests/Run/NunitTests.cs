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
            Task.Run.UnitTestFramework.Nunit(x => x.FileToTest(Settings.PathToSamplesFolder + @"\\Run\SimpleTestAssembly\Sample.Test.dll")
                .XmlOutputTo(_outputFile)
                .PathToNunitConsoleRunner(_pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe"));
            Assert.That(File.Exists(_outputFile));
        }


        [Test, ExpectedException(typeof(ExecutableFailedException))]
        public void ShouldFailIfErrorOccurs()
        {   
            //test fail/continue on error
           Task.Run.UnitTestFramework.Nunit(x=>x.FileToTest("nonexistant.dll")
                .XmlOutputTo(_outputFile)
                .PathToNunitConsoleRunner(_pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe"));
            Assert.That(File.Exists(_outputFile));
        }

        [Test]
        public void ShouldSucceedIfFailOnErrorIsContinue()
        {
            //test fail/continue on error
            Task.Run.UnitTestFramework.Nunit(x=>x.FileToTest("nonexistant.dll")
                .XmlOutputTo(_outputFile)
                .ContinueOnError
                .PathToNunitConsoleRunner(_pathToProjectRoot + "\\tools\\nunit\\nunit-console.exe"));
        }

    }
}