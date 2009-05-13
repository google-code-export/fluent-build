using System;

namespace FluentBuild.BuildFile
{
    public abstract class ApplicationAndTestsInSeperateFolder
    {
        internal BuildFolder directory_base;
        internal BuildFolder directory_compile;
        internal BuildFolder directory_tests;
        internal BuildFolder directory_tools;

        internal BuildArtifact assembly;
        internal BuildArtifact assembly_tests;

        public virtual void Execute(string assemblyOutputFileName, string testOutputFileName)
        {
            directory_base = new BuildFolder(Environment.CurrentDirectory).SubFolder("..\\");
            directory_compile = directory_base.SubFolder("compile");
            directory_tools = directory_base.SubFolder("tools");

            assembly = directory_compile.FileName(assemblyOutputFileName);
            assembly_tests = directory_tests.FileName(testOutputFileName);

            thirdparty_nunit = directory_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = directory_compile.FileName("rhino.mocks.dll");

            Clean();
            CompileSources();
            CompileTests();
            RunTests();
        }

        private void Clean()
        {
            directory_compile.Delete().Create();
        }

        private void CompileSources()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                .Include(directory_tools.RecurseAllSubFolders().FileName("rhino.mocks.dll"))
                .CopyTo(directory_compile);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit).OutputFileTo(assembly).Execute();
        }

        private void CompileTests()
        {
            new FileSet()
                .Include(directory_tools.RecurseAllSubFolders().FileName("nunit.framework.dll"))
                .Include(assembly)
                .CopyTo(directory_compile);

            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("tests").RecurseAllSubFolders().FileName("*.cs"));
            Build.UsingCsc.AddSources(sourceFiles).AddRefences(thirdparty_rhino, thirdparty_nunit, assembly).OutputFileTo(assembly_tests).Execute();
        }

        private void RunTests()
        {
            Run.Executeable(directory_tools.SubFolder("nunit").FileName("nunit-console.exe")).WithArguments(assembly.ToString()).Execute();
        }
    }
}