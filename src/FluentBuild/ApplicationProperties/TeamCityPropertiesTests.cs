using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.ApplicationProperties
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class TeamCityPropertiesTests
    {
        private IEnvironmentVariableWrapper _environmentVariableWrapper;
        private TeamCityProperties _subject;

        ///<summary>
        ///</summary>
        ///<summary />
	[SetUp]
        public void Setup()
        {
            _environmentVariableWrapper = MockRepository.GenerateStub<IEnvironmentVariableWrapper>();
            _subject = new TeamCityProperties(_environmentVariableWrapper);
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void BuildNumberShouldCallToWrapper()
        {
            var buildNumber = _subject.BuildNumber;
            _environmentVariableWrapper.AssertWasCalled(x=>x.Get(Arg<string>.Is.Anything));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void ConfigurationNameShouldCallToWrapper()
        {
            var data = _subject.ConfigurationName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void ProjectName()
        {
            var data = _subject.ProjectName;
            _environmentVariableWrapper.AssertWasCalled(x => x.Get(Arg<string>.Is.Anything));
        }
    }
}