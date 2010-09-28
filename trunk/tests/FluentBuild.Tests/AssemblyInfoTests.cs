using System.IO;
using System.Text;
using FluentBuild.Core;
using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class AssemblyInfoTests : TestBase
    {
        [Test]
        public void ShouldGenerateAssemblyInfoFile()
        {
            string outputLocation = rootFolder + "\\assemblyinfo.cs";
            AssemblyInfo
                .Language.CSharp
                .Import("non.existant.namespace")
                .Version("1.0.0.0")
                .OutputTo(outputLocation);

            var expected = new StringBuilder();
            expected.AppendLine("using non.existant.namespace;");
            expected.AppendLine("using System.Reflection;");
            expected.AppendLine("[assembly: AssemblyVersionAttribute(\"1.0.0.0\")]");
            expected.AppendLine("[assembly: AssemblyTitleAttribute(\"\")]");
            expected.AppendLine("[assembly: AssemblyDescriptionAttribute(\"\")]");
            expected.AppendLine("[assembly: AssemblyCopyrightAttribute(\"\")]");
            expected.AppendLine("[assembly: AssemblyCompany(\"\")]");
            expected.AppendLine("[assembly: AssemblyProduct(\"\")]");

            string text = File.ReadAllText(outputLocation);
            Assert.That(text.Trim(), Is.EqualTo(expected.ToString().Trim()));
        }
    }
}