using System.IO;

namespace FluentBuild.Sample
{
    internal class MainBuild
    {
        private const string name_Commons = "IglooCoder.Commons.dll";
        private const string name_Commons_Tests = "IglooCoder.Commons.Tests.dll";
        private string dir_base;
        private string dir_commons;
        private string dir_commons_tests;
        private string dir_compile;
        private string dir_lib;
        private string dir_release;
        private string dir_source;
        private string dir_test;
        private string dir_tools;
        private string thirdparty_castlecore;
        private string thirdparty_dynamicproxy;
        private string thirdparty_log4net;
        private string thirdparty_microkernel;
        private string thirdparty_nhibernate;
        private string thirdparty_nunit;
        private string thirdparty_rhino;
        private string thirdparty_windsor;
        private string tools_nunit_console;
        private const string application_version = "0.0.0.0";

        public void Execute()
        {
            SetupProperties();
            SetupDirs();
            SetupRequiredBuildFiles();
            BuildAssemblyInfo();
            Compile();
            Compile_Commons_Tests();
            RunTests();
        }

        private void SetupProperties()
        {
            dir_base = Directory.GetCurrentDirectory();
            //external from ccNet
            //pulled from command line in nAnt
            //http://nant.sourceforge.net/release/latest/help/fundamentals/running-nant.html
            //<if test="${property::exists('CCNetWorkingDirectory')}"><property name="dir.base" value="${CCNetWorkingDirectory}"/>
            //<if test="${property::exists('CCNetLabel')}"><property name="application.version" value="${CCNetLabel}"/>
            //<if test="${property::exists('CCNetArtifactDirectory')}"><property name="dir.results.unittests" value="${CCNetArtifactDirectory}\testresults"/>
            dir_source = dir_base.SubFolder("src");
            dir_test = dir_base.SubFolder("test");
            dir_compile = dir_base.SubFolder("compile");
            dir_release = dir_base.SubFolder("release");
            dir_lib = dir_base.SubFolder("lib");
            dir_tools = dir_base.SubFolder("tools");
            dir_commons = dir_source.SubFolder("igloocoder.commons");
            dir_commons_tests = dir_test.SubFolder("igloocoder.commons.tests");
            thirdparty_windsor = dir_compile.FileName("Castle.Windsor.dll");
            thirdparty_castlecore = dir_compile.FileName("Castle.Core.dll");
            thirdparty_microkernel = dir_compile.FileName("Castle.MicroKernel.dll");
            thirdparty_dynamicproxy = dir_compile.FileName("Castle.DynamicProxy2.dll");
            thirdparty_nhibernate = dir_compile.FileName("nhibernate.dll");
            thirdparty_nunit = dir_compile.FileName("nunit.framework.dll");
            thirdparty_rhino = dir_compile.FileName("rhino.mocks.dll");
            thirdparty_log4net = dir_compile.FileName("log4net.dll");
            tools_nunit_console = dir_tools.SubFolder("nunit").FileName("nunit-console.exe");
        }

        private void SetupDirs()
        {
            DirectoryUtility.RecreateDirectory(dir_compile);
            DirectoryUtility.RecreateDirectory(dir_release);
        }

        private void RunTests()
        {
            ProcessUtility.StartProcess(tools_nunit_console, dir_compile.FileName(name_Commons_Tests), dir_base);
        }

        private void Compile_Commons_Tests()
        {
            FileSet sources = new FileSet().Include(dir_commons_tests.AllSubFolders().FileName("*.cs")).Include(dir_compile.FileName("CommonAssemblyInfo.cs")).Exclude(dir_commons.AllSubFolders().FileName("AssemblyInfo.cs"));
            Build build = CreateBuildTask.UsingCsc.OutputFileTo(dir_compile.FileName(name_Commons_Tests)).IncludeDebugSymbols.Target.Library.AddSources(sources).AddRefences(thirdparty_windsor, thirdparty_castlecore, thirdparty_microkernel, thirdparty_dynamicproxy, thirdparty_nunit, thirdparty_rhino, thirdparty_nhibernate, thirdparty_log4net, dir_compile + "\\" + name_Commons);
            build.Execute();
        }

        private void Compile()
        {
            FileSet sources = new FileSet().Include(dir_commons.AllSubFolders().FileName("*.cs")).Include(dir_compile.FileName("CommonAssemblyInfo.cs")).Exclude(dir_commons.AllSubFolders().FileName("AssemblyInfo.cs"));
            Build build = CreateBuildTask.UsingCsc.OutputFileTo(dir_compile.FileName(name_Commons)).Target.Library.IncludeDebugSymbols.AddSources(sources).AddRefences(thirdparty_windsor, thirdparty_castlecore, thirdparty_microkernel, thirdparty_nhibernate, thirdparty_log4net);
            build.Execute();
        }

        private void BuildAssemblyInfo()
        {
            new AssemblyInfo().Import("System", "System.Reflection").AssemblyVersion(application_version).AssemblyCopyright("Copyright (c) 2008 igloocoder.com Consulting Inc.").AssemblyCompany("company").AssemblyProduct("product").OutputTo(dir_compile.FileName("CommonAssemblyInfo.cs"));
        }

        private void SetupRequiredBuildFiles()
        {
            DirectoryUtility.CopyAllFiles(dir_lib, dir_compile);
            FileSet fileset = new FileSet().Include(dir_tools.AllSubFolders().FileName("nunit.framework.dll")).Include(dir_tools.AllSubFolders().FileName("rhino.mocks.dll"));
            DirectoryUtility.CopyAllFiles(fileset, dir_compile);
        }
    }
}