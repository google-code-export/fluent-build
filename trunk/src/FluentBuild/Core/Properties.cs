using System;
using FluentBuild.ApplicationProperties;

namespace FluentBuild.Core
{
    /// <summary>
    /// Deletes the folder if it exists. If it does not exist then no action is taken
    /// </summary>
    /// <returns>The current BuildFolder</returns>
    public class Properties
    {
        public static TeamCityProperties TeamCity
        {
            get { return new TeamCityProperties(); }
        }

        public static CruiseControlProperties CruiseControl
        {
            get { return new CruiseControlProperties(); }
        }

        public static CommandLineProperties CommandLineProperties
        {
            get { return new CommandLineProperties();}
        }

        public static string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }
    }
}