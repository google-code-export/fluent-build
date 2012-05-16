using System;
using FluentBuild;
using FluentBuild.Compilation;
using FluentBuild.Core;
using FluentBuild.Runners;
using FluentFs.Core;


namespace Build
{
    public class Publish : Default
    {
        internal readonly File AssemblyFluentBuildRelease_Merged;
        internal readonly File AssemblyFluentBuildRelease_Partial;
        internal readonly File AssemblyFluentBuildRunnerRelease;
        internal File ZipFilePath;
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
            //AddTask(PublishToRepository);
        }

        private void CompileRunner()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild.BuildExe"))
                .RecurseAllSubDirectories.Filter("*.cs");

          Task.Build(Using.Csc.Target.Executable
                .AddSources(sourceFiles)
                .AddRefences(AssemblyFluentBuildRelease_Merged)
                .OutputFileTo(AssemblyFluentBuildRunnerRelease));
        }

        private void PublishToRepository()
        {
            Task.Publish.ToGoogleCode(x=>x.LocalFileName(ZipFilePath.ToString())
                .UserName(Properties.CommandLineProperties.GetProperty("GoogleCodeUsername"))
                .Password(Properties.CommandLineProperties.GetProperty("GoogleCodePassword"))
                .ProjectName("fluent-build")
                .Summary("Alpha Release (v" + _version + ")")
                .TargetFileName(_finalFileName));
        }

        private void CompileBuildFileConverterWithoutTests()
        {
            var sourceFiles = new FileSet();
            sourceFiles.Include(directory_src_converter).RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(directory_src_converter).RecurseAllSubDirectories.Filter("*Tests.cs"); ;

           Task.Build(Using.Csc.Target.Executable
                .AddSources(sourceFiles)
                .OutputFileTo(assembly_BuildFileConverter_WithTests));
        }

        private void CompileCoreWithOutTests()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild"))
                .RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(directory_base.SubFolder("src").SubFolder("FluentBuild"))
                .RecurseAllSubDirectories.Filter("*Tests.cs");

            Task.Build(Using.Csc.Target.Library
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_sharpzip, thirdparty_fluentFs)
                .OutputFileTo(AssemblyFluentBuildRelease_Partial));

           Task.Run.ILMerge(x=>x.ExecutableLocatedAt(@"tools\ilmerge\ilmerge.exe")
                .AddSource(AssemblyFluentBuildRelease_Partial)
                .AddSource(thirdparty_sharpzip)
                .AddSource(thirdparty_fluentFs)
                .OutputTo(AssemblyFluentBuildRelease_Merged));

            //now that it is merged delete the partial file
            AssemblyFluentBuildRelease_Partial.Delete();
        }

        private void Compress()
        {
            Task.Run.Zip(x=>x.Compress.SourceFolder(directory_compile).To(ZipFilePath));
        }
    }
}