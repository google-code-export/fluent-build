using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class CscParserTests
    {
        [Test, Ignore("method is not implemented yet")]
        public void TestSomething()
        {
            var input = new StringBuilder();
            input.AppendLine("<csc output=\"${assembly.output}\" target=\"library\" debug=\"${debug}\">");
            input.AppendLine("  <sources>");
            input.AppendLine("      <include name=\"${dir.commons}/**/*.cs\"/>");
            input.AppendLine("      <exclude name=\"${dir.commons}/**/AssemblyInfo.cs\"/>");
            input.AppendLine("      <include name=\"${dir.compile}/CommonAssemblyInfo.cs\"/>");
            input.AppendLine("  </sources>");
            input.AppendLine("  <references>");
            input.AppendLine("      <include name=\"${thirdparty.windsor}\"/>");
            input.AppendLine("      <include name=\"${thirdparty.castlecore}\"/>");
            input.AppendLine("  </references>");
            input.AppendLine("</csc>");

            var subject = new CscParser();
            subject.Parse(XElement.Parse(input.ToString()));
        }
    }
}