using System;
using System.Collections.Generic;
using System.IO;
using FluentBuild.Core;

namespace FluentBuild.MessageLoggers
{
    internal class ConsoleMessageLogger : IMessageLogger
    {
        internal List<string> WrapText(int leftColumnStartsAtPostion, string message)
        {
            var maxLengthOfMessage = WindowWidth - 1; //add some padding on the right
            if (message.Length <= maxLengthOfMessage)
                return new List<string> { message };

            //split be newline, then wrap each line
            var lines = new List<string>();
            var headerWritten = false;
            foreach (var linesInMessage in message.Split(Environment.NewLine.ToCharArray()))
            {
                string remainingText;
                if (!headerWritten)
                {
                    lines.Add(linesInMessage.Substring(0, maxLengthOfMessage)); //add the line with the prefix
                    remainingText = linesInMessage.Substring(maxLengthOfMessage); //cut out the string we already put into lines
                    headerWritten = true;
                }
                else
                {
                    remainingText = linesInMessage;
                }

                while (remainingText.Length > 0)
                {
                    //create a line that has room for left indent
                    var lengthToEndOfline = maxLengthOfMessage - leftColumnStartsAtPostion;
                    //if the length we calculate is longer than the remaining text
                    //then just take the length of the remaning text
                    if (lengthToEndOfline > remainingText.Length)
                        lengthToEndOfline = remainingText.Length;
                    var line = remainingText.Substring(0, lengthToEndOfline).Trim();
                    var padding = "".PadLeft(leftColumnStartsAtPostion, ' ');
                    lines.Add(padding + line);
                    //shrink down remaining text
                    remainingText = remainingText.Substring(lengthToEndOfline);
                }

            }

            return lines;
        }

        
        private static int _windowWidth;

        internal int WindowWidth
        {
            get
            {
                //this check is done to allow us to redirect the output handle to a textwriter
                //without getting an invalid handle
                //this is done by having our unit tests set the width before it runs
                if (_windowWidth == 0)
                    try
                    {
                        _windowWidth = Console.WindowWidth;
                    }
                    catch (IOException) //if the output is redirected to a stream then getting the width will fail
                    {
                        _windowWidth = 80;
                    }
                return _windowWidth;
            }
            set { _windowWidth = value; }
        }

        public void WriteHeader(string header)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Purple);
            Console.WriteLine(header);
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);

        }

        public void WriteDebugMessage(string message)
        {
            Write("DEBUG", message);
        }

        public void Write(string type, string message)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
            string outputMessage = String.Format("  [{0}] {1}", type, message);
            var wrapText = WrapText(5 + type.Length, outputMessage);
            foreach (var text in wrapText)
            {
                Console.WriteLine(text);
            }
        }

        public void WriteError(string type, string message)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Red);
            Write(type, message);
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
        }

        public void WriteWarning(string type, string message)
        {
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Yellow);
            Write(type, message);
            Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
        }
    }
}