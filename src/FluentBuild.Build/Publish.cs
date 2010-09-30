using FluentBuild.Core;
using FluentBuild.Publishing;

namespace Build
{
    public class Publish : Default
    {
        internal readonly BuildArtifact AssemblyFluentBuildRelease;
        internal readonly BuildArtifact AssemblyFluentBuildRunnerRelease;
        private readonly string _finalFileName;
        internal BuildArtifact ZipFilePath;

        public Publish()
        {
            AssemblyFluentBuildRelease = directory_compile.File("FluentBuild.dll");
            AssemblyFluentBuildRunnerRelease = directory_compile.File("fb.exe");
            _finalFileName = "FluentBuild-Alpha-" + _version + ".zip";
            ZipFilePath = directory_release.File(_finalFileName);

            AddTask(Clean);
            AddTask(CompileCoreWithOutTests);
            AddTask(CompileRunner);
            AddTask(Compress);
            //move to tools folder here?
            AddTask(PublishToRepository);
        }

        private void CompileRunner()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild.BuildExe")
                             .RecurseAllSubFolders()
                             .File("*.cs"));

            FluentBuild.Core.Build.UsingCsc
                .AddSources(sourceFiles)
                .AddRefences(AssemblyFluentBuildRelease)
                .Target.Executable
                .OutputFileTo(AssemblyFluentBuildRunnerRelease)
                .Execute();
        }

        private void PublishToRepository()
        {
            //http://code.google.com/p/support/wiki/ScriptedUploads
            //http://code.google.com/p/nant-googlecode/
            //throw new NotImplementedException();
            var x = new GoogleCode();
            x.LocalFileName = ZipFilePath.ToString();
            //TODO: pass these in via the command line
            x.UserName = "dave@solidhouse.com";
            x.Password = "mW3za4Ku3wf6";
            x.ProjectName = "fluent-build";
            x.Summary = "Alpha Release (v" + _version + ")";
            x.TargetFileName = _finalFileName;
            x.Upload();
        }

        private void CompileCoreWithOutTests()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild")
                             .RecurseAllSubFolders()
                             .File("*.cs"))
                .Exclude(directory_base.SubFolder("src").SubFolder("FluentBuild")
                             .RecurseAllSubFolders()
                             .File("*Tests.cs").ToString());

            FluentBuild.Core.Build.UsingCsc
                .AddSources(sourceFiles)
                .AddRefences(thirdparty_sharpzip)
                .OutputFileTo(AssemblyFluentBuildRelease)
                .Target.Library
                .Execute();
        }

        private void Compress()
        {
            thirdparty_sharpzip.Copy.To(directory_compile);
            Run.Zip.Compress.SourceFolder(directory_compile).To(ZipFilePath);
        }
    }
}