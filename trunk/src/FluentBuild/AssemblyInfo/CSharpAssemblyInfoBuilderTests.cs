using System;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;


namespace FluentBuild
{
    [TestFixture]
    public class CSharpAssemblyInfoBuilderTests
    {
        [Test]
        public void ShouldBuildString()
        {
            var builder = new CSharpAssemblyInfoBuilder();
            var details = new AssemblyInfoDetails(builder).ComVisible(false).ClsCompliant(false).AssemblyVersion("1.0.0.0").AssemblyTitle("asmTitle").AssemblyDescription("asmDesc").AssemblyCopyright("asmCopyright");
           
            
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine("using System.Runtime.InteropServices;");

            sb.AppendLine("[assembly: ComVisible(False)]");
            sb.AppendLine("[assembly: ClsCompliant(False)]");
            sb.AppendLine("[assembly: AssemblyVersionAttribute(\"1.0.0.0\")]");
            sb.AppendLine("[assembly: AssemblyTitleAttribute(\"asmTitle\")]");
            sb.AppendLine("[assembly: AssemblyDescriptionAttribute(\"asmDesc\")]");
            sb.AppendLine("[assembly: AssemblyCopyrightAttribute(\"asmCopyright\")]");
            //sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", details._applicationName, Environment.NewLine);
            //sb.AppendFormat("[assembly: ApplicationCompanyAttribute(\"{0}\")]{1}", details._company, Environment.NewLine);
            //sb.AppendFormat("[assembly: ApplicationProductNameAttribute(\"{0}\")]{1}", details._productName, Environment.NewLine);
            Assert.That(builder.Build(details).Trim(), Is.EqualTo(sb.ToString().Trim()));
        }
    }
}