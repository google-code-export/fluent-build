using System;

namespace FluentBuild.BuildFile
{
    public class MainBuildTask : IBuild
    {
        private BuildFolder directory_base;
        private BuildFolder directory_compile;
        private BuildFolder directory_tools;
        private BuildFolder directory_functional_tests;

        private BuildArtifact assembly_FluentBuild;
        private BuildArtifact thirdparty_rhino;
        private BuildArtifact thirdparty_nunit;
        private BuildArtifact assembly_Functional_Tests;

        public void Execute()
        {
            directory_base = new BuildFolder(Environment.CurrentDirectory).SubFolder("..\\");
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");
            directory_functional_tests = directory_compile.SubFolder("functional");

            assembly_FluentBuild = directory_compile.File("FluentBuild.dll");
            assembly_Functional_Tests = directory_functional_tests.File("FluentBuild_Functional_Tests.dll");

            thirdparty_nunit = directory_compile.File("nunit.framework.dll");
            thirdparty_rhino = directory_compile.File("rhino.mocks.dll");

            MessageLogger.WriteHeader("Setup Directories");
            directory_compile.Delete().Create();
            directory_functional_tests.Delete().Create();

            MessageLogger.WriteHeader("Compile Sources");
            CompileSources();
            MessageLogger.WriteHeader("Run Tests");
            RunTests();
            MessageLogger.WriteHeader("Compile Functional Tests");
            CompileFunctionalTests();
            MessageLogger.WriteHeader("Run Functional Tests");
            RunFunctionalTests();

        }
        
        private void CompileSources()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().File("nunit.framework.dll"))
                .Include(directory_tools.RecurseAllSubFolders().File("rhino.mocks.dll"))
                .Copy.To(directory_compile);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src").RecurseAllSubFolders().File("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit).OutputFileTo(assembly_FluentBuild).Execute();
        }

        private void CompileFunctionalTests()
        {
            new FileSet()
             .Include(directory_tools.RecurseAllSubFolders().File("nunit.framework.dll"))
             .Include(assembly_FluentBuild)
             .Copy.To(directory_functional_tests);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("tests").RecurseAllSubFolders().File("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild).OutputFileTo(assembly_Functional_Tests).Execute();

        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").File("nunit-console.exe")).WithArguments(assembly_FluentBuild.ToString()).Execute();
        }

        private void RunFunctionalTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").File("nunit-console.exe")).WithArguments(assembly_Functional_Tests.ToString()).Execute();
        }

      
    }
}