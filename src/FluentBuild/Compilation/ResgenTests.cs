using FluentBuild.Core;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    ///<summary />	[TestFixture]
    public class ResgenTests
    {
        #region Setup/Teardown

        ///<summary />	    [SetUp]
        public void SetUp()
        {
            var frameworkVersion = MockRepository.GenerateStub<IFrameworkVersion>();
            frameworkVersion.Stub(x => x.GetPathToSdk()).Return("c:\\temp").Repeat.Any();
            Defaults.FrameworkVersion = frameworkVersion;
        }

        #endregion

        ///<summary />	    [Test]
        public void Execute_ShouldRunAgainstMock()
        {
            var fileset = new FileSet();
            fileset.Include(@"c:\temp\nonexistant.txt");

            var mockExe = MockRepository.GenerateStub<IExecuteable>();

            Resgen subject = new Resgen(mockExe).GenerateFrom(fileset).OutputTo("c:\\");
            mockExe.Stub(x => x.Executable("c:\\temp\\bin\\resgen.exe")).Return(mockExe);
            mockExe.Stub(x => x.WithArguments(Arg<string[]>.Is.Anything)).Return(mockExe);
            mockExe.Stub(x => x.WithArguments(Arg<string[]>.Is.Anything)).Return(mockExe);

            subject.Execute();
            mockExe.AssertWasCalled(x => x.Execute());
        }

        ///<summary />	[Test]
        public void GenerateFrom_ShouldPopulateFiles()
        {
            var fileset = new FileSet();
            fileset.Include("c:\temp\nonexistant.txt");

            Resgen subject = new Resgen().GenerateFrom(fileset);

            Assert.That(subject.Files, Is.Not.Null);
            Assert.That(subject, Is.Not.Null);
        }

        ///<summary />	[Test]
        public void OutputTo_ShouldPopulatePathAndNotBeNull()
        {
            string folder = "c:\temp";
            Resgen subject = new Resgen().OutputTo(folder);

            Assert.That(subject.OutputFolder, Is.EqualTo(folder));
            Assert.That(subject, Is.Not.Null);
        }


        ///<summary />	[Test]
        public void PrefixOutputsWith_ShouldSetPrefixProperly()
        {
            string prefix = "blah";
            Resgen subject = new Resgen().PrefixOutputsWith(prefix);

            Assert.That(subject.Prefix, Is.EqualTo(prefix));
            Assert.That(subject, Is.Not.Null);
        }
    }
}