using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void ShouldNotFindDatabase()
        {
           Assert.IsFalse(Database.MsSqlDatabase.CreateOrUpgradeDatabase().ConnectionString("server=.\\SQLExpress;Initial Catalog=garbage;Integrated Security=SSPI;").DoesDatabaseAlreadyExist());
        }

        [Test]
        public void ShouldFindDatabase()
        {
            Assert.IsTrue(Database.MsSqlDatabase.CreateOrUpgradeDatabase().ConnectionString("server=.\\SQLExpress;Initial Catalog=master;Integrated Security=SSPI;").DoesDatabaseAlreadyExist());
        }
    }

}