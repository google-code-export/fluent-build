using System;

namespace FluentBuild.BuildFile
{
    internal class MainBuildTask
    {
        private string directory_base;
        private string directory_compile;
        private string assembly_FluentBuild;
        private string thirdparty_rhino;
        private string thirdparty_nunit;
        private string directory_tools;

        public void Execute()
        {

            directory_base =Environment.CurrentDirectory.SubFolder("..\\");
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");
            assembly_FluentBuild = directory_compile.FileName("FluentBuild.dll");
            thirdparty_nunit = directory_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = directory_compile.FileName("rhino.mocks.dll");
            
            DirectoryUtility.RecreateDirectory(directory_compile);
            CompileSources();
            RunTests();
        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").FileName("nunit-console.exe")).WithArguments(assembly_FluentBuild).Execute();
        }

        private void CompileSources()
        {
            var tools = new FileSet().Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                .Include(directory_tools.RecurseAllSubFolders().FileName("rhino.mocks.dll"));
            Copy.From(tools).To(directory_compile);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit).OutputFileTo(assembly_FluentBuild).Execute();
        }
    }
}