using FluentBuild.Core;

namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    internal class ConsoleTestSuiteMessageLogger : ITestSuiteMessageLogger
    {
     
        public ITestSuiteMessageLogger WriteTestSuiteStared(string name)
        {
            return MessageLogger.WriteTestSuiteStarted(name);
        }

        public void WriteTestSuiteFinished()
        {
            ConsoleMessageLogger.TestIndentation--;
        }

        public ITestLogger WriteTestStarted(string testName)
        {
            return new ConsoleTestLogger(testName);
        }
    }
}