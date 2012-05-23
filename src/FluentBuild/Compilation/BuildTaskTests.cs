using System;
using System.Collections.Generic;
using System.IO;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using FluentFs.Core;
using NUnit.Framework;
using Rhino.Mocks;
using File = FluentFs.Core.File;

namespace FluentBuild.Compilation
{
    ///<summary>
    ///</summary>
    ///<summary />
    [TestFixture]
    public class BuildTaskTests
    {
        [Test]
        public void AddResource_ShouldAddSingleFileResource()
        {
            var fileName = "blah.txt";
            var build = new BuildTask("csc.exe", "library").AddResource(fileName);
            var resrouce = build.Resources[0];
            Assert.That(resrouce, Is.Not.Null);
            Assert.That(resrouce.FilePath, Is.EqualTo(fileName));
            Assert.That(resrouce.Identifier, Is.EqualTo(null));
            
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void Args_ShouldCreateProperArgs()
        {
            string outputAssembly = "myapp.dll";
            BuildTask build = new BuildTask("csc.exe", "library").OutputFileTo(outputAssembly);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}", outputAssembly, "library")));
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
            FileSet sources = new FileSet().Include(source);
            BuildTask build = new BuildTask("", "library").OutputFileTo(outputAssembly).AddResources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /resource:\"myfile.cs\" /target:{1}", outputAssembly, "library")));
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
            BuildTask build = new BuildTask("", "library").OutputFileTo(outputAssembly).AddResource("Test", "ResName");
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /resource:\"Test\",ResName /target:{1}", outputAssembly, "library", reference, source)));
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
            FileSet sources = new FileSet().Include(source);
             BuildTask build =  new BuildTask("","library").OutputFileTo(outputAssembly).AddRefences(reference).AddSources(sources);
             Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\"", outputAssembly, "library", reference, source)));
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
            FileSet sources = new FileSet().Include(source);
            BuildTask build = new BuildTask("", "library").OutputFileTo(outputAssembly).AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\" /debug", outputAssembly, "library", reference, source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void Args_ShouldCreateProperReferences()
        {
            var references = new List<File>();
            references.Add(new File("ref1.dll"));
            references.Add(new File("ref2.dll"));

            string outputAssembly = "myapp.dll";
            string source = "myfile.cs";
            FileSet sources = new FileSet().Include(source);
            BuildTask build = new BuildTask("", "library").OutputFileTo(outputAssembly).AddRefences(references.ToArray()).AddSources(sources);
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\" /reference:\"{3}\"  \"{4}\"", outputAssembly, "library", references[0], references[1], source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void OutputFileTo_ShouldWorkWithBuildArtifact()
        {
            string reference = "external.dll";
            var outputAssembly = new File("myapp.dll");
            string source = "myfile.cs";
            FileSet sources = new FileSet().Include(source);
            BuildTask build = new BuildTask("", "library").OutputFileTo(outputAssembly).AddRefences(reference).AddSources(sources).IncludeDebugSymbols;
            Assert.That(build.Args.Trim(), Is.EqualTo(String.Format("/out:\"{0}\"  /target:{1}  /reference:\"{2}\"  \"{3}\" /debug", outputAssembly, "library", reference, source)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void UsingCsc_Compiler_Should_Be_CSC()
        {
            BuildTask build = new BuildTask("csc.exe", "library");
            Assert.That(Path.GetFileName(build.Compiler), Is.EqualTo("csc.exe"));
        }

        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void UsingCsc_Compiler_Should_Be_VBC()
        {
            BuildTask build =  new BuildTask("vbc.exe","library");
            Assert.That(Path.GetFileName(build.Compiler), Is.EqualTo("vbc.exe"));
        }

        [Test]
        public void ShouldExecute()
        {
            var mock = MockRepository.GenerateStub<IActionExcecutor>();
            BuildTask build = new BuildTask(mock,"vbc.exe", "library");
            build.InternalExecute();
            mock.AssertWasCalled(x=>x.Execute(Arg<Action<Executable>>.Is.Anything));
        }

    }

}