using System;
using FluentBuild.Compilation;
using FluentBuild.Core;
using NUnit.Framework;

namespace FluentBuild.Tests.Build
{
    [TestFixture]
    public class BuildUsingCscTests : BuildTests
    {
        public override FileSet GetBasicSources()
        {
            var fileSet = new FileSet();
            fileSet.Include(Settings.PathToSamplesFolder + "\\Build\\Simple\\C#\\*.cs");
            return fileSet;
        }

        public override FileSet GetWithReferenceSources()
        {
            var fileSet = new FileSet();
            fileSet.Include(Settings.PathToSamplesFolder + "\\Build\\WithReference\\C#\\*.cs");
            return fileSet;
        }

        public override TargetType CreateBuildTask()
        {
            return Core.Build.UsingCsc;
        }

        [Test]
        public override void ShouldCompileBasicAssembly()
        {
            Actual_ShouldCompileBasicAssembly();
        }

        [Test]
        public override void ShouldCompileBasicAssemblyWithDebugSymbols()
        {
            Actual_ShouldCompileBasicAssemblyWithDebugSymbols();
        }

        [Test]
        public override void ShouldCompileConsoleApplication()
        {
            Actual_ShouldCompileConsoleApplication();
        }

        [Test]
        public override void ShouldCompileModule()
        {
            Actual_ShouldCompileModule();
        }

        [Test]
        public override void ShouldCompileWindowsExe()
        {
            Actual_ShouldCompileWindowsExe();
        }

        [Test]
        public override void ShouldCompileWithReference()
        {
            
            Actual_ShouldCompileWithReference();
        }

        public override void ShouldCompileWithResource()
        {
            Actual_ShouldCompileWithResource();
        }
    }
}