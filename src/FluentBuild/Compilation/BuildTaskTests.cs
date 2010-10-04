using System;
using System.IO;
using FluentBuild.Core;
using NUnit.Framework;


namespace FluentBuild.Compilation
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class BuildTaskTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void Args_ShouldCreateProperArgs()
        {
            string outputAssembly = "myapp.dll";
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}", outputAssembly, "library")));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void Args_ShouldCreateProperArgs_With_Sources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly).AddRefences(reference).AddSources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\"", outputAssembly, "library", reference, source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void Args_ShouldCreateProperReferences()
        {
            var references = new System.Collections.Generic.List<BuildArtifact>();
            references.Add(new BuildArtifact("ref1.dll"));
            references.Add(new BuildArtifact("ref2.dll"));
            
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly).AddRefences(references.ToArray()).AddSources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\" /reference:\"{3}\"  \"{4}\"", outputAssembly, "library", references[0], references[1], source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void Args_ShouldCreateProperArgs_With_Sources_And_Debug_Symbols()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly).AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\" /debug", outputAssembly, "library", reference, source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void OutputFileTo_ShouldWorkWithBuildArtifact()
        {
            string reference = "external.dll";
            var outputAssembly = new BuildArtifact("myapp.dll");
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly).AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\" /debug", outputAssembly, "library", reference, source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void Args_ShouldCreateProperArgs_With_Resources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly).AddResource("Test", "ResName");
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /resource:\"Test\",ResName /target:{1}", outputAssembly, "library", reference, source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void Args_ShouldCreateProperArgs_With_Fileset_Resources()
        {
            string reference = "external.dll";
            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            var sources = new FileSet().Include(source);
            BuildTask build = Build.UsingCsc.Target.Library.OutputFileTo(outputAssembly).AddResources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /resource:\"myfile.cs\" /target:{1}", outputAssembly, "library")));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void UsingCsc_Compiler_Should_Be_CSC()
        {
            BuildTask build = Build.UsingCsc.Target.Library;
            Assert.That(Path.GetFileName(build.Compiler), Is.EqualTo("csc.exe"));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void UsingCsc_Compiler_Should_Be_VBC()
        {
            BuildTask build = Build.UsingVbc.Target.Library;
            Assert.That(Path.GetFileName(build.Compiler), Is.EqualTo("vbc.exe"));
        }

        [Test]
        public void AddResource_ShouldAddSingleFileResource()
        {
            var fileName = "blah.txt";
            var build = Build.UsingCsc.Target.Library.AddResource(fileName);
            var resrouce = build.Resources[0];
            Assert.That(resrouce, Is.Not.Null);
            Assert.That(resrouce.FilePath, Is.EqualTo(fileName));
            Assert.That(resrouce.Identifier, Is.EqualTo(null));
        }
    }
}