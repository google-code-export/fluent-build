using System;
using System.Collections.Generic;
using System.IO;

namespace FluentBuild.Core
{
    public class MessageLogger
    {
        //[DllImport("kernel32.dll")]
        //public static extern IntPtr GetStdHandle(uint nStdHandle);

        private static int _windowWidth;

        internal static int WindowWidth
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
                    catch (IOException e) //if the output is redirected to a stream then getting the width will fail
                    {
                        _windowWidth = 80;
                    }
                return _windowWidth;
            }
            set { _windowWidth = value; }
        }

        ///<summary>
        /// Gets or sets the message logging verbosity level
        ///</summary>
        public static VerbosityLevel Verbosity { get; set; }

        internal class DebugMessages : IDisposable
        {
            private VerbosityLevel _originalVerbosity;

            public DebugMessages()
            {
                _originalVerbosity = MessageLogger.Verbosity;
                MessageLogger.Verbosity = VerbosityLevel.Full;
            }

            public void Dispose()
            {
                MessageLogger.Verbosity = _originalVerbosity;
            }
        }

        public static IDisposable ShowDebugMessages
        {
            get { return new DebugMessages(); }
        }


        public static void WriteHeader(string header)
        {
            if (Verbosity >= VerbosityLevel.TaskNamesOnly)
            {
                Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Purple);
                Console.WriteLine(header);
                Utilities.ConsoleColor.SetColor(Utilities.ConsoleColor.BuildColor.Default);
            }
        }

        public static void WriteDebugMessage(string message)
        {
            if (Verbosity >= VerbosityLevel.Full)
                Write("DEBUG", message);
        }

        internal static List<String> WrapText(int leftColumnStartsAtPostion, string message)
        {
            var maxLengthOfMessage = WindowWidth - 1; //add some padding on the right
            if (message.Length <= maxLengthOfMessage)
                return new List<string>() {message};


            var lines = new List<string>();
            lines.Add(message.Substring(0, maxLengthOfMessage)); //add the line with the prefix

            var remainingText = message.Substring(maxLengthOfMessage); //cut out the string we already put into lines
            while(remainingText.Length > 0)
            {
                //create a line that has room for left indent
                var lengthToEndOfline = maxLengthOfMessage - leftColumnStartsAtPostion;
                //if the length we calculate is longer than the remaining text
                //then just take the length of the remaning text
                if (lengthToEndOfline > remainingText.Length) 
                    lengthToEndOfline = remainingText.Length;
                var line = remainingText.Substring(0, lengthToEndOfline);
                var padding = "".PadLeft(leftColumnStartsAtPostion, ' ');
                lines.Add(padding + line);
                //shrink down remaining text
                remainingText = remainingText.Substring(lengthToEndOfline);
            }
            return lines;
        }

        public static void Write(string type, string message)
        {
            string outputMessage = String.Format("  [{0}] {1}", type, message);
            var wrapText = WrapText(5 + type.Length, outputMessage);
            foreach (var text in wrapText)
            {
                Console.WriteLine(text);
            }
        }

        public static void BlankLine()
        {
            Console.WriteLine();
        }
    }
}