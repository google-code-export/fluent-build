using FluentBuild.AssemblyInfoBuilding;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class AssemblyInfoTests
    {
        [Test]
        public void MethodCallShouldNotThrowException()
        {
            AssemblyInfoLanguage assemblyInfoLanguage = AssemblyInfo.Language;
            Assert.That(assemblyInfoLanguage, Is.Not.Null);
        }
    }

}