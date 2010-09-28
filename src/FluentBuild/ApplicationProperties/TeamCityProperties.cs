using System;
using FluentBuild.Utilities;

namespace FluentBuild.ApplicationProperties
{
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