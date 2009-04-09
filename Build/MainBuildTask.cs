using System;
using FluentBuild;

namespace Build
{
    internal class MainBuildTask
    {
        private string directory_base;
        private string directory_compile;
        private string assembly_FluentBuild;
        private string assembly_FluentBuild_Tests;
        private string thirdparty_rhino;
        private string thirdparty_nunit;
        private string directory_tools;

        public void Execute()
        {

            directory_base = Environment.CurrentDirectory;
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");
            assembly_FluentBuild = directory_compile.FileName("FluentBuild.dll");
            assembly_FluentBuild_Tests = directory_compile.FileName("FluentBuild.Tests.dll");
            thirdparty_nunit = directory_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = directory_compile.FileName("rhino.mocks.dll");
            
            DirectoryUtility.RecreateDirectory(directory_compile);
            CompileSources();
          //  CompileFunctionalTests();
            RunTests();
        }
        
        private void CompileSources()
        {
            var tools = new FileSet().Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                                    .Include(directory_tools.RecurseAllSubFolders().FileName("rhino.mocks.dll"));
            DirectoryUtility.CopyAllFiles(tools, directory_compile);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src").RecurseAllSubFolders().FileName("*.cs"));
            CreateBuildTask.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit).OutputFileTo(assembly_FluentBuild).Execute();
        }

        private void CompileFunctionalTests()
        {
            var tools = new FileSet().Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                                     .Include(directory_tools.RecurseAllSubFolders().FileName("rhino.mocks.dll"));

            DirectoryUtility.CopyAllFiles(tools, directory_compile);
            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("tests").RecurseAllSubFolders().FileName("*.cs"));
            CreateBuildTask.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild).OutputFileTo(assembly_FluentBuild_Tests).Execute();
        }

        private void RunTests()
        {
            ProcessUtility.StartProcess(directory_tools.SubFolder("nunit").FileName("nunit-console.exe"), assembly_FluentBuild);
        }

      
    }
}