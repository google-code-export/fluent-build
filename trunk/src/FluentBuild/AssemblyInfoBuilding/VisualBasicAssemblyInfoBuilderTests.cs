using System;
using System.Text;
using NUnit.Framework;


namespace FluentBuild.AssemblyInfoBuilding
{
    
	///<summary />	[TestFixture]
    public class VisualBasicAssemblyInfoBuilderTests
    {
        ///<summary />		[Test]
        public void ShouldBuildString()
        {
            var builder = new VisualBasicAssemblyInfoBuilder();
            var details = new AssemblyInfoDetails(builder).ComVisible(false).ClsCompliant(false).Version("1.0.0.0").Title("asmTitle").Description("asmDesc").Copyright("asmCopyright");
           
            
            var sb = new StringBuilder();
            sb.AppendLine("imports System");
            sb.AppendLine("imports System.Reflection");
            sb.AppendLine("imports System.Runtime.InteropServices");

            sb.AppendLine("<assembly: ComVisible(false)>");
            sb.AppendLine("<assembly: CLSCompliant(false)>");
            sb.AppendLine("<assembly: AssemblyVersion(\"1.0.0.0\")>");
            sb.AppendLine("<assembly: AssemblyTitle(\"asmTitle\")>");
            sb.AppendLine("<assembly: AssemblyDescription(\"asmDesc\")>");
            sb.AppendLine("<assembly: AssemblyCopyright(\"asmCopyright\")>");
            //sb.AppendFormat("[assembly: ApplicationNameAttribute(\"{0}\")]{1}", details._applicationName, Environment.NewLine);
            sb.AppendFormat("<assembly: AssemblyCompany(\"{0}\")>{1}", details.AssemblyCompany, Environment.NewLine);
            sb.AppendFormat("<assembly: AssemblyProduct(\"{0}\")>{1}", details.AssemblyProduct, Environment.NewLine);
            Assert.That(builder.Build(details).Trim(), Is.EqualTo(sb.ToString().Trim()));
        }
    }
}