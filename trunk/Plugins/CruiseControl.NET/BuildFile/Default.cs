using System;
using FluentBuild.Core;
using FluentBuild.Utilities;

namespace BuildFile
{
    public class Default : FluentBuild.Core.BuildFile
    {
        private BuildFolder dirBase;
        private BuildFolder dirCompile;
        private BuildFolder dirTools;
        private BuildFolder dirInstall;

        private BuildArtifact assemblyCompiledPlugin;
        private BuildArtifact assemblyFluentBuild;
        private BuildArtifact assemblyThoughtworksCore;
        private BuildArtifact assemblyNetReflector;
        

        public Default()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET3_5;

            dirBase = new BuildFolder(Environment.CurrentDirectory);
            dirCompile = dirBase.SubFolder("compile");
            dirTools = dirBase.SubFolder("tools");
            dirInstall = new BuildFolder(@"C:\Program Files (x86)\CruiseControl.NET\server");

            assemblyFluentBuild = dirTools.SubFolder("FluentBuild").File("fluentbuild.dll");
            assemblyThoughtworksCore = dirTools.SubFolder("CruiseControl").File("ThoughtWorks.CruiseControl.Core.dll");
            assemblyNetReflector = dirTools.SubFolder("CruiseControl").File("NetReflector.dll");
            assemblyCompiledPlugin = dirCompile.File("ccnet.fluentbuild.plugin.dll");

            AddTask(Clean);
            AddTask(Uninstall);
            AddTask(Compile);
            AddTask(Install);
        }

        private void Install()
        {
            assemblyFluentBuild.Copy.To(dirInstall);
            assemblyCompiledPlugin.Copy.To(dirInstall);
            Run.Executeable("net").WithArguments("stop", "CCService").Execute();
            Run.Executeable("net").WithArguments("start", "CCService").Execute();
        }

        private void Compile()
        {
            var sources = new FileSet().Include(dirBase.SubFolder("ccnet")).RecurseAllSubDirectories.Filter("*.cs");
            Build.UsingCsc
                .Target.Library
                .AddSources(sources)
                .AddRefences(assemblyFluentBuild, assemblyThoughtworksCore, assemblyNetReflector)
                .OutputFileTo(assemblyCompiledPlugin)
                .Execute();
        }

        private void Clean()
        {
            dirCompile.Delete(OnError.Continue).Create();
        }

        private void Uninstall()
        {
            dirInstall.File("fluentbuild.dll").Delete(OnError.Continue);
            dirInstall.File("ICSharpCode.SharpZipLib.dll").Delete(OnError.Continue);
            dirInstall.File("ccnet.fluentbuild.plugin.dll").Delete(OnError.Continue);
        }
    }
}