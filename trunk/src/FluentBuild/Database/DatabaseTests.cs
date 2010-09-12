using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild.Database
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void ShouldCreateProperObject()
        {
            Assert.That(Database.MsSqlDatabase, Is.TypeOf(typeof(MsSqlConnection)));
        }
    }
}