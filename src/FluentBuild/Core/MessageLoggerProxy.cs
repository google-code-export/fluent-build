using System;
using FluentBuild.MessageLoggers;

namespace FluentBuild.Core
{
    public class MessageLoggerProxy : IMessageLogger
    {
        internal IMessageLogger InternalLogger;

        public VerbosityLevel Verbosity { get; set; }
        public MessageLoggerProxy(IMessageLogger internalLogger)
        {
            InternalLogger = internalLogger;
        }

        public void WriteHeader(string header)
        {
            if (Verbosity >= VerbosityLevel.TaskNamesOnly)
            {
                InternalLogger.WriteHeader(header);
            }
        }

        public void WriteDebugMessage(string message)
        {
            if (Verbosity >= VerbosityLevel.Full)
                InternalLogger.WriteDebugMessage(message);
        }

        public void Write(string type, string message)
        {
            InternalLogger.Write(type, message);
        }

        public void WriteError(string type, string message)
        {
            InternalLogger.WriteError(type, message);
        }

        public void WriteWarning(string type, string message)
        {
            InternalLogger.WriteWarning(type, message);
        }

        public IDisposable ShowDebugMessages
        {
            get { return new DebugMessages(this); }
        }

        public ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            return InternalLogger.WriteTestSuiteStarted(name);
        }
    }
}