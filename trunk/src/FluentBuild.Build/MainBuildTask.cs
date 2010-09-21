using System;
using FluentBuild;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace Build
{
    public class MainBuildTask : BuildFile
    {
        private readonly BuildArtifact assembly_FluentBuild_WithTests;
        private readonly BuildArtifact assembly_FluentBuild_Release;
        private readonly BuildArtifact assembly_Functional_Tests;
        private readonly BuildFolder directory_base;
        private readonly BuildFolder directory_compile;
        private readonly BuildFolder directory_functional_tests;
        private readonly BuildFolder directory_tools;

        private readonly BuildArtifact thirdparty_nunit;
        private readonly BuildArtifact thirdparty_rhino;

        public MainBuildTask()
        {
            //FluentBuild.Core.Properties.CommandLineProperties.Add();
            //var x = new FluentBuild.ApplicationProperties.CommandLineProperties();
            

            directory_base = new BuildFolder(Environment.CurrentDirectory).SubFolder("..\\");
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");
            directory_functional_tests = directory_compile.SubFolder("functional");

            assembly_FluentBuild_WithTests = directory_compile.File("FluentBuildWithTests.dll");
            assembly_FluentBuild_Release = directory_compile.File("FluentBuild.dll");
            assembly_Functional_Tests = directory_functional_tests.File("FluentBuild_Functional_Tests.dll");

            thirdparty_nunit = directory_compile.File("nunit.framework.dll");
            thirdparty_rhino = directory_compile.File("rhino.mocks.dll");

            directory_compile.Delete(OnError.Continue).Create();
            directory_functional_tests.Delete(OnError.Continue).Create();

            AddTask(CompileSourcesWithTests);
            AddTask(RunTests);
            AddTask(CompileFunctionalTests);
            AddTask(RunFunctionalTests);
            //if all this passes then recompile the dll without tests included
            AddTask(CompileSourcesWithOutTests);
        }


        private void CompileSourcesWithTests()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().File("nunit.framework.dll"))
                .Include(directory_tools.RecurseAllSubFolders().File("rhino.mocks.dll"))
                .Copy.To(directory_compile);

            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src")
                             .RecurseAllSubFolders()
                             .File("*.cs"));

            FluentBuild.Core.Build.UsingCsc
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_rhino, thirdparty_nunit)
                .OutputFileTo(assembly_FluentBuild_WithTests)
                .Execute();


        }

        private void CompileSourcesWithOutTests()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src")
                             .RecurseAllSubFolders()
                             .File("*.cs"))
                 .Exclude("*Tests.cs");

            FluentBuild.Core.Build.UsingCsc
                .AddSources(sourceFiles)
                .OutputFileTo(assembly_FluentBuild_Release)
                .Execute();


        }

        private void CompileFunctionalTests()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().File("nunit.framework.dll"))
                .Include(assembly_FluentBuild_WithTests)
                .Copy.To(directory_functional_tests);

            FileSet sourceFiles =new FileSet().Include(directory_base.SubFolder("tests").RecurseAllSubFolders().File("*.cs"));
            FluentBuild.Core.Build.UsingCsc
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild_WithTests)
                .OutputFileTo(assembly_Functional_Tests)
                .Execute();
        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").File("nunit-console.exe")).WithArguments(assembly_FluentBuild_WithTests.ToString()).Execute();
        }

        private void RunFunctionalTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").File("nunit-console.exe")).WithArguments(
                assembly_Functional_Tests.ToString()).Execute();
        }
    }
}