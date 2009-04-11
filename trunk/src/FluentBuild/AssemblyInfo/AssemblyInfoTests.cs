using NUnit.Framework;

namespace FluentBuild
{
    [TestFixture]
    public class AssemblyInfoTests
    {
        [Test]
        public void Build_ShouldCreateAssemblyInfoFile()
        {
            AssemblyInfo.Language.CSharp.Import("System", "System.Reflection").ComVisible(false).ClsCompliant(true).AssemblyVersion("1.0.0.0").AssemblyTitle("Test Title").AssemblyDescription("Description").AssemblyCopyright("(c) 2009").OutputTo("AssemblyInfo.cs");
        }
    }
}