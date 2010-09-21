using System;
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

    public class TeamCityProperties
    {
        private readonly IEnvironmentVariableWrapper _environmentVariableWrapper;

        internal TeamCityProperties() : this(new EnvironmentVariableWrapper())
        {}

        internal TeamCityProperties(IEnvironmentVariableWrapper environmentVariableWrapper)
        {
            _environmentVariableWrapper = environmentVariableWrapper;
        }

        public string BuildNumber
        {
            get { return GetEnvironmentVariable("BUILD_NUMBER"); }
        }

        public string ConfigurationName
        {
            get { return GetEnvironmentVariable("TEAMCITY_BUILDCONF_NAME"); }
        }

        public string ProjectName
        {
            get { return GetEnvironmentVariable("TEAMCITY_PROJECT_NAME"); }
        }

        private string GetEnvironmentVariable(string name)
        {
            return _environmentVariableWrapper.Get(name);
        }
    }
}