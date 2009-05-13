using System;

namespace FluentBuild.BuildFile
{
    internal class MainBuildTask 
    {
        internal BuildFolder directory_base;
        internal BuildFolder directory_compile;
        internal BuildFolder directory_functional_tests;
        internal BuildFolder directory_tools;

        private BuildArtifact assembly_FluentBuild;
        private BuildArtifact assembly_Functional_Tests;
        private BuildArtifact thirdparty_nunit;
        private BuildArtifact thirdparty_rhino;

        public void Execute()
        {
            directory_base = new BuildFolder(Environment.CurrentDirectory).SubFolder("..\\");
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");
            directory_functional_tests = directory_compile.SubFolder("functional");

            assembly_FluentBuild = directory_compile.FileName("FluentBuild.dll");
            assembly_Functional_Tests = directory_functional_tests.FileName("FluentBuild_Functional_Tests.dll");

            thirdparty_nunit = directory_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = directory_compile.FileName("rhino.mocks.dll");

            Clean();
            CompileSources();
            RunTests();
            CompileFunctionalTests();
            RunFunctionalTests();
        }

        private void Clean()
        {
            directory_compile.Delete().Create();
            directory_functional_tests.Delete().Create();
        }

        private void CompileSources()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                .Include(directory_tools.RecurseAllSubFolders().FileName("rhino.mocks.dll"))
                .CopyTo(directory_compile);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit).OutputFileTo(assembly_FluentBuild).Execute();
        }

        private void CompileFunctionalTests()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                .Include(assembly_FluentBuild)
                .CopyTo(directory_functional_tests);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("tests").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild).OutputFileTo(assembly_Functional_Tests).Execute();
        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").FileName("nunit-console.exe")).WithArguments(assembly_FluentBuild.ToString()).Execute();
        }

        private void RunFunctionalTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").FileName("nunit-console.exe")).WithArguments(assembly_Functional_Tests.ToString()).Execute();
        }
    }
}