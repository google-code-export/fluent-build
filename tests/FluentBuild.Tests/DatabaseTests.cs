using NUnit.Framework;

namespace FluentBuild.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        [Ignore]
        public void ShouldNotFindDatabase()
        {
           //Assert.IsFalse(Database.Database.MsSqlDatabase.CreateOrUpgradeDatabase().ConnectionString("server=.\\SQLExpress;Initial Catalog=garbage;Integrated Security=SSPI;").DoesDatabaseAlreadyExist());
        }

        [Ignore]
        public void ShouldFindDatabase()
        {
            //Assert.IsTrue(Database.Database.MsSqlDatabase.CreateOrUpgradeDatabase().ConnectionString("server=.\\SQLExpress;Initial Catalog=master;Integrated Security=SSPI;").DoesDatabaseAlreadyExist());
        }
    }

}