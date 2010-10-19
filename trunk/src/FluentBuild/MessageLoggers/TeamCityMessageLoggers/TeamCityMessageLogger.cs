using System;
using FluentBuild.Core;

namespace FluentBuild.MessageLoggers
{
    internal class TeamCityMessageLogger : IMessageLogger
    {
        internal string _currentHeader;

        #region IMessageLogger Members

        public void WriteHeader(string header)
        {
            if (!String.IsNullOrEmpty(_currentHeader))
                Console.WriteLine(String.Format("##teamcity[blockClosed name='{0}']", _currentHeader));

            _currentHeader = header;
            Console.WriteLine(String.Format("##teamcity[blockOpened name='{0}']", header));
        }

        public void WriteDebugMessage(string message)
        {
            Write("DEBUG", message);
        }

        public void Write(string type, string message)
        {
            string outputMessage = String.Format("[{0}] {1}", type, message);
            WriteMessage(outputMessage);
        }

        public void WriteError(string type, string message)
        {
            WriteMessage(message,message, "ERROR");
        }

        public void WriteWarning(string type, string message)
        {
            var outputMessage = String.Format("[{0}] {1}", type, message);
            WriteMessage(outputMessage, string.Empty, "WARNING");
        }

        public ITestSuiteMessageLogger WriteTestSuiteStared(string name)
        {
            return new TeamCityTestSuiteMessageLogger(name);
        }

        #endregion

        private static void WriteMessage(string message)
        {
            WriteMessage(message, string.Empty, "NORMAL");
        }

        private static void WriteMessage(string message, string error, string type)
        {
            //NORMAL, WARNING, FAILURE, ERROR
            message = EscapeCharacters(message);
            error = EscapeCharacters(error);
            Console.WriteLine(String.Format("##teamcity[message text='{0}' errorDetails='{1}' status='{2}']", message,
                                            error, type));
        }

        internal static string EscapeCharacters(string data)
        {
            return data.Replace("|", "||").Replace("'", "|'").Replace("\n", "|n").Replace("\r", "|r").Replace("]", "|]");
        }
    }

    internal class TeamCityTestSuiteMessageLogger : ITestSuiteMessageLogger
    {
        private readonly string _name;

        public TeamCityTestSuiteMessageLogger(string name)
        {
            Console.WriteLine(String.Format("##teamcity[testSuiteStarted name='{0}']", TeamCityMessageLogger.EscapeCharacters(name)));
            _name = name;
        }

        public ITestSuiteMessageLogger WriteTestSuiteStared(string name)
        {
            return new TeamCityTestSuiteMessageLogger(name);
        }

        public void WriteTestSuiteFinished()
        {
            Console.WriteLine(String.Format("##teamcity[testSuiteFinished name='{0}']", TeamCityMessageLogger.EscapeCharacters(_name)));
        }

        public ITestLogger WriteTestStarted(string testName)
        {
            Console.WriteLine(String.Format("##teamcity[testStarted name='{0}']", TeamCityMessageLogger.EscapeCharacters(testName)));
            return new TeamCityTestMessageLogger(testName);
        }
    }

    internal class TeamCityTestMessageLogger : ITestLogger
    {
        private readonly string _testName;

        public TeamCityTestMessageLogger(string testName)
        {
            _testName = testName;
        }

        private void WriteTestFinished(double duration)
        {
            Console.WriteLine(String.Format("##teamcity[testFinished name='{0}' duration='{1}']", TeamCityMessageLogger.EscapeCharacters(_testName), duration + "ms"));
        }

        public void WriteTestPassed(TimeSpan duration)
        {
            WriteTestFinished(duration.TotalMilliseconds);            
        }

        public void WriteTestIgnored(string message)
        {
            Console.WriteLine(String.Format("##teamcity[testIgnored name='{0}' message='{1}']", TeamCityMessageLogger.EscapeCharacters(_testName),TeamCityMessageLogger.EscapeCharacters(message)));
            WriteTestFinished(0);
        }

        public void WriteTestFailed(string message, string details)
        {
            Console.WriteLine(String.Format("##teamcity[testFailed name='{0}' message='{1}' details='{2}']", TeamCityMessageLogger.EscapeCharacters(_testName),
                                            TeamCityMessageLogger.EscapeCharacters(message), TeamCityMessageLogger.EscapeCharacters(details)));
            WriteTestFinished(0);
        }
    }
}