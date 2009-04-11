using System;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;


namespace FluentBuild
{
    [TestFixture]
    public class VisualBasicAssemblyInfoBuilderTests
    {
        [Test]
        public void ShouldBuildString()
        {
            var builder = new VisualBasicAssemblyInfoBuilder();
            var details = new AssemblyInfoDetails(builder).ComVisible(false).ClsCompliant(false).AssemblyVersion("1.0.0.0").AssemblyTitle("asmTitle").AssemblyDescription("asmDesc").AssemblyCopyright("asmCopyright");
           
            
            var sb = new StringBuilder();
            sb.AppendLine("imports System");
            sb.AppendLine("imports System.Reflection");
            sb.AppendLine("imports System.Runtime.InteropServices");

            sb.AppendLine("<assembly: ComVisible(False)>");
            sb.AppendLine("<assembly: ClsCompliant(False)>");
            sb.AppendLine("<assembly: AssemblyVersion(\"1.0.0.0\")>");
            sb.AppendLine("<assembly: AssemblyTitle(\"asmTitle\")>");
            sb.AppendLine("<assembly: AssemblyDescription(\"asmDesc\")>");
            sb.AppendLine("<assembly: AssemblyCopyright(\"asmCopyright\")>");
            //sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", details._applicationName, Environment.NewLine);
            //sb.AppendFormat("[assembly: ApplicationCompanyAttribute(\"{0}\")]{1}", details._company, Environment.NewLine);
            //sb.AppendFormat("[assembly: ApplicationProductNameAttribute(\"{0}\")]{1}", details._productName, Environment.NewLine);
            Assert.That(builder.Build(details).Trim(), Is.EqualTo(sb.ToString().Trim()));
        }
    }
}