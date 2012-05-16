using NUnit.Framework;


namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class AssemblyInfoLanguageTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void EnsureCSharpLanguageBuildsProperly()
        {
            AssemblyInfoDetails details = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder()); 
            Assert.That(details, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.TypeOf(typeof(CSharpAssemblyInfoBuilder)));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void EnsureVisualBasicLanguageBuildsProperly()
        {
            AssemblyInfoDetails details = new AssemblyInfoDetails(new VisualBasicAssemblyInfoBuilder()); 
            Assert.That(details, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.TypeOf(typeof(VisualBasicAssemblyInfoBuilder)));
        }
    }
}