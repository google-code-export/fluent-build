using System;
using FluentBuild.Core;

namespace Build
{
    public class Publish : Default
    {
        internal readonly BuildArtifact AssemblyFluentBuildRelease_Merged;
        internal readonly BuildArtifact AssemblyFluentBuildRelease_Partial;
        internal readonly BuildArtifact AssemblyFluentBuildRunnerRelease;
        internal BuildArtifact ZipFilePath;
        internal string _finalFileName;

        public Publish()
        {
            AssemblyFluentBuildRelease_Partial = directory_compile.File("FluentBuild-partial.dll");
            AssemblyFluentBuildRelease_Merged = directory_compile.File("FluentBuild.dll");
            AssemblyFluentBuildRunnerRelease = directory_compile.File("fb.exe");

            _version = "1.0.1.0";
            _finalFileName = "FluentBuild-Beta-" + _version + ".zip";
            ZipFilePath = directory_release.File(_finalFileName);

            AddTask(Clean);
            AddTask(CompileCoreWithOutTests);
            AddTask(CompileRunner);
            AddTask(CompileBuildFileConverterWithoutTests);
            AddTask(Compress);
            //move to tools folder here?
            AddTask(PublishToRepository);
        }

        private void CompileRunner()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild.BuildExe"))
                .RecurseAllSubDirectories.Filter("*.cs");

            FluentBuild.Core.Build.UsingCsc.Target.Executable
                .AddSources(sourceFiles)
                .AddRefences(AssemblyFluentBuildRelease_Merged)
                .OutputFileTo(AssemblyFluentBuildRunnerRelease)
                .Execute();
        }

        private void PublishToRepository()
        {
            FluentBuild.Core.Publish.ToGoogleCode.LocalFileName(ZipFilePath.ToString())
                .UserName(Properties.CommandLineProperties.GetProperty("GoogleCodeUsername"))
                .Password(Properties.CommandLineProperties.GetProperty("GoogleCodePassword"))
                .ProjectName("fluent-build")
                .Summary("Alpha Release (v" + _version + ")")
                .TargetFileName(_finalFileName)
                .Upload();
        }

        private void CompileBuildFileConverterWithoutTests()
        {
            var sourceFiles = new FileSet();
            sourceFiles.Include(directory_src_converter).RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(directory_src_converter).RecurseAllSubDirectories.Filter("*Tests.cs"); ;

            FluentBuild.Core.Build.UsingCsc.Target.Executable
                .AddSources(sourceFiles)
                .OutputFileTo(assembly_BuildFileConverter_WithTests)
                .Execute();
        }

        private void CompileCoreWithOutTests()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild"))
                .RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(directory_base.SubFolder("src").SubFolder("FluentBuild"))
                .RecurseAllSubDirectories.Filter("*Tests.cs");

            FluentBuild.Core.Build.UsingCsc.Target.Library
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_sharpzip)
                .OutputFileTo(AssemblyFluentBuildRelease_Partial)
                .Execute();

            Run.ILMerge
                .ExecutableLocatedAt(@"tools\ilmerge\ilmerge.exe")
                .AddSource(AssemblyFluentBuildRelease_Partial)
                .AddSource(thirdparty_sharpzip)
                .OutputTo(AssemblyFluentBuildRelease_Merged)
                .Execute();

            //now that it is merged delete the partial file
            AssemblyFluentBuildRelease_Partial.Delete();
        }

        private void Compress()
        {
            Run.Zip.Compress.SourceFolder(directory_compile).To(ZipFilePath);
        }
    }
}