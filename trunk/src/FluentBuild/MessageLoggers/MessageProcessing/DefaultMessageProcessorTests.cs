using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FluentBuild.MessageLoggers.MessageProcessing
{
    [TestFixture]
    public class DefaultMessageProcessorTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _subject = new DefaultMessageProcessor();
        }

        #endregion

        private DefaultMessageProcessor _subject;

        [Test]
        public void ShouldHaveAllMessagesAsErrorWhenProcessErrorCodeNonZero()
        {
            IList<Message> messages = _subject.Parse("prefix", "I did something", "", 1);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Error));
        }

        [Test]
        public void ShouldHaveMessagesAsRegular()
        {
            IList<Message> messages = _subject.Parse("prefix", "I did something", "", 0);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Regular));
        }

        [Test]
        public void ShouldHaveNonErrorAndErrorMessage()
        {
            IList<Message> messages = _subject.Parse("prefix", "I did something", "I failed on something", 0);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Regular));
            Assert.That(messages[1].MessageType, Is.EqualTo(MessageType.Error));
        }

        [Test]
        public void ShouldHaveWarningLine()
        {
            IList<Message> messages =
                _subject.Parse("prefix", "I did something" + Environment.NewLine + "Warning: something is broken", "", 0);
            Assert.That(messages[0].MessageType, Is.EqualTo(MessageType.Regular));
            Assert.That(messages[1].MessageType, Is.EqualTo(MessageType.Warning));
        }
    }
}