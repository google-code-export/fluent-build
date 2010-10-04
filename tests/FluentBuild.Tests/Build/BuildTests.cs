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
        public abstract TargetType CreateBuildTask();

        public abstract void ShouldCompileBasicAssembly();
        internal void Actual_ShouldCompileBasicAssembly()
        {
            //Using Resource
            var outputFileLocation = rootFolder + "\\temp.dll";
            CreateBuildTask().Target.Library.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileBasicAssemblyWithDebugSymbols();
        internal void Actual_ShouldCompileBasicAssemblyWithDebugSymbols()
        {
            var outputFileLocation = rootFolder + "\\temp.dll";
            CreateBuildTask().Target.Library.AddSources(GetBasicSources()).IncludeDebugSymbols.OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
            Assert.That(File.Exists(rootFolder + "\\temp.pdb"));
        }

        public abstract void ShouldCompileConsoleApplication();
        internal void Actual_ShouldCompileConsoleApplication()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().Target.Executable.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileModule();
        internal void Actual_ShouldCompileModule()
        {
            var outputFileLocation = rootFolder + "\\temp.netmodule";
            CreateBuildTask().Target.Module.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWindowsExe();
        internal void Actual_ShouldCompileWindowsExe()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().Target.WindowsExecutable.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWithReference();
        internal void Actual_ShouldCompileWithReference()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().Target.Library.AddSources(GetBasicSources())
                .AddRefences(Settings.PathToRootFolder + "\\Tools\\nunit\\nunit.framework.dll")
                .OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWithResource();
        internal void Actual_ShouldCompileWithResource()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            CreateBuildTask().Target.Library.AddSources(GetBasicSources())
                .AddResource(Settings.PathToRootFolder + "\\WithReference\\C#\\test.resource")
                .OutputFileTo(outputFileLocation).Execute();
            Assert.That(File.Exists(outputFileLocation));
        }
    }
}