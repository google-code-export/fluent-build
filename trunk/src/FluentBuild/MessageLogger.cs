using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace FluentBuild
{
    [TestFixture]
    public class MessageLoggerTests
    {
        private TextMessageWriter _textMessageWriter;

        [SetUp]
        public void Setup()
        {
            //as output is redirected the window width is invalid now so we set it manually
            MessageLogger.WindowWidth = 80;
            _textMessageWriter = new TextMessageWriter();
            Console.SetOut(_textMessageWriter);   
        }
        
        [Test]
        public void WriteHeader()
        {
            MessageLogger.WriteHeader("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("test" + Environment.NewLine));
        }

        [Test]
        public void WriteDebugMessage_ShouldWriteIfDebugTurnedOn()
        {
            MessageLogger.ShowDebugMessages = true;
            MessageLogger.WriteDebugMessage("test");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("\t[DEBUG] test" + Environment.NewLine));
        }

        [Test]
        public void WriteDebugMessage_ShouldNotWriteIfDebugTurnedOff()
        {
            MessageLogger.ShowDebugMessages = false;
            MessageLogger.WriteDebugMessage("test");
            Assert.That(_textMessageWriter.ToString(), Is.Not.EqualTo("test" + Environment.NewLine));
        }

        [Test]
        public void WriteBlankLine_ShouldCreateNewLine()
        {
            MessageLogger.BlankLine();
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo(Environment.NewLine));
        }

        [Test]
        public void Write_ShouldCreateProperlyIndentedLines()
        {
            MessageLogger.Write("TEST", "Content of message");
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("\t[TEST] Content of message" + Environment.NewLine));
        }

        [Test]
        public void Write_ShouldWrapLongLines()
        {
            //width is set to 80 characters in setup
            MessageLogger.Write("TEST", "Content of message that is really really really long and it just keeps on going");
            _textMessageWriter.Flush();
            Assert.That(_textMessageWriter.ToString(), Is.EqualTo("\t[TEST] Content of message that is really really really long \r\n\t       and it just keeps on going\r\n"));
        }


    }

    public class MessageLogger
    {
        public static bool ShowDebugMessages;
        private static int _windowWidth;
        internal static int WindowWidth
        {
            get {
                //this check is done to allow us to redirect the output handle to a textwriter
                //without getting an invalid handle
                //this is done by having our unit tests set the width before it runs
                if (_windowWidth == 0)
                    _windowWidth = Console.WindowWidth;
                return _windowWidth; 
            }
            set { _windowWidth = value; }
        }


        public static void WriteHeader(string header)
        {
            Console.WriteLine(header);
        }

        public static void WriteDebugMessage(string message)
        {
            if (ShowDebugMessages)
                Write("DEBUG", message);
        }

        public static void Write(string type, string message)
        {
            string outputMessage = String.Format("[{0}] {1}", type, message);
            do
            {
                int length = 0;
                if (outputMessage.Length > WindowWidth - 20)
                    length = WindowWidth - 20;
                else
                    length = outputMessage.Length;

                //ugly indentation code. Better way to do it but brain hurts
                if (!outputMessage.Substring(0, length).Contains(String.Format("[{0}]", type)))
                {
                    Console.WriteLine("\t".PadRight(type.Length+4, ' ') + outputMessage.Substring(0, length));
                }
                else
                {
                    Console.WriteLine("\t" + outputMessage.Substring(0, length));
                }
                outputMessage = outputMessage.Substring(length);
            } while (outputMessage.Trim().Length > 0);
        }

        public static void BlankLine()
        {
            Console.WriteLine();
        }
    }
}
