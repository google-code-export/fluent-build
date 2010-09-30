using NUnit.Framework;


namespace FluentBuild.Database
{
    [TestFixture]
    public class MsSqlConnectionTests
    {
        [Test]
        public void ShouldCreateMsSqlUtilities()
        {
            var subject = new MsSqlConnection();
            Assert.That(subject.WithConnectionString(""), Is.TypeOf(typeof(MsSqlUtilities)));
        }

        [Test]
        public void ShouldCreateMsSqlUtilitiesAndHaveEngineBuilt()
        {
            var subject = new MsSqlConnection();
            MsSqlUtilities withConnectionString = subject.WithConnectionString("Server=.;Initial Catalog=test;Integrated Security=SSPI;");

            Assert.That(withConnectionString._engine, Is.Not.Null);
            Assert.That(withConnectionString._engine.ConnectionString, Is.Not.Null);
            Assert.That(withConnectionString._engine.ConnectionString.InitialCatalog, Is.EqualTo("test"));
        }
    }
}