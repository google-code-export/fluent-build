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
        
        ///<summary />
	[Test]
        public void WriteHeader()
        {
            MessageLogger.WriteHeader("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("test" + Environment.NewLine));
        }

        ///<summary />
	[Test]
        public void WriteDebugMessage_ShouldWriteIfDebugTurnedOn()
        {
            MessageLogger.ShowDebugMessages = true;
            MessageLogger.WriteDebugMessage("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("\t[DEBUG] test" + Environment.NewLine));
        }

        ///<summary />
	[Test]
        public void WriteDebugMessage_ShouldNotWriteIfDebugTurnedOff()
        {
            MessageLogger.ShowDebugMessages = false;
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
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("\t[TEST] Content of message" + Environment.NewLine));
        }

        ///<summary />
	[Test]
        public void Write_ShouldWrapLongLines()
        {
            //width is set to 80 characters in setup
            MessageLogger.Write("TEST", "Content of message that is really really really long and it just keeps on going");
            _textMessageWriter.Flush();
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("\t[TEST] Content of message that is really really really long \r\n\t       and it just keeps on going\r\n"));
        }


    }
}