using System;
using FluentBuild.Core;

namespace FluentBuild.MessageLoggers.ConsoleMessageLoggers
{
    internal class ConsoleTestLogger : ITestLogger
    {
        private readonly string _testName;


        public ConsoleTestLogger(string testName)
        {
            _testName = testName.Substring(testName.LastIndexOf(".") +1); //strip out the suite name from the test name
        }

        public void WriteTestPassed(TimeSpan duration)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Green);
            WriteMessage(_testName, "Passed " + duration.TotalSeconds.ToString("N3") + "s");
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
        }

        private void WriteMessage(string name, string data)
        {
            var remainingWidth = ConsoleMessageLogger.WindowWidth-10 - ConsoleMessageLogger.TestIndentation - name.Length - data.Length;
            if (remainingWidth <=0)
            {
                remainingWidth = 0;
            }
            MessageLogger.Write("TEST", "".PadRight(ConsoleMessageLogger.TestIndentation, ' ') + name + "".PadRight(remainingWidth, '.')  + data);
        }

        public void WriteTestIgnored(string message)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Yellow);
            WriteMessage(_testName, "Ignored");
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
        }

        public void WriteTestFailed(string message, string details)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Red);
            WriteMessage(_testName, "Failed");
            MessageLogger.Write("Details", message);
            MessageLogger.Write("StackTrace", details);
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
        }
    }
}