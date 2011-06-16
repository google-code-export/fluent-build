using NUnit.Framework;

namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class AssemblyInfoDetailsTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void ImportShouldNotAllowDuplicates()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            Assert.That(subject.Imports.Count, Is.EqualTo(0));
            subject.Import("test");
            subject.Import("test");
            Assert.That(subject.Imports.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldHandleAllDefaultAttributes()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            subject.Company("").Copyright("").Description("").Product("").Title("")
                .Version("1.0.0.0").Culture("").DelaySign(true).FileVersion("1.0.0.0")
                .InformationalVersion("1.0.0.0").KeyFile("").KeyName("").Trademark("");
//    InternalsVisibleTo

        }
    }
}