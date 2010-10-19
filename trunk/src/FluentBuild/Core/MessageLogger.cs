using System;
using FluentBuild.MessageLoggers;
using FluentBuild.MessageLoggers.ConsoleMessageLoggers;

namespace FluentBuild.Core
{
    public class MessageLogger
    {
        internal static IMessageLogger InternalLogger;

        static MessageLogger()
        {
            InternalLogger = new ConsoleMessageLogger();
        }

        ///<summary>
        /// Gets or sets the message logging verbosity level
        ///</summary>
        public static VerbosityLevel Verbosity { get; set; }
        internal class DebugMessages : IDisposable
        {
            private VerbosityLevel _originalVerbosity;

            public DebugMessages()
            {
                _originalVerbosity = MessageLogger.Verbosity;
                MessageLogger.Verbosity = VerbosityLevel.Full;
            }

            public void Dispose()
            {
                MessageLogger.Verbosity = _originalVerbosity;
            }
        }

        public static IDisposable ShowDebugMessages
        {
            get { return new DebugMessages(); }
        }

        public static ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            return InternalLogger.WriteTestSuiteStared(name);
        }

        public static void WriteHeader(string header)
        {
            if (Verbosity >= VerbosityLevel.TaskNamesOnly)
            {
                InternalLogger.WriteHeader(header);
            }
        }


        public static void WriteDebugMessage(string message)
        {
            if (Verbosity >= VerbosityLevel.Full)
                InternalLogger.WriteDebugMessage(message);
        }


        public static void Write(string type, string message)
        {
            InternalLogger.Write(type, message);
        }
        
        public static void WriteError(string message)
        {
            InternalLogger.WriteError("ERROR", message);
        }

        public static void WriteError(string prefix, string message)
        {
            InternalLogger.WriteError(prefix, message);
        }

        public static void WriteWarning(string prefix, string message)
        {
            InternalLogger.WriteWarning(prefix, message);
        }

        public static void SetLogger(string logger)
        {
            switch (logger.ToUpper())
            {
                case "CONSOLE":
                    InternalLogger = new ConsoleMessageLogger();
                    break;
                case "TEAMCITY":
                    InternalLogger = new TeamCityMessageLogger();
                    break;
                default:
                    throw new ArgumentException("logger type " + logger + " unkown.");
            }
            
        }
    }
}