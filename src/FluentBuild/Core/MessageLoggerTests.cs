using System;
using NUnit.Framework;

namespace FluentBuild.Core
{
    ///<summary />
	[TestFixture]
    public class MessageLoggerTests
    {
        private TextMessageWriter _textMessageWriter;

        ///<summary />
	[SetUp]
        public void Setup()
        {
            //as output is redirected the window width is invalid now so we set it manually
            MessageLogger.WindowWidth = 80;
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);   
        }

        [Test]
        public void WrapText_ShouldHaveOneLine()
        {
            MessageLogger.WindowWidth = 22;
            var wrapText = MessageLogger.WrapText(0, "hello");
            Assert.That(wrapText[0], Is.EqualTo("hello"));
        }

        [Test]
        public void WrapText_ShouldHaveTwoLines()
        {
            MessageLogger.WindowWidth = 22;
            var wrapText = MessageLogger.WrapText(9, "  [exec] hello world how are you");
            Assert.That(wrapText[0], Is.EqualTo("  [exec] hello world "));
            Assert.That(wrapText[1], Is.EqualTo("         how are you"));
        }
        
        ///<summary />
	[Test]
        public void WriteHeader()
        {
            MessageLogger.WriteHeader("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("test" + Environment.NewLine));
        }

        ///<summary />
	    [Test]
        public void WriteDebugMessage_ShouldWriteIfVerbosityIsFull()
        {
            MessageLogger.Verbosity = VerbosityLevel.Full;
            MessageLogger.WriteDebugMessage("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [DEBUG] test" + Environment.NewLine));
        }

        [Test]
        public void UsingDebug_ShouldOnlyWriteOneDebugMessage()
        {
            MessageLogger.Verbosity = VerbosityLevel.TaskDetails;
            MessageLogger.WriteDebugMessage("test1");
            using(MessageLogger.ShowDebugMessages)
            {
                MessageLogger.WriteDebugMessage("test2");
            }
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [DEBUG] test2" + Environment.NewLine));
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
            Assert.That(_textMessageWriter.ToString(), Is.Not.EqualTo("test" + Environment.NewLine));
        }

        ///<summary />
	[Test]
        public void WriteBlankLine_ShouldCreateNewLine()
        {
            MessageLogger.BlankLine();
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(Environment.NewLine));
        }

        ///<summary />
	[Test]
        public void Write_ShouldCreateProperlyIndentedLines()
        {
            MessageLogger.Write("TEST", "Content of message");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("  [TEST] Content of message" + Environment.NewLine));
        }

        ///<summary />
    }
}