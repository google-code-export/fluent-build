using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class AssemblyInfoTests
    {
        [Test]
        public void Build_ShouldCreateAssemblyInfoFile()
        {
            //Language.CSharp
            new AssemblyInfo().Import("System", "System.Reflection").ComVisible(false).ClsCompliant(true).AssemblyVersion("1.0.0.0").AssemblyTitle("Test Title").AssemblyDescription("Description").AssemblyCopyright("(c) 2009").ApplicationName("MyAppName").OutputTo("AssemblyInfo.cs");
        }
    }
}