using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild.Runners
{
    [TestFixture]
    public class UnitTestFrameworkRunTests
    {
        [Test]
        public void Nunit_ShouldCreateNunitRunner()
        {
            var subject = new UnitTestFrameworkRun();
            Assert.That(subject.NUnit, Is.TypeOf(typeof (NUnitRunner)));
        }
    }
}