using System;
using System.IO;
using FluentBuild.Core;
using NUnit.Framework;


namespace FluentBuild.Compilation
{
    [TestFixture]
    public class BuildTaskTests
    {
        [Test]
        public void Args_ShouldCreateProperArgs()
        {
            string outputAssembly = "myapp.dll";
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}", outputAssembly, "library")));
        }

        [Test]
        public void Args_ShouldCreateProperArgs_With_Sources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddRefences(reference).AddSources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\"", outputAssembly, "library", reference, source)));
        }


        [Test]
        public void Args_ShouldCreateProperReferences()
        {
            var references = new System.Collections.Generic.List<BuildArtifact>();
            references.Add(new BuildArtifact("ref1.dll"));
            references.Add(new BuildArtifact("ref2.dll"));
            
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddRefences(references.ToArray()).AddSources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\" /reference:\"{3}\"  \"{4}\"", outputAssembly, "library", references[0], references[1], source)));
        }

        [Test]
        public void Args_ShouldCreateProperArgs_With_Sources_And_Debug_Symbols()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\" /debug", outputAssembly, "library", reference, source)));
        }

        [Test]
        public void OutputFileTo_ShouldWorkWithBuildArtifact()
        {
            string reference = "external.dll";
            var outputAssembly = new BuildArtifact("myapp.dll");
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\" /debug", outputAssembly, "library", reference, source)));
        }

        [Test]
        public void Args_ShouldCreateProperArgs_With_Resources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddResource("Test", "ResName");
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /resource:\"Test\",ResName /target:{1}", outputAssembly, "library", reference, source)));
        }

        [Test]
        public void Args_ShouldCreateProperArgs_With_Fileset_Resources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.OutputFileTo(outputAssembly).Target.Library.AddResources(sources);
            //TODO: what is up with the comma? does it affect multiple resources being included?
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /resource:\"myfile.cs\", /target:{1}", outputAssembly, "library")));
        }

        [Test]
        public void UsingCsc_Compiler_Should_Be_CSC()
        {
            BuildTask build = Build.UsingCsc;
            Assert.That(Path.GetFileName(build.Compiler), Is.EqualTo("csc.exe"));
        }

        [Test]
        public void UsingCsc_Compiler_Should_Be_VBC()
        {
            BuildTask build = Build.UsingVbc;
            Assert.That(Path.GetFileName(build.Compiler), Is.EqualTo("vbc.exe"));
        }
    }
}