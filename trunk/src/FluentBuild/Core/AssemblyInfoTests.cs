using FluentBuild.AssemblyInfoBuilding;
using NUnit.Framework;

namespace FluentBuild.Core
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