using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild.Compilation
{
    [TestFixture]
    public class BuildTaskTests
    {
        [Test]
        public void Args_ShouldCreateProperArgs()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:{0} /target:{1}", outputAssembly, "library")));
        }

        [Test]
        public void Args_ShouldCreateProperArgs_With_Sources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddRefences(reference).AddSources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:{0} /target:{1}  /reference:{2}  {3}", outputAssembly, "library", reference, source)));
        }

        [Test]
        public void Args_ShouldCreateProperArgs_With_Sources_And_Debug_Symbols()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:{0} /target:{1}  /reference:{2}  {3} /debug", outputAssembly, "library", reference, source)));
        }

        [Test]
        public void UsingCsc_Compiler_Should_Be_CSC()
        {
            BuildTask build = Build.UsingCsc;
            Assert.That(Path.GetFileName(build.compiler), Is.EqualTo("csc.exe"));
        }

        [Test]
        public void UsingCsc_Compiler_Should_Be_VBC()
        {
            BuildTask build = Build.UsingVbc;
            Assert.That(Path.GetFileName(build.compiler), Is.EqualTo("vbc.exe"));
        }
    }
}