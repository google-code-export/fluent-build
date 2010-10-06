using System;
using FluentBuild.MessageLoggers;

namespace FluentBuild.Core
{
    public class MessageLogger
    {
        internal static IMessageLogger InternalLogger;

        static MessageLogger()
        {
            InternalLogger = new TeamCityMessageLogger();
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
            InternalLogger.WriteError(message);
        }
    }
}