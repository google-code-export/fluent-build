using FluentBuild.AssemblyInfoBuilding;
using NUnit.Framework;

namespace FluentBuild.Core
{
    ///<summary />	[TestFixture]
    public class AssemblyInfoTests
    {
        ///<summary />	[Test]
        public void MethodCallShouldNotThrowException()
        {
            AssemblyInfoLanguage assemblyInfoLanguage = AssemblyInfo.Language;
            Assert.That(assemblyInfoLanguage, Is.Not.Null);
        }
    }

}