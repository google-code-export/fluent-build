using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using FluentBuild.Compilation;

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
            Task.CreateAssemblyInfo.Language.CSharp(x=>x.Import("non.existant.namespace")
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
            Task.CreateAssemblyInfo.Language.CSharp(x=>x.Company("company")
                .Copyright("copyright")
                .Description("description")
                .Product("product")
                .Title("title")
                .Version("1.0.0.0")
                .Culture("en-US")
                .DelaySign(true)
                .FileVersion("1.0.0.0")
                .InformationalVersion("1.0.0.0")
                //.KeyFile("c:\\temp\\nonexistant.snk")
                //.KeyName("name of key")
                .ComVisible(true)
                .ClsCompliant(true)
                .Trademark("trademark")
                .OutputPath(outputLocation));

            var fs = new FileSet();
            fs.Include(outputLocation);
            var outputFileLocation = rootFolder + "\\asminfoCSharp.dll";
            Task.Build.Csc.Target.Library(x => x.AddSources(fs).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));

            VerifyDllWithReflection(outputFileLocation);
        }

        private void VerifyDllWithReflection(string outputFileLocation)
        {
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(outputFileLocation));

            Assert.That(GetAttribute<AssemblyCopyrightAttribute>(assembly).Copyright, Is.EqualTo("copyright"));
            Assert.That(GetAttribute<AssemblyDescriptionAttribute>(assembly).Description, Is.EqualTo("description"));
            Assert.That(GetAttribute<AssemblyProductAttribute>(assembly).Product, Is.EqualTo("product"));
            Assert.That(GetAttribute<AssemblyTitleAttribute>(assembly).Title, Is.EqualTo("title"));
            Assert.That(assembly.GetName().Version.ToString(), Is.EqualTo("1.0.0.0"));
            Assert.That(assembly.GetName().CultureInfo.Name, Is.EqualTo("en-US"));
            //Assert.That(GetAttribute<AssemblyCultureAttribute>(assembly).Culture, Is.EqualTo("en-us"));
            Assert.That(GetAttribute<AssemblyDelaySignAttribute>(assembly).DelaySign, Is.EqualTo(true));
            Assert.That(GetAttribute<AssemblyFileVersionAttribute>(assembly).Version, Is.EqualTo("1.0.0.0"));
            Assert.That(GetAttribute<AssemblyInformationalVersionAttribute>(assembly).InformationalVersion, Is.EqualTo("1.0.0.0"));
            Assert.That(GetAttribute<ComVisibleAttribute>(assembly).Value, Is.EqualTo(true));
            Assert.That(GetAttribute<CLSCompliantAttribute>(assembly).IsCompliant, Is.EqualTo(true));
            Assert.That(GetAttribute<AssemblyTrademarkAttribute>(assembly).Trademark, Is.EqualTo("trademark"));

            
        }

        private T GetAttribute<T>(Assembly assembly)
        {
            return (T) assembly.GetCustomAttributes(typeof (T), false)[0];
        }

        [Test]
        public void ShouldCompileAssemblyInfoToVisualBasic()
        {
            string outputLocation = rootFolder + "\\assemblyinfo.cs";
            Task.CreateAssemblyInfo.Language.VisualBasic(x=>x.Company("company")
                .Copyright("copyright")
                .Description("description")
                .Product("product")
                .Title("title")
                .Version("1.0.0.0")
                .Culture("en-US")
                .DelaySign(true)
                .FileVersion("1.0.0.0")
                .InformationalVersion("1.0.0.0")
                //.KeyFile("c:\\temp\\nonexistant.snk")
                //.KeyName("name of key")
                .ComVisible(true)
                .ClsCompliant(true)
                .Trademark("trademark")
                .OutputPath(outputLocation));

            var fs = new FileSet();
            fs.Include(outputLocation);
            var outputFileLocation = rootFolder + "\\asminfoVisualBasic.dll";
            Task.Build.Vbc.Target.Library(x=>x.AddSources(fs).OutputFileTo(outputFileLocation));
            Assert.That(File.Exists(outputFileLocation));

            VerifyDllWithReflection(outputFileLocation);

        }
    }
}