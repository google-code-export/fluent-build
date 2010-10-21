using System;
using System.Diagnostics;
using System.IO;
using FluentBuild;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace Build
{
    public class Default : BuildFile
    {
        private readonly BuildArtifact assembly_FluentBuild_WithTests;
        private readonly BuildArtifact assembly_Functional_Tests;
        private readonly BuildArtifact assembly_FluentBuild_Runner;

        internal readonly BuildFolder directory_base;
        internal readonly BuildFolder directory_compile;
        internal readonly BuildFolder directory_release;

        private readonly BuildFolder directory_tools;
        private BuildFolder directory_src_core;
        private BuildFolder directory_src_runner;

        private readonly BuildArtifact thirdparty_nunit;
        private readonly BuildArtifact thirdparty_rhino;
        internal string _version;
        internal BuildArtifact thirdparty_sharpzip;


        public Default()
        {
            directory_base = new BuildFolder(Environment.CurrentDirectory);
            directory_compile = directory_base.SubFolder("compile");
            directory_release = directory_base.SubFolder("release");
            directory_tools = directory_base.SubFolder("tools");
            directory_src_core = directory_base.SubFolder("src").SubFolder("FluentBuild");
            directory_src_runner = directory_base.SubFolder("src").SubFolder("FluentBuild.BuildExe");

            assembly_FluentBuild_WithTests = directory_compile.File("FluentBuildWithTests.dll");
            assembly_Functional_Tests = directory_compile.File("FluentBuild_Functional_Tests.dll");

            assembly_FluentBuild_Runner = directory_compile.File("fb.exe");

            thirdparty_nunit = directory_tools.SubFolder("nunit").File("nunit.framework.dll");
            thirdparty_rhino = directory_tools.SubFolder("rhino").File("rhino.mocks.dll");
            thirdparty_sharpzip = directory_base.SubFolder("lib").SubFolder("SharpZipLib-net2.0").File("ICSharpCode.SharpZipLib.dll");

            _version = "0.0.0.0";

            Defaults.FrameworkVersion = FrameworkVersion.NET4_0.Full;

            AddTask(Clean);
            AddTask(GenerateAssemblyInfoFiles);
            AddTask(CopyDependantAssembliesToCompileDir);
            AddTask(CompileCoreSources);
            AddTask(CompileRunnerSources);
            AddTask(RunTests);
            AddTask(CompileFunctionalTests);
            //AddTask(RunFunctionalTests);      
        }

        private void CopyDependantAssembliesToCompileDir()
        {
            new FileSet()
                .Include(thirdparty_nunit)
                .Include(thirdparty_rhino)
                .Include(thirdparty_sharpzip)
                .Copy.To(directory_compile);
        }


        private void GenerateAssemblyInfoFiles()
        {
            GenerateAssemblyInfoFor("FluentBuild", "Core FluentBuild assembly");
            GenerateAssemblyInfoFor("FluentBuild.BuildExe", "FluentBuild Build Runner");
        }

        private void GenerateAssemblyInfoFor(string folder, string description)
        {
             AssemblyInfo.Language.CSharp.ClsCompliant(true)
                .Company("Solidhouse")
                .ComVisible(false)
                .Copyright("Copyright 2009-" + DateTime.Now.Year)
                .Description(description)
                .Product("FluentBuild")
                .Version(_version)
                .OutputTo(directory_base.SubFolder("src").SubFolder(folder).SubFolder("Properties").File("AssemblyInfo.cs"));
        }

        internal void Clean()
        {
            directory_compile.Delete(OnError.Continue).Create();
            directory_release.Delete(OnError.Continue).Create();
        }


        private void CompileCoreSources()
        {
            var sourceFiles = new FileSet();
            sourceFiles.Include(directory_src_core).RecurseAllSubDirectories.Filter("*.cs");

            FluentBuild.Core.Build.UsingCsc.Target.Library
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_rhino, thirdparty_nunit, thirdparty_sharpzip)
                .OutputFileTo(assembly_FluentBuild_WithTests)
                .IncludeDebugSymbols
                .Execute();
        }

        private void CompileRunnerSources()
        {
            FileSet sourceFiles = new FileSet()
                             .Include(directory_src_runner)
                             .RecurseAllSubDirectories.Filter("*.cs");

            FluentBuild.Core.Build.UsingCsc.Target.Executable
                .AddSources(sourceFiles)
                .AddRefences(assembly_FluentBuild_WithTests)
                
                .OutputFileTo(assembly_FluentBuild_Runner)
                .Execute();
        }

        private void CompileFunctionalTests()
        {
            FileSet sourceFiles =new FileSet().Include(directory_base.SubFolder("tests")).RecurseAllSubDirectories.Filter("*.cs");
            FluentBuild.Core.Build.UsingCsc.Target.Library
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild_WithTests)
                .OutputFileTo(assembly_Functional_Tests)
                .Execute();
        }

        private void RunTests()
        {
           //Run.Debugger();
           Run.UnitTestFramework.NUnit.FileToTest(assembly_FluentBuild_WithTests).Execute();
        }

        private void RunFunctionalTests()
        {
            //Copy sample folder to compile directory
            var sampleData = directory_base.SubFolder("tests").SubFolder("FluentBuild.Tests").SubFolder("Samples");
            Run.Executeable(@"C:\Windows\System32\xcopy.exe").WithArguments(sampleData.ToString(), directory_compile.SubFolder("Samples").ToString(), "/E /I").Execute();
            var configSource = directory_base.SubFolder("tests").SubFolder("FluentBuild.Tests").File("app.config.template");
            var configDestination = directory_compile.File("FluentBuild_Functional_Tests.dll.config");
            configSource.Copy
                .ReplaceToken("RelativeRoot").With("..\\")
                .ReplaceToken("RelativeSamples").With(@"Samples")
                .To(configDestination.ToString());

            Run.UnitTestFramework.NUnit.FileToTest(assembly_Functional_Tests).Execute();
        }
    }
}
