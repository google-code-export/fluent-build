using System.Text;
using FluentBuild.Compilation;
using FluentBuild.Core;
using FluentFs.Core;
using NUnit.Framework;
using File = System.IO.File;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class AssemblyInfoTests : TestBase
    {
        [Test]
        public void ShouldGenerateAssemblyInfoFile()
        {
            string outputLocation = rootFolder + "\\assemblyinfo.cs";
            Task.CreateAssemblyInfo(x=>x.Language.CSharp
                .Import("non.existant.namespace")
                .Version("1.0.0.0")
                .OutputPath(outputLocation));

            var expected = new StringBuilder();
            expected.AppendLine("using non.existant.namespace;");
            expected.AppendLine("using System.Reflection;");
            expected.AppendLine("[assembly: AssemblyVersionAttribute(\"1.0.0.0\")]");
//            expected.AppendLine("[assembly: AssemblyTitleAttribute(\"\")]");
//            expected.AppendLine("[assembly: AssemblyDescriptionAttribute(\"\")]");
//            expected.AppendLine("[assembly: AssemblyCopyrightAttribute(\"\")]");
//            expected.AppendLine("[assembly: AssemblyCompany(\"\")]");
//            expected.AppendLine("[assembly: AssemblyProduct(\"\")]");
//
            string text = File.ReadAllText(outputLocation);
            Assert.That(text.Trim(), Is.EqualTo(expected.ToString().Trim()));
        }

        [Test]
        public void ShouldCompileAssemblyInfoToCSharp()
        {
            string outputLocation = rootFolder + "\\assemblyinfo.cs";
            Task.CreateAssemblyInfo(x=>x.Language.CSharp.Company("company")
                .Copyright("copyright")
                .Description("description")
                .Product("product")
                .Title("title")
                .Version("1.0.0.0")
                .Culture("culture")
                .DelaySign(true)
                .FileVersion("1.0.0.0")
                .InformationalVersion("1.0.0.0")
                //.KeyFile("c:\\temp\\nonexistant.snk")
                //.KeyName("name of key")
                .Trademark("trademark")
                .OutputPath(outputLocation));

            var fs = new FileSet();
            fs.Include(outputLocation);
            var outputFileLocation = rootFolder + "\\asminfo.dll";
            Task.Build(Using.Csc.Target.Library.AddSources(fs).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));

        }

        [Test]
        public void ShouldCompileAssemblyInfoToVisualBasic()
        {
            string outputLocation = rootFolder + "\\assemblyinfo.cs";
            Task.CreateAssemblyInfo(x=>x.Language.VisualBasic.Company("company")
                .Copyright("copyright")
                .Description("description")
                .Product("product")
                .Title("title")
                .Version("1.0.0.0")
                .Culture("culture")
                .DelaySign(true)
                .FileVersion("1.0.0.0")
                .InformationalVersion("1.0.0.0")
                //.KeyFile("c:\\temp\\nonexistant.snk")
                //.KeyName("name of key")
                .Trademark("trademark")
                .OutputPath(outputLocation));

            var fs = new FileSet();
            fs.Include(outputLocation);
            var outputFileLocation = rootFolder + "\\asminfo.dll";
            Task.Build(Using.Vbc.Target.Library.AddSources(fs).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));

        }
    }
}