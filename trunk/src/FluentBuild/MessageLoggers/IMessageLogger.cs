using System;

namespace FluentBuild.MessageLoggers
{
    internal interface IMessageLogger
    {
        void WriteHeader(string header);
        void WriteDebugMessage(string message);
        void Write(string type, string message);
        void WriteError(string type, string message);
        void WriteWarning(string type, string message);
        ITestSuiteMessageLogger WriteTestSuiteStared(string name);
    }

    public interface  ITestSuiteMessageLogger
    {
        ITestSuiteMessageLogger WriteTestSuiteStared(string name);
        void WriteTestSuiteFinished();
        ITestLogger WriteTestStarted(string testName);
        
    }

    public interface ITestLogger
    {
        void WriteTestPassed(TimeSpan duration);
        void WriteTestIgnored(string message);
        void WriteTestFailed(string message, string details);
    }
}