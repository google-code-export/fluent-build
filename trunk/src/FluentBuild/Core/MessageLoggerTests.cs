using System;
using FluentBuild.MessageLoggers;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Core
{
    [TestFixture]
    public class MessageLoggerTests
    {
        ///<summary />
        [SetUp]
        public void Setup()
        {
            MessageLogger.InternalLogger = MockRepository.GenerateMock<IMessageLogger>();
        }

        [Test]
        public void UsingDebug_ShouldOnlyWriteOneDebugMessage()
        {
            MessageLogger.Verbosity = VerbosityLevel.TaskDetails;
            MessageLogger.WriteDebugMessage("test1");
            using (MessageLogger.ShowDebugMessages)
            {
                MessageLogger.WriteDebugMessage("test2");
            }
            MessageLogger.InternalLogger.AssertWasCalled(x=>x.WriteDebugMessage("test2"));
        }

        [Test]
        public void UsingDebug_DebugLevelsSholdChange()
        {
            MessageLogger.Verbosity = VerbosityLevel.TaskDetails;
            using (MessageLogger.ShowDebugMessages)
            {
                Assert.That(MessageLogger.Verbosity, Is.EqualTo(VerbosityLevel.Full));
            }
            Assert.That(MessageLogger.Verbosity, Is.EqualTo(VerbosityLevel.TaskDetails));
        }

        ///<summary />
        [Test]
        public void WriteDebugMessage_ShouldNotWriteIfVerbosityIsLessThanFull()
        {
            MessageLogger.Verbosity = VerbosityLevel.TaskDetails;
            MessageLogger.WriteDebugMessage("test");
            MessageLogger.InternalLogger.AssertWasNotCalled(x=>x.WriteDebugMessage(Arg<String>.Is.Anything));
        }

        ///<summary />
        [Test]
        public void WriteDebugMessage_ShouldWriteIfVerbosityIsFull()
        {
            MessageLogger.Verbosity = VerbosityLevel.Full;
            MessageLogger.WriteDebugMessage("test");
            MessageLogger.InternalLogger.AssertWasCalled(x=>x.WriteDebugMessage("test"));
        }

        [Test]
        public void WriteHeader_ShouldWriteIfVerbosityIsFull()
        {
            MessageLogger.Verbosity = VerbosityLevel.Full;
            MessageLogger.WriteHeader("test");
            MessageLogger.InternalLogger.AssertWasCalled(x => x.WriteHeader("test"));
        }

        [Test]
        public void WriteHeader_ShouldNotWriteIfVerbosityIsNone()
        {
            MessageLogger.Verbosity = VerbosityLevel.None;
            MessageLogger.WriteHeader("test");
            MessageLogger.InternalLogger.AssertWasNotCalled(x => x.WriteHeader("test"));
        }

        [Test]
        public void WriteError_ShouldWriteIfVerbosityIsNone()
        {
            MessageLogger.Verbosity = VerbosityLevel.None;
            MessageLogger.WriteError("test");
            MessageLogger.InternalLogger.AssertWasCalled(x => x.WriteError("test"));
        }

    }
}