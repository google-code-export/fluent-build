using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class AssemblyInfoLanguageTests
    {
        [Test]
        public void EnsureCSharpLanguageBuildsProperly()
        {
            AssemblyInfoDetails details = new AssemblyInfoLanguage().CSharp;
            Assert.That(details, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.TypeOf(typeof(CSharpAssemblyInfoBuilder)));
        }

        [Test]
        public void EnsureVisualBasicLanguageBuildsProperly()
        {
            AssemblyInfoDetails details = new AssemblyInfoLanguage().VisualBasic;
            Assert.That(details, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.Not.Null);
            Assert.That(details.AssemblyInfoBuilder, Is.TypeOf(typeof(VisualBasicAssemblyInfoBuilder)));
        }
    }
}