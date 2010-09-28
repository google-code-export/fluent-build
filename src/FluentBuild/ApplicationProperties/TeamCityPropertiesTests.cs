using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.ApplicationProperties
{
    [TestFixture]
    public class TeamCityPropertiesTests
    {
        private IEnvironmentVariableWrapper _environmentVariableWrapper;
        private TeamCityProperties _subject;

        [SetUp]
        public void Setup()
        {
            _environmentVariableWrapper = MockRepository.GenerateStub<IEnvironmentVariableWrapper>();
            _subject = new TeamCityProperties(_environmentVariableWrapper);
        }

        [Test]
        public void BuildNumberShouldCallToWrapper()
        {
            var buildNumber = _subject.BuildNumber;
            _environmentVariableWrapper.AssertWasCalled(x=>x.Get(Arg<string>.Is.Anything));
        }

        [Test]
        public void ConfigurationNameShouldCallToWrapper()
        {
            var data = _subject.ConfigurationName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        [Test]
        public void ProjectName()
        {
            var data = _subject.ProjectName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }
    }
}