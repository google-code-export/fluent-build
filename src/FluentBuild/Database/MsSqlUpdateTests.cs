using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild.Database
{
    [TestFixture]
    public class MsSqlUpdateTests
    {
        [Test]
        public void VersionTable_ShouldSetVersionOnEngine()
        {
            var engine = MockRepository.GenerateMock<IMsSqlEngine>();
            var subject = new MsSqlUpdate(engine);
            var msSqlVersionTable = subject.VersionTable("blah");
            Assert.That(msSqlVersionTable, Is.TypeOf(typeof(MsSqlVersionTable)));
            engine.AssertWasCalled(x=>x.VersionTable="blah");
        }
    }
}