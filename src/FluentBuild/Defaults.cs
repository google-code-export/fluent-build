using System;
using System.Collections.Generic;

using FluentBuild.MessageLoggers;
using FluentBuild.MessageLoggers.ConsoleMessageLoggers;
using FluentBuild.Utilities;

namespace FluentBuild
{
    ///<summary>
    /// Defaults for the fluent build runner
    ///</summary>
    public static class Defaults
    {
        ///<summary>
        /// Sets the behavior of what to do when an error occurs. The default is to fail.
        ///</summary>
        public static OnError OnError = OnError.Fail;

        ///<summary>
        /// Sets the .NET Framework version to use. The default is the highest desktop framework found.
        ///</summary>
        public static IFrameworkVersion FrameworkVersion;

        static Defaults()
        {
            var frameworkVersionsToCheck = new List<IFrameworkVersion>();
            frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET4_0.Full);
            frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET4_0.Client);
            frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET3_5);
            frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET3_0);
            frameworkVersionsToCheck.Add(Utilities.FrameworkVersion.NET2_0);

            foreach (var frameworkVersion in frameworkVersionsToCheck)
            {
                if (frameworkVersion.IsFrameworkInstalled())
                {
                    Defaults.FrameworkVersion = frameworkVersion;
                    return;
                }
            }
        }

        private static IMessageLogger _logger = new MessageLoggerProxy(new ConsoleMessageLogger());
        public static IMessageLogger Logger { get { return _logger;  } }

        public static void SetLogger(string logger)
        {
            switch (logger.ToUpper())
            {
                case "CONSOLE":
                    _logger = new MessageLoggerProxy(new ConsoleMessageLogger());
                    break;
                case "TEAMCITY":
                    _logger = new MessageLoggerProxy(new MessageLoggers.TeamCityMessageLoggers.MessageLogger());
                    break;
                default:
                    throw new ArgumentException("logger type " + logger + " unkown.");
            }
        }

        public static void SetLogger(IMessageLogger logger)
        {
            _logger = new MessageLoggerProxy(logger);
        }
    }
}