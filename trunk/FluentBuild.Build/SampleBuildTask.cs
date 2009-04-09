using System;

namespace FluentBuild.BuildFile
{
    public class SampleBuildTask
    {
        private BuildArtifact assembly_FluentBuild;
        private BuildArtifact assembly_FluentBuild_Tests;
        private BuildFolder directory_base;
        private BuildFolder directory_compile;
        private BuildFolder directory_tools;

        private BuildArtifact thirdparty_nunit;
        private BuildArtifact thirdparty_rhino;

        public void Execute()
        {
            directory_base = new BuildFolder(Environment.CurrentDirectory);
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");
            assembly_FluentBuild = directory_compile.FileName("FluentBuild.dll");
            assembly_FluentBuild_Tests = directory_compile.FileName("FluentBuild.Tests.dll");
            thirdparty_nunit = directory_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = directory_compile.FileName("rhino.mocks.dll");

            directory_compile.Delete().Create(); //recreate the compile directory
            CompileSources();
            CompileTests();
            RunTests();
        }

        private void CompileSources()
        {
            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).OutputFileTo(assembly_FluentBuild).Execute();
        }

        private void CompileTests()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                .Include(directory_tools.RecurseAllSubFolders().FileName("rhino.mocks.dll"))
                .CopyTo(directory_compile);
            
            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("tests").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild).OutputFileTo(assembly_FluentBuild_Tests).Execute();
        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").FileName("nunit-console.exe")).WithArguments(assembly_FluentBuild.ToString()).Execute();
        }
    }
}