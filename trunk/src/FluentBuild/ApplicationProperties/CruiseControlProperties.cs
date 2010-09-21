using System;
using FluentBuild.Core;

namespace FluentBuild.ApplicationProperties
{
    //Cruise control passes parameters via the command line
    public class CruiseControlProperties
    {
        
        public string ProjectName
        {
            get { return GetValue("projectname"); }
        }

        public string LastBuild
        {
            get { return GetValue("lastbuild"); }
        }

        public string LastSuccessfulBuild
        {
            get { return GetValue("lastsuccessfulbuild"); }
        }

        public DateTime BuildDate
        {
            get { return DateTime.Parse(GetValue("builddate")); }
        }

        public string Timestamp
        {
            get { return GetValue("cctimestamp"); }
        }

        public string Label
        {
            get { return GetValue("label"); }
        }

        public int Interval
        {
            get
            {
                return int.Parse(GetValue("interval"));
            }
        }

        public bool LastBuildSuccessful
        {
            get { return bool.Parse(GetValue("lastbuildsuccessful")); }
        }

        public string LogDirectory
        {
            get { return GetValue("logdir"); }
        }

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