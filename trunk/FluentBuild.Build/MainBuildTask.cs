using System;

namespace FluentBuild.BuildFile
{
    internal class MainBuildTask
    {
        private BuildFolder directory_base;
        private BuildFolder directory_compile;
        private BuildFolder directory_tools;

        private BuildArtifact assembly_FluentBuild;
        private BuildArtifact thirdparty_rhino;
        private BuildArtifact thirdparty_nunit;
        
        public void Execute()
        {
            directory_base = new BuildFolder(Environment.CurrentDirectory).SubFolder("..\\");
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");

            assembly_FluentBuild = directory_compile.FileName("FluentBuild.dll");
            thirdparty_nunit = directory_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = directory_compile.FileName("rhino.mocks.dll");

            directory_compile.Delete().Create();
            CompileSources();
            RunTests();
        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").FileName("nunit-console.exe")).WithArguments(assembly_FluentBuild.ToString()).Execute();
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
    }
}