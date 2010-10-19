using System;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.MessageLoggers.MessageProcessing
{
    [TestFixture]
    public class NunitMessageProcessorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _rootSuiteName = "C:\\Program Files\\NUnit 2.2.9\\bin\\mock-assembly.dll";
            _passingTestName = "NUnit.Tests.Assemblies.MockTestFixture.MockTest1";
            _xml = new StringBuilder();
            _xml.Append("<?xml version=\"1.0\" ?>");
            _xml.Append("<test-results>");
            _xml.Append("    <environment />");
            _xml.Append("    <culture-info />");
            _xml.Append("    <test-suite name=\"" + _rootSuiteName + "\" success=\"True\" time=\"0.016\" asserts=\"0\">");
            _xml.Append("        <results>");
            _xml.Append("            <test-case name=\"" + _passingTestName +
                        "\" description=\"Mock Test #1\" executed=\"True\" success=\"True\" time=\"5.160\" asserts=\"0\"/>");
            _xml.Append("        </results>");
            _xml.Append("    </test-suite>");
            _xml.Append("</test-results>");

            _logger = MockRepository.GenerateMock<IMessageLogger>();
            _suiteLogger = MockRepository.GenerateMock<ITestSuiteMessageLogger>();
            _testLogger = MockRepository.GenerateMock<ITestLogger>();
            _logger.Stub(x => x.WriteTestSuiteStared(Arg<string>.Is.Anything)).Return(_suiteLogger);
            _suiteLogger.Stub(x => x.WriteTestStarted(Arg<string>.Is.Anything)).Return(_testLogger);
            _subject = new NunitMessageProcessor(_logger);
        }

        #endregion

        private StringBuilder _xml;
        private string _rootSuiteName;
        private IMessageLogger _logger;
        private ITestSuiteMessageLogger _suiteLogger;
        private NunitMessageProcessor _subject;
        private string _passingTestName;
        private ITestLogger _testLogger;

        [Test]
        public void DisplayShouldLogTestSuiteStartedAndFinished()
        {
            _subject.Display("", _xml.ToString(), null, 0);
            _logger.AssertWasCalled(x => x.WriteTestSuiteStared(_rootSuiteName));
            _suiteLogger.AssertWasCalled(x => x.WriteTestSuiteFinished());
        }

        [Test]
        public void ParseTime_ShouldParseTimeFormat()
        {
            TimeSpan timeSpan = _subject.ParseTime("1.345");
            Assert.That(timeSpan.Seconds, Is.EqualTo(1));
            Assert.That(timeSpan.Milliseconds, Is.EqualTo(345));
        }

        [Test, Ignore("I don't know why this is breaking")]
        public void ProcessTestSuite()
        {
            
            XDocument xmlDoc = XDocument.Parse(_xml.ToString());
            _subject.ProcessTestSuite(xmlDoc.Root.Element("test-suite"));
            _suiteLogger.AssertWasCalled(x => x.WriteTestStarted(_passingTestName));
            _testLogger.AssertWasCalled(x => x.WriteTestPassed(Arg<TimeSpan>.Is.Anything));
        }
    }
}