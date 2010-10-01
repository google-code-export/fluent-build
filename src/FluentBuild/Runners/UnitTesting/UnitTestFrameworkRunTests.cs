using NUnit.Framework;

namespace FluentBuild.Runners.UnitTesting
{
    ///<summary />	[TestFixture]
    public class UnitTestFrameworkRunTests
    {
        ///<summary />	[Test]
        public void Nunit_ShouldCreateNunitRunner()
        {
            var subject = new UnitTestFrameworkRun();
            Assert.That(subject.NUnit, Is.TypeOf(typeof (NUnitRunner)));
        }
    }
}