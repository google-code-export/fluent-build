using NUnit.Framework;

namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    ///</summary>
    ///<summary />	[TestFixture]
    public class AssemblyInfoDetailsTests
    {
        ///<summary>
        ///</summary>
        ///<summary />	[Test]
        public void ImportShouldNotAllowDuplicates()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            Assert.That(subject.Imports.Count, Is.EqualTo(0));
            subject.Import("test");
            subject.Import("test");
            Assert.That(subject.Imports.Count, Is.EqualTo(1));
        }
    }
}