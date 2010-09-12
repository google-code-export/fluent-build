using System;
using System.IO;

namespace FluentBuild
{
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
