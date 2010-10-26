using FluentBuild.Core;
using FluentBuild.Utilities;

namespace FluentBuild.BuildFile
{
    public class Default : Core.BuildFile
    {
        private readonly BuildArtifact assembly_FluentBuild;
        private readonly BuildArtifact assembly_FluentBuild_Tests;
        private readonly BuildFolder directory_base;
        private readonly BuildFolder directory_compile;
        private readonly BuildFolder directory_release;
        private readonly BuildFolder directory_tools;

        private readonly BuildArtifact thirdparty_nunit;
        private readonly BuildArtifact thirdparty_rhino;

        public Default()
        {
            directory_base = new BuildFolder(Properties.CurrentDirectory);
            directory_compile = directory_base.SubFolder("compile");
            directory_release = directory_base.SubFolder("release");
            directory_tools = directory_base.SubFolder("tools");

            assembly_FluentBuild = directory_compile.File("FluentBuild.dll");
            assembly_FluentBuild_Tests = directory_compile.File("FluentBuild.Tests.dll");
            thirdparty_nunit = directory_compile.File("nunit.framework.dll");
            thirdparty_rhino = directory_compile.File("rhino.mocks.dll");

            AddTask(Clean);
            AddTask(CompileSources);
            AddTask(CompileTests);
            AddTask(RunTests);
            AddTask(Package);

            //set the verbosity. Can also be set via command line
            MessageLogger.Verbosity = VerbosityLevel.TaskNamesOnly;
        }

        private void Clean()
        {
            directory_compile.Delete(OnError.Continue).Create();

            //Turn on debugging messages for only this step
            using (MessageLogger.ShowDebugMessages)
            {
                directory_release.Delete(OnError.Continue).Create();
            }
        }

        private void Package()
        {
                Run.Zip.Compress
                    .SourceFolder(directory_compile)
                    .UsingCompressionLevel.Nine
                    .To(directory_release.File("release.zip"));
        }

        private void CompileSources()
        {
            FileSet sourceFiles = new FileSet().Include(directory_base.SubFolder("src"))
                .RecurseAllSubDirectories
                .Filter("*.cs");

            Core.Build.UsingCsc.Target.Library.AddSources(sourceFiles)
                .OutputFileTo(assembly_FluentBuild).Execute();
        }

        private void CompileTests()
        {
            new FileSet()
                .Include(directory_tools).RecurseAllSubDirectories.Filter("nunit.framework.dll")
                .Include(directory_tools).RecurseAllSubDirectories.Filter("rhino.mocks.dll")
                .Copy.To(directory_compile);

            FileSet sourceFiles =
                new FileSet().Include(directory_base.SubFolder("tests")).RecurseAllSubDirectories.Filter("*.cs");
            Core.Build.UsingCsc
                .Target.Library
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild)
                .OutputFileTo(assembly_FluentBuild_Tests)
                .Execute();
        }

        private void RunTests()
        {
            Run.UnitTestFramework.NUnit.FileToTest(assembly_FluentBuild.ToString()).Execute();
        }
    }
}