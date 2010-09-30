using System;
using FluentBuild.Utilities;

namespace FluentBuild.ApplicationProperties
{
    ///<summary>
    /// Access common properties set by team city
    ///</summary>
    public class TeamCityProperties
    {
        private readonly IEnvironmentVariableWrapper _environmentVariableWrapper;

        internal TeamCityProperties() : this(new EnvironmentVariableWrapper())
        {}

        internal TeamCityProperties(IEnvironmentVariableWrapper environmentVariableWrapper)
        {
            _environmentVariableWrapper = environmentVariableWrapper;
        }

        ///<summary>
        /// The BuildNumber that team city has determined for this build
        ///</summary>
        public string BuildNumber
        {
            get { return GetEnvironmentVariable("BUILD_NUMBER"); }
        }

        ///<summary>
        /// The configuration being used for this build
        ///</summary>
        public string ConfigurationName
        {
            get { return GetEnvironmentVariable("TEAMCITY_BUILDCONF_NAME"); }
        }

        ///<summary>
        /// The project being built
        ///</summary>
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