using NUnit.Framework;

using Rhino.Mocks;

namespace FluentBuild.Database
{
    [TestFixture]
    public class MsSqlCreateTests
    {
        [Test]
        public void ShouldSetPathOnEngine()
        {
            string path = @"c:\temp";

            var engine = MockRepository.GenerateMock<IMsSqlEngine>();
            var subject = new MsSqlCreate(engine);
            
            var pathToUpdateScripts = subject.PathToUpdateScripts(path);
            
            Assert.That(pathToUpdateScripts, Is.TypeOf(typeof (MsSqlUpdate)));
            
            engine.AssertWasCalled(x=>x.PathToUpdateScripts=path);
        }
    }

    public class MsSqlCreate
    {
        private readonly IMsSqlEngine _engine;

        public MsSqlCreate(IMsSqlEngine engine)
        {
            _engine = engine;
        }

        public MsSqlUpdate PathToUpdateScripts(string path)
        {
            _engine.PathToUpdateScripts = path;
            return new MsSqlUpdate(_engine);
        }
    }
}