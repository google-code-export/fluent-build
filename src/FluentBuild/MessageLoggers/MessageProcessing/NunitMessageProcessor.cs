using System;
using System.Xml.Linq;
using FluentBuild.Core;

namespace FluentBuild.MessageLoggers.MessageProcessing
{
    internal class NunitMessageProcessor : IMessageProcessor
    {
        private readonly IMessageLogger _logger;

        public NunitMessageProcessor(IMessageLogger logger)
        {
            _logger = logger;
        }

        public NunitMessageProcessor()
        {
            _logger = MessageLogger.InternalLogger;
        }

        #region IMessageProcessor Members

        public void Display(string prefix, string output, string error, int exitCode)
        {
            //xml message is proceeded with some other console info
            output = output.Substring(output.IndexOf("<?xml"));
            XDocument xmlDoc = XDocument.Parse(output);
            if (xmlDoc.Root == null)
                return;

            foreach (XElement testSuite in xmlDoc.Root.Elements("test-suite"))
            {
                ProcessTestSuite(testSuite);
            }

            _logger.Write("TEST", String.Format("Run completed. Successfull: {0}  Failed: {1}  Ignored: {2}", successfull, failed,ignored));
        }

        #endregion

        private int successfull;
        private int ignored;
        private int failed;

        internal void ProcessTestSuite(XElement testSuite)
        {
            ITestSuiteMessageLogger testSuiteMessageLogger =_logger.WriteTestSuiteStared((string) testSuite.Attribute("name"));
            foreach (var results in testSuite.Elements("results"))
            {
                foreach (var testCase in results.Elements("test-case"))
                {
                    var logger = testSuiteMessageLogger.WriteTestStarted((string) testCase.Attribute("name"));
                    switch ((string)testCase.Attribute("result"))
                    {
                        case "Success":
                            successfull++;
                            logger.WriteTestPassed(ParseTime((string)testCase.Attribute("time")));
                            break;
                        case "Error":
                        case "Failure":
                            failed++;
                            WriteFailure(logger, testCase);
                            break;
                        case "Ignored":
                            ignored++;
                            WriteIgnored(logger, testCase);
                            break;
                    }
                }

                foreach (var childSuite in results.Elements("test-suite"))
                {
                    ProcessTestSuite(childSuite);
                }
            }
            
            testSuiteMessageLogger.WriteTestSuiteFinished();
        }

        private void WriteIgnored(ITestLogger logger, XElement testCase)
        {
            var failureElement = testCase.Element("reason").Element("message");
            logger.WriteTestIgnored((string) failureElement);
        }

        private void WriteFailure(ITestLogger logger, XElement testCase)
        {
            var failureElement = testCase.Element("failure");
            logger.WriteTestFailed((string) failureElement.Element("message"), (string) failureElement.Element("stack-trace"));
        }

        internal TimeSpan ParseTime(string time)
        {
            int seconds = int.Parse(time.Substring(0, time.IndexOf(".")));
            int milliseconds = int.Parse(time.Substring(time.IndexOf(".") + 1));
            return new TimeSpan(0, 0, 0, seconds, milliseconds);
        }
    }
}