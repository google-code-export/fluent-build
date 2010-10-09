using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers
{
    [TestFixture]
    public class TeamCityMessageLoggerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);
            _subject = new TeamCityMessageLogger();
        }

        #endregion

        private TeamCityMessageLogger _subject;
        private TextMessageWriter _textMessageWriter;

        [Test]
        public void WriteHeader_ShouldOpenNewHeaderIfNothingElseHasBeenOpened()
        {
            var header = "test";
            _subject.WriteHeader(header);
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(String.Format("##teamcity[blockOpened name='{0}']" + Environment.NewLine, header)));            
        }

        [Test]
        public void Write_ShouldCreateMessage()
        {   
            _subject.Write("test", "this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='[test|] this is a test' errorDetails='' status='NORMAL']\r\n"));
        }

        [Test]
        public void Debug_ShouldCreateMessage()
        {
            _subject.WriteDebugMessage("this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='[DEBUG|] this is a test' errorDetails='' status='NORMAL']\r\n"));
        }

        [Test]
        public void Error_ShouldCreateMessage()
        {
            _subject.WriteError("CSC", "this is a test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("##teamcity[message text='this is a test' errorDetails='this is a test' status='ERROR']\r\n"));
        }

        [Test]
        public void WriteHeader_ShouldCloseOldHeaderIfItExists()
        {
            var newHeader = "NewHeader";
            var oldHeader = "OldHeader";
            _subject._currentHeader = oldHeader;
            _subject.WriteHeader(newHeader);
            var expected = String.Format("##teamcity[blockClosed name='{0}']", oldHeader) +
                           Environment.NewLine +
                           String.Format("##teamcity[blockOpened name='{0}']", newHeader) +
                           Environment.NewLine;
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(expected));
        }


        [Test]
        public void EscapeCharacters_ShouldEscapeCharacters()
        {
            var itemsToTest = new NameValueCollection();

            itemsToTest.Add("|", "||");
            itemsToTest.Add("'", "|'");
            itemsToTest.Add("\n", "|n");
            itemsToTest.Add("\r", "|r");
            itemsToTest.Add("]", "|]");

            foreach (string key in itemsToTest.Keys)
            {
                Assert.That(TeamCityMessageLogger.EscapeCharacters(key), Is.EqualTo(itemsToTest[key]));
            }
        }

        [Test]
        public void EscapeCharacters_ShouldNotEscapePipeRepeatedly()
        {
            string escapeCharacters = TeamCityMessageLogger.EscapeCharacters("\n|");
            Assert.That(escapeCharacters, Is.EqualTo("|n||"));
        }
    }

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
}