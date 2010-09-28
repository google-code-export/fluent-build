﻿using System;
using System.IO;
using FluentBuild.Compilation;
using FluentBuild.Core;
using NUnit.Framework;

namespace FluentBuild.Tests.Build
{
    //NUNIT does not seem to process the [Test] attribute in base classes. 
    //To deal with this an abstract class is created so the inheritor can
    //attribute it with [Test] and then call the underlying method that 
    //does the actual work

    public abstract class BuildTests : TestBase
    {
        public abstract FileSet GetBasicSources();
        public abstract FileSet GetWithReferenceSources();
        public abstract BuildTask CreateBuildTask();

        public abstract void ShouldCompileBasicAssembly();
        internal void Actual_ShouldCompileBasicAssembly()
        {
            //Using Resource
            var outputFileLocation = rootFolder + "\\temp.dll";
            CreateBuildTask().AddSources(GetBasicSources()).Target.Library.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileBasicAssemblyWithDebugSymbols();
        internal void Actual_ShouldCompileBasicAssemblyWithDebugSymbols()
        {
            var outputFileLocation = rootFolder + "\\temp.dll";
            CreateBuildTask().AddSources(GetBasicSources()).Target.Library.IncludeDebugSymbols.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
            Assert.That(File.Exists(rootFolder + "\\temp.pdb"));
        }

        public abstract void ShouldCompileConsoleApplication();
        internal void Actual_ShouldCompileConsoleApplication()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().AddSources(GetBasicSources()).Target.Executable.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileModule();
        internal void Actual_ShouldCompileModule()
        {
            var outputFileLocation = rootFolder + "\\temp.netmodule";
            CreateBuildTask().AddSources(GetBasicSources()).Target.Module.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWindowsExe();
        internal void Actual_ShouldCompileWindowsExe()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().AddSources(GetBasicSources()).Target.WindowsExecutable.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWithReference();
        internal void Actual_ShouldCompileWithReference()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().AddSources(GetBasicSources())
                .AddRefences(Environment.CurrentDirectory + "\\..\\..\\..\\..\\Tools\\nunit\\nunit.framework.dll")
                .Target.Library.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWithResource();
        internal void Actual_ShouldCompileWithResource()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().AddSources(GetBasicSources())
                .AddResource(Environment.CurrentDirectory + "\\..\\..\\Build\\Samples\\WithReference\\C#\\test.resource")
                .Target.Library.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }
    }

    [TestFixture]
    public class BuildUsingCscTests : BuildTests
    {
        public override FileSet GetBasicSources()
        {
            var fileSet = new FileSet();
            fileSet.Include(Environment.CurrentDirectory + "\\..\\..\\Build\\Samples\\Simple\\C#\\*.cs");
            return fileSet;
        }

        public override FileSet GetWithReferenceSources()
        {
            var fileSet = new FileSet();
            fileSet.Include(Environment.CurrentDirectory + "\\..\\..\\Build\\Samples\\WithReference\\C#\\*.cs");
            return fileSet;
        }

        public override BuildTask CreateBuildTask()
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