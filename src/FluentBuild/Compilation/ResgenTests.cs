using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace FluentBuild.Compilation.ResGen
{
    [TestFixture]
    public class ResgenTests
    {
        [Test]
        public void Execute_ShouldRunAgainstMock()
        {
            var fileset = new FileSet();
            fileset.Include(@"c:\temp\nonexistant.txt");

            var mockSdkFinder = MockRepository.GenerateStub<IWindowsSdkFinder>();
            var mockExe = MockRepository.GenerateStub<IExecuteable>();

            var subject = new Resgen(mockSdkFinder,mockExe).GenerateFrom(fileset).OutputTo("c:\\");
            mockSdkFinder.Stub(x => x.IsWindowsSdkInstalled()).Return(true);
            mockSdkFinder.Stub(x => x.PathToHighestVersionedSdk()).Return("c:\\temp");
            mockExe.Stub(x => x.Executable("c:\\temp\\bin\\resgen.exe")).Return(mockExe);
            mockExe.Stub(x => x.WithArguments(Arg<string[]>.Is.Anything)).Return(mockExe);
            mockExe.Stub(x => x.WithArguments(Arg<string[]>.Is.Anything)).Return(mockExe);

            subject.Execute();
            mockExe.AssertWasCalled(x => x.Execute());
            
        }

        
        [Test]
        public void GenerateFrom_ShouldPopulateFiles()
        {
            var fileset = new FileSet();
            fileset.Include("c:\temp\nonexistant.txt");

            var subject = new Resgen().GenerateFrom(fileset);

            Assert.That(subject._files, Is.Not.Null);
            Assert.That(subject, Is.Not.Null);
        }

        [Test]
        public void OutputTo_ShouldPopulatePathAndNotBeNull()
        {
            string folder = "c:\temp";
            var subject = new Resgen().OutputTo(folder);

            Assert.That(subject._outputFolder, Is.EqualTo(folder));
            Assert.That(subject, Is.Not.Null);
        }


        [Test]
        public void PrefixOutputsWith_ShouldSetPrefixProperly()
        {
            string prefix = "blah";
            var subject = new Resgen().PrefixOutputsWith(prefix);

            Assert.That(subject._prefix, Is.EqualTo(prefix));
            Assert.That(subject, Is.Not.Null);
        }
    }
}