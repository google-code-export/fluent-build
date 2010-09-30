using System;
using FluentBuild.Core;

namespace FluentBuild.ApplicationProperties
{
    ///<summary>
    /// Accesses a set list of cruise control properties.
    ///</summary>
    public class CruiseControlProperties
    {
        
        ///<summary>
        /// Gets the project name
        ///</summary>
        public string ProjectName
        {
            get { return GetValue("projectname"); }
        }

        ///<summary>
        /// Gets the ID number of the last build
        ///</summary>
        public string LastBuild
        {
            get { return GetValue("lastbuild"); }
        }

        ///<summary>
        /// Gets the ID of the last successful build
        ///</summary>
        public string LastSuccessfulBuild
        {
            get { return GetValue("lastsuccessfulbuild"); }
        }

        ///<summary>
        /// Returns the build date
        ///</summary>
        public DateTime BuildDate
        {
            get { return DateTime.Parse(GetValue("builddate")); }
        }

        ///<summary>
        /// Gets the cctimestamp property
        ///</summary>
        public string Timestamp
        {
            get { return GetValue("cctimestamp"); }
        }

        ///<summary>
        /// Gets the cruise control label
        ///</summary>
        public string Label
        {
            get { return GetValue("label"); }
        }

        ///<summary>
        /// Gets the interval property
        ///</summary>
        public int Interval
        {
            get
            {
                return int.Parse(GetValue("interval"));
            }
        }

        ///<summary>
        /// Wether the last build that was run was successful
        ///</summary>
        public bool LastBuildSuccessful
        {
            get { return bool.Parse(GetValue("lastbuildsuccessful")); }
        }

        ///<summary>
        /// The directory that cruise control logs to
        ///</summary>
        public string LogDirectory
        {
            get { return GetValue("logdir"); }
        }

        ///<summary>
        /// The file that cruise control logs to
        ///</summary>
        public string LogFile
        {
            get { return GetValue("logfile"); }
        }

        private string GetValue(string name)
        {
            return Properties.CommandLineProperties.GetProperty(name);
        }
    }
}