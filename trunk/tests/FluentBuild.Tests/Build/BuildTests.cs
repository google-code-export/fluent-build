using System;
using System.Reflection;
using FluentBuild.Compilation;

using FluentFs.Core;
using NUnit.Framework;
using File = System.IO.File;
using System.Linq;

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
        //public abstract TargetType CreateBuildTask();

        public abstract void ShouldCompileBasicAssembly();
        internal void Actual_ShouldCompileBasicAssembly()
        {
            //Using Resource
            var outputFileLocation = rootFolder + "\\temp.dll";
            Task.Build.Csc.Target.Library(x => x.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileBasicAssemblyWithDebugSymbols();
        internal void Actual_ShouldCompileBasicAssemblyWithDebugSymbols()
        {
            var outputFileLocation = rootFolder + "\\temp.dll";
            Task.Build.Csc.Target.Library(x => x.AddSources(GetBasicSources()).IncludeDebugSymbols.OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
            Assert.That(File.Exists(rootFolder + "\\temp.pdb"));
        }

        public abstract void ShouldCompileConsoleApplication();
        internal void Actual_ShouldCompileConsoleApplication()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            Task.Build.Csc.Target.Executable(x => x.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileModule();
        internal void Actual_ShouldCompileModule()
        {
            var outputFileLocation = rootFolder + "\\temp.netmodule";
            Task.Build.Csc.Target.Module(x=>x.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWindowsExe();
        internal void Actual_ShouldCompileWindowsExe()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            Task.Build.Csc.Target.WindowsExecutable(x=>x.AddSources(GetBasicSources()).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWithReference();
        internal void Actual_ShouldCompileWithReference()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            Task.Build.Csc.Target.Library(x => x.AddSources(GetBasicSources())
                .AddRefences(Settings.PathToRootFolder + "\\Tools\\nunit\\nunit.framework.dll")
                .OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));
        }

        public abstract void ShouldCompileWithResource();
        internal void Actual_ShouldCompileWithResource()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            string resourceDirectory = rootFolder + "\\WithReference\\CSharp";
            string resourceFile = resourceDirectory + "\\test.resource";
            System.IO.Directory.CreateDirectory(resourceDirectory);
            File.Create(resourceFile).Dispose();

            Task.Build.Csc.Target.Library(x => x.AddSources(GetBasicSources())
                .AddResource(resourceFile)
                .OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));

            Assembly assembly = Assembly.Load(File.ReadAllBytes(outputFileLocation));
            Assert.That(assembly.GetManifestResourceNames().Contains("test.resource"), Is.True);
        }

        public abstract void ShouldCompileWithNamedResource();
        internal void Actual_ShouldCompileWithNamedResource()
        {
            var outputFileLocation = rootFolder + "\\temp.exe";
            string resourceDirectory = rootFolder + "\\WithReference\\CSharp";
            string resourceFile = resourceDirectory + "\\test.resource";
            System.IO.Directory.CreateDirectory(resourceDirectory);
            File.Create(resourceFile).Dispose();

            Task.Build.Csc.Target.Library(x => x.AddSources(GetBasicSources())
                .AddResource(resourceFile, "TestIdentifier")
                .OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));

            Assembly assembly = Assembly.Load(File.ReadAllBytes(outputFileLocation));
            Assert.That(assembly.GetManifestResourceNames().Contains("TestIdentifier"), Is.True);
        }
    }
}